﻿using System;
using System.Linq;
using System.Collections.Generic;
using Domination.Warfare;
using Domination.EventsSystem;


namespace Domination
{
    public class Character
    {
        public event Action OnTurnFinish;

        private const int DefaultCoinsCount = 50;
        private static uint NextId = 1;

        private int coins = DefaultCoinsCount;

        private Dictionary<Settlment, Army> stationedArmies = new Dictionary<Settlment, Army>();
        private List<MarchingUnit> marchingUnits = new List<MarchingUnit>();


        public static uint PlayerId { get; protected set; }
        public uint Id { get; private set; }

        public int MeleeWeaponLevel { get; private set; } = 0;
        public int RangedWeaponLevel { get; private set; } = 0;

        public int Income => Settlments.Sum((settlment) => settlment.Income);

        public List<Settlment> Settlments { get; private set; } = new List<Settlment>();


        public Settlment Castle => Settlments[0];

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
        }


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
                    if (!stationedArmies.TryGetValue(marchingUnits[i].targetSettlment, out var army))
                    {
                        army = new Army();
                        stationedArmies.Add(marchingUnits[i].targetSettlment, army);
                    }

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


        public List<MarchingUnit> GetMarchingUnits(Settlment settlment) => marchingUnits.FindAll((unit) => unit.targetSettlment == settlment);


        public bool HasSettlment(uint settlmentId) => GetSettlmentById(settlmentId) != null;


        public virtual void DestroyBuilding(uint settlmentId, BuildingType buildingType)
        {
            GetSettlmentById(settlmentId).DestroyBuilding(buildingType);
        }


        public virtual void UpgradeBuilding(uint settlmentId, BuildingType buildingType)
        {
            Settlment settlment = GetSettlmentById(settlmentId);
            Coins -= BuildingSystem.GetUpgradePrice(buildingType, settlment.GetBuilding(buildingType).level);
            settlment.UpgradeBuilding(buildingType);
        }


        public virtual void Build(uint settlmentId, BuildingType buildingType)
        {
            Settlment settlment = GetSettlmentById(settlmentId);
            Coins -= BuildingSystem.GetConstructionPrice(buildingType);
            settlment.Build(buildingType);
        }


        public void Recruit(Unit unit, uint settlmentId)
        {
            Settlment settlment = GetSettlmentById(settlmentId);
            Coins -= RecruitmentSystem.UnitPrice;
            GetSettlmentArmy(settlment).AddUnit(unit);
            EventsAggregator.TriggerEvent(new UnitRecruitedMessage(Id, settlmentId));
        }


        public Settlment GetSettlmentById(uint settlmentId) => Settlments.Find((settlment) => settlment.Id == settlmentId);


        public bool HasCoins(int recuiredAmount) => Coins >= recuiredAmount;


        public Army GetSettlmentArmy(Settlment settlment)
        {
            stationedArmies.TryGetValue(settlment, out var army);
            return army;
        }

        public void AddSettlment(Settlment settlment)
        {
            Settlments.Add(settlment);
            stationedArmies.Add(settlment, new Army());
            settlment.Lord = this;
        }


        protected void FinishTurn() => OnTurnFinish?.Invoke();


        protected virtual void SetNewCoinsCount(int coins) => this.coins = coins;
    }
}
