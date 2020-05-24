using System;
using System.Linq;
using System.Collections.Generic;
using Domination.Warfare;

namespace Domination
{
    public class Character
    {
        public event Action OnTurnFinish;

        private const int DefaultCoinsCount = 50;
        private static int NextId = 0;

        protected List<Settlment> settlments = new List<Settlment>();
        private int coins = DefaultCoinsCount;


        public static int PlayerId { get; protected set; }
        public int Id { get; private set; }

        public int MeleeWeaponLevel { get; private set; } = 0;
        public int RangedWeaponLevel { get; private set; } = 0;

        public int Income => settlments.Sum((settlment) => settlment.Income);

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


        public Character(Castle castle)
        {
            Id = NextId;
            NextId++;

            settlments.Add(castle);
        }


        public virtual void StartTurn(bool isFirstTurn)
        {
            if (!isFirstTurn)
            {
                Coins += Income;
            }
        }


        public bool HasSettlment(int settlmentId) => GetSettlmentById(settlmentId) != null;


        protected void FinishTurn()
        {
            OnTurnFinish?.Invoke();
        }


        protected virtual void SetNewCoinsCount(int coins)
        {
            this.coins = coins;
        }


        public virtual void DestroyBuilding(int settlmentId, BuildingType buildingType)
        {
            GetSettlmentById(settlmentId).DestroyBuilding(buildingType);
        }


        public virtual void UpgradeBuilding(int settlmentId, BuildingType buildingType)
        {
            Settlment settlment = GetSettlmentById(settlmentId);
            Coins -= BuildingSystem.GetUpgradePrice(buildingType, settlment.GetBuilding(buildingType).level);
            settlment.UpgradeBuilding(buildingType);
        }


        public virtual void Build(int settlmentId, BuildingType buildingType)
        {
            Settlment settlment = GetSettlmentById(settlmentId);
            Coins -= BuildingSystem.GetConstructionPrice(buildingType);
            settlment.Build(buildingType);
        }


        public void Recruit(Unit unit, int settlmentId)
        {
            Settlment settlment = GetSettlmentById(settlmentId);
            Coins -= RecruitmentSystem.UnitPrice;
            settlment.Recruit(unit);
        }


        public Settlment GetSettlmentById(int settlmentId) => settlments.Find((settlment) => settlment.Id == settlmentId);


        public bool HasCoins(int recuiredAmount) => Coins >= recuiredAmount;
    }
}
