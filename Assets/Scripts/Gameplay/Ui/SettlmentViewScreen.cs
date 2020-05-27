using System;
using System.Collections.Generic;
using UnityEngine;
using Domination.EventsSystem;
using UnityEngine.UI;
using Domination.Warfare;
using TMPro;


namespace Domination.Ui
{
    public class SettlmentViewScreen : UiUnit<object>
    {
        private enum Mode
        {
            Construction,
            Recruitment,
            EnemySettlment
        }

        public static readonly ResourceBehavior<SettlmentViewScreen> Prefab = new ResourceBehavior<SettlmentViewScreen>("Ui/SettlmentViewScreen");

        [Header("Construction Layout")]
        [SerializeField] private BuiltSlotUI builtSlotPrefab = default;
        [SerializeField] private EmptySlotUi emptyConstructionSlotUi = default;
        [SerializeField] private Transform constructionSlotsRoot = default;
        [SerializeField] private GameObject constructionLayout = default;
        [SerializeField] private Button openConstructionLayoutButton = default;
        [Header("Recruitment Layout")]
        [SerializeField] private RecruitUnitButton recruitmentSlotPrefab = default;
        [SerializeField] private Transform recruitmentSlotsRoot = default;
        [SerializeField] private GameObject recruitmentLayout = default;
        [SerializeField] private Button openRecruitmentLayoutButton = default;
        [Header("Enemy settlment")]
        [SerializeField] private Button sendTroopsButton = default;
        [Space]
        [SerializeField] private GameObject armyPanel = default;
        [SerializeField] private TextMeshProUGUI meleeUnitsCount = default;
        [SerializeField] private TextMeshProUGUI rangedUnitsCount = default;

        private List<GameObject> constructionSlots = new List<GameObject>();
        private List<GameObject> recruitmentSlots = new List<GameObject>();

        private Settlment selectedSettlment;
        private Character player;


        private void Awake()
        {
            openConstructionLayoutButton.onClick.AddListener(() => SetMode(Mode.Construction));
            openRecruitmentLayoutButton.onClick.AddListener(() => SetMode(Mode.Recruitment));
        }


        public void Show(Settlment settlment, Character player, Action<object> onHidden = null)
        {
            Show(onHidden);

            selectedSettlment = settlment;
            this.player = player;

            EventsAggregator.Subscribe(MessageType.UpdateUi, HandlePlayerSettlmentsUpdate);
            selectedSettlment.OnUnitsChange += UpdateUnitsCount;

            if (selectedSettlment.Lord == player)
            {
                SetMode(Mode.Construction);
            }
            else
            {
                SetMode(Mode.EnemySettlment);
            }

            UpdateUnitsCount();
            RefreshUi();
        }


        public override void Hide(object result)
        {
            base.Hide(result);

            EventsAggregator.Unsubscribe(MessageType.UpdateUi, HandlePlayerSettlmentsUpdate);
            selectedSettlment.OnUnitsChange -= UpdateUnitsCount;

            selectedSettlment = null;
        }


        private void SetMode(Mode mode)
        {
            constructionLayout.SetActive(false);
            recruitmentLayout.SetActive(false);
            sendTroopsButton.gameObject.SetActive(false);
            openRecruitmentLayoutButton.gameObject.SetActive(false);
            armyPanel.SetActive(false);

            switch (mode)
            {
                case Mode.Construction:
                    constructionLayout.SetActive(true);
                    openRecruitmentLayoutButton.gameObject.SetActive(true);
                    armyPanel.SetActive(true);
                    break;

                case Mode.Recruitment:
                    recruitmentLayout.SetActive(true);
                    openRecruitmentLayoutButton.gameObject.SetActive(true);
                    armyPanel.SetActive(true);
                    break;

                case Mode.EnemySettlment:
                    constructionLayout.SetActive(true);
                    sendTroopsButton.gameObject.SetActive(true);
                    break;
            }
        }


        private void HandlePlayerSettlmentsUpdate(IMessage _) => RefreshUi();


        private void RefreshUi()
        {
            foreach (GameObject slot in constructionSlots)
            {
                Destroy(slot);
            }

            constructionSlots.Clear();

            List<Settlment.Building> buildings = selectedSettlment.GetBuildings();
            bool isPlayerSettlment = (selectedSettlment.Lord == player);

            for (int i = 0; i < SettlmentsSettings.GetMaxBuildingsCount(selectedSettlment.Type); i++)
            {
                if (i < buildings.Count)
                {
                    BuiltSlotUI buildingInfo = Instantiate(builtSlotPrefab, constructionSlotsRoot);
                    buildingInfo.Init(selectedSettlment.Id, selectedSettlment.Type, buildings[i], isPlayerSettlment);
                    constructionSlots.Add(buildingInfo.gameObject);
                }
                else
                {
                    EmptySlotUi emptySlot = Instantiate(emptyConstructionSlotUi, constructionSlotsRoot);
                    constructionSlots.Add(emptySlot.gameObject);
                    emptySlot.Init(() =>
                    {
                        ChooseBuildingScreen.Prefab.Instance.Show(selectedSettlment.Id, (chosenBuilding) =>
                        {
                            if (chosenBuilding != BuildingType.None)
                            {
                                BuildingSystem.Build(selectedSettlment.Id, chosenBuilding);
                            }
                        });
                    }, isPlayerSettlment);
                }
            }

            foreach (GameObject slot in recruitmentSlots)
            {
                Destroy(slot);
            }

            recruitmentSlots.Clear();

            if (selectedSettlment.HasBuilding(BuildingType.Barracks))
            {
                CreateUnitSlot(WeaponType.Melee, selectedSettlment.Lord.MeleeWeaponLevel);
                CreateUnitSlot(WeaponType.Ranged, selectedSettlment.Lord.RangedWeaponLevel);
            }
        }


        private void CreateUnitSlot(WeaponType type, int level)
        {
            RecruitUnitButton recruitUnitButton = Instantiate(recruitmentSlotPrefab, recruitmentSlotsRoot);
            recruitUnitButton.Init(() => RecruitmentSystem.Recruit(selectedSettlment.Id, type, level), RecruitmentSystem.CanRecruit(selectedSettlment.Id), type, level);
            recruitmentSlots.Add(recruitUnitButton.gameObject);
        }


        private void UpdateUnitsCount()
        {
            meleeUnitsCount.text = selectedSettlment.GetUnitsCount(WeaponType.Melee).ToString();
            rangedUnitsCount.text = selectedSettlment.GetUnitsCount(WeaponType.Ranged).ToString();
        }
    }
}
