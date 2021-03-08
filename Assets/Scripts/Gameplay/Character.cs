using System;
using System.Linq;
using System.Collections.Generic;
using Domination.Warfare;
using Domination.EventsSystem;
using Domination.LevelLogic;
using Domination.Data;
using UnityEngine;


namespace Domination
{
    public class Character
    {
        public event Action OnTurnFinish;

        private const int DefaultCoinsCount = 50;
        private static uint NextId = 1;

        private int coins;

        private Dictionary<Settlment, Army> stationedArmies = new Dictionary<Settlment, Army>();
        private List<MarchingUnit> marchingUnits = new List<MarchingUnit>();


        public uint Id { get; private set; }

        public int MeleeWeaponLevel { get; private set; } = 0;
        public int RangedWeaponLevel { get; private set; } = 0;

        public List<Settlment> OwnedSettlments { get; private set; } = new List<Settlment>();
        public Settlment Castle => OwnedSettlments.Find(s => s.Type == SettlmentType.Castle);
        
        public int Income => OwnedSettlments.Sum((settlment) => settlment.Income);


        public int Coins
        {
            get => coins;
            set
            {
                if (coins != value)
                {
                    SetNewCoinsCount(value);
                }
            }
        }


        public Character()
        {
            Id = NextId;
            NextId++;
            coins = DefaultCoinsCount;
        }

        public Character(Func<uint, Settlment> settlmentGetter, CharacterData data)
        {
            Id = data.id;
            Coins = data.coinsCount;

            MeleeWeaponLevel = data.meleeWeaponLevel;
            RangedWeaponLevel = data.rangedWeaponLevel;

            foreach (var settlmentId in data.ownedSettlments)
            {
                AddSettlment(settlmentGetter(settlmentId));
            }

            foreach (var armyData in data.stationedArmies)
            {
                stationedArmies.Add(settlmentGetter(armyData.settlmentId), new Army(armyData.units));
            }

            foreach (var marchingUnitData in data.marchingUnits)
            {
                AddMarchingUnit(new Unit(
                    marchingUnitData.unitData),
                    settlmentGetter(marchingUnitData.targetSettlment), 
                    marchingUnitData.daysLeft);
            }
        }


        public CharacterData GetData() => new CharacterData
        {
            id = Id,
            isPlayer = this is Player,
            coinsCount = Coins,

            meleeWeaponLevel = MeleeWeaponLevel,
            rangedWeaponLevel = RangedWeaponLevel,

            ownedSettlments = OwnedSettlments.Select(s => s.Id).ToArray(),

            stationedArmies = stationedArmies.Select(pair => new StationedArmyData
            {
                settlmentId = pair.Key.Id,
                units = pair.Value.GetData()
            }).ToArray(),
            marchingUnits = marchingUnits.Select(unit => unit.GetData()).ToArray()
        };

        public virtual void StartTurn(bool isFirstTurn)
        {
            if (!isFirstTurn)
            {
                Coins += Income;
            }

            //Update marching units
            for (int i = marchingUnits.Count - 1; i >= 0; i--)
            {
                marchingUnits[i].daysLeft--;
                if (marchingUnits[i].daysLeft == 0)
                {
                    var army = GetSettlmentArmy(marchingUnits[i].targetSettlment);
                    army.AddUnit(marchingUnits[i].unit);
                    marchingUnits.RemoveAt(i);
                }
            }
        }

        public void AddMarchingUnit(Unit unit, Settlment targetSettlment, int marchDuration)
        {
            marchingUnits.Add(new MarchingUnit
            {
                unit = unit,
                targetSettlment = targetSettlment,
                daysLeft = marchDuration
            });
        }

        public List<MarchingUnit> GetMarchingUnits(Settlment settlment) => 
            marchingUnits.FindAll((unit) => unit.targetSettlment == settlment);

        public virtual void DestroyBuilding(uint settlmentId, BuildingType buildingType) => 
            GetSettlmentById(settlmentId).DestroyBuilding(buildingType);

        public virtual void UpgradeBuilding(uint settlmentId, BuildingType buildingType)
        {
            if (TryRemoveCoins(BuildingSystem.GetConstructionPrice(buildingType), "Character doesn't have enough money to build the building"))
            {
                Settlment settlment = GetSettlmentById(settlmentId);
                settlment.UpgradeBuilding(buildingType);
            }
        }

        public virtual void Build(uint settlmentId, BuildingType buildingType)
        {
            if (TryRemoveCoins(BuildingSystem.GetConstructionPrice(buildingType), "Character doesn't have enough money to build the building"))
            {
                Settlment settlment = GetSettlmentById(settlmentId);
                settlment.Build(buildingType);
            }
        }

        public void Recruit(Unit unit, uint settlmentId)
        {
            if (TryRemoveCoins(RecruitmentSystem.UnitPrice, "Character doesn't have enough money to recruit a unit"))
            {
                Settlment settlment = GetSettlmentById(settlmentId);
                GetSettlmentArmy(settlment).AddUnit(unit);
                EventsAggregator.TriggerEvent(new UnitRecruitedMessage(Id, settlmentId));
            }
        }

        public Settlment GetSettlmentById(uint settlmentId) => 
            OwnedSettlments.Find((settlment) => settlment.Id == settlmentId);

        public bool HasCoins(int recuiredAmount) => Coins >= recuiredAmount;

        public Army GetSettlmentArmy(Settlment settlment)
        {
            if (!stationedArmies.ContainsKey(settlment))
            {
                stationedArmies.Add(settlment, new Army());
            }
            return stationedArmies[settlment];
        }

        public bool HasUnitsInSettlment(Settlment settlment)
        {
            var army = GetSettlmentArmy(settlment);

            if (army != null)
            {
                return !army.IsEmpty;
            }

            return false;
        }

        public void AddSettlment(Settlment settlment)
        {
            OwnedSettlments.Add(settlment);
            settlment.Lord = this;
        }

        protected void FinishTurn() => OnTurnFinish?.Invoke();

        protected virtual void SetNewCoinsCount(int coins) => this.coins = coins;

        private bool TryRemoveCoins(int coinsCount, string failureMessage)
        {
            if (!HasCoins(coinsCount))
            {
                Debug.LogError(failureMessage);
                return false;
            }
            Coins -= RecruitmentSystem.UnitPrice;
            return true;
        }
    }
}
