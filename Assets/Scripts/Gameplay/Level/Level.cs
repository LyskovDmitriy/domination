using Domination.Data;
using Domination.Generator;
using Domination.LevelLogic;
using Utils;
using System;
using System.Linq;
using Domination.EventsSystem;


namespace Domination
{
    public class Level
    {
        public event Action OnTurnFinished;

        private LevelMap levelMap;

        private int activeCharacterIndex;

        private Character neutralCharacter; //To place garrisons in neutral villages

        private EventsAggregator LocalAggregator;


        public Character[] Characters { get; private set; }
        public Character Player => Characters[0];
        public Character ActiveCharacter => Characters[activeCharacterIndex];

        public Tile[,] Map => levelMap.map;

        public Castle[] Castles => levelMap.castles;
        public Village[] Villages => levelMap.villages;

        public int CurrentTurn { get; private set; }

        public RecruitmentSystem RecruitmentSystem { get; private set; }
        public BuildingSystem BuildingSystem { get; private set; }


        public Level(EventsAggregator aggregator)
        {
            LocalAggregator = new EventsAggregator(aggregator);
            levelMap = LevelGenerator.Generate();

            Characters = new Character[2];

            Characters[0] = new Player(LocalAggregator);
            Characters[0].AddSettlment(levelMap.castles[0]);

            Characters[1] = new AiCharacter(LocalAggregator);
            Characters[1].AddSettlment(levelMap.castles[1]);

            BuildingSystem = new BuildingSystem(GetSettlment);
            RecruitmentSystem = new RecruitmentSystem(GetSettlment);

            neutralCharacter = new Character(LocalAggregator);
            neutralCharacter.Coins = int.MaxValue;

            foreach (var village in levelMap.villages)
            {
                neutralCharacter.AddSettlment(village);
                RecruitmentSystem.SetupNeutralVillageArmy(village);
            }

            ActiveCharacter.OnTurnFinish += OnCharacterTurnFinish;
            ActiveCharacter.StartTurn(true);
        }

        public Level(EventsAggregator aggregator, LevelData data)
        {
            LocalAggregator = new EventsAggregator(aggregator);
            levelMap = new LevelMap(data.mapData);

            activeCharacterIndex = data.activeCharacterIndex;
            CurrentTurn = data.currentTurn;

            Characters = new Character[data.actingCharacters.Length];

            for (int i = 0; i < data.actingCharacters.Length; i++)
            {
                var characterData = data.actingCharacters[i];

                if (characterData.isPlayer)
                {
                    Characters[i] = new Player(LocalAggregator, GetSettlment, characterData);
                }
                else
                {
                    Characters[i] = new AiCharacter(LocalAggregator, GetSettlment, characterData);
                }
            }

            neutralCharacter = new Character(LocalAggregator, GetSettlment, data.neutralCharacter);

            BuildingSystem = new BuildingSystem(GetSettlment);
            RecruitmentSystem = new RecruitmentSystem(GetSettlment);

            ActiveCharacter.OnTurnFinish += OnCharacterTurnFinish;
        }


        public LevelData GetData() => new LevelData
        {
            activeCharacterIndex = activeCharacterIndex,
            currentTurn = CurrentTurn,

            actingCharacters = Characters.Select(c => c.GetData()).ToArray(),
            neutralCharacter = neutralCharacter.GetData(),

            mapData = levelMap.GetData()
        };

        public void ShutDown() => LocalAggregator.ShutDown();

        public float CalculateDistanceBetweenSettlments(Settlment startingSettlment, Settlment targetSettlment) => 
            AStarPathfinding.GetDistance(
                startingSettlment.Position, 
                targetSettlment.Position, 
                levelMap.simpleMap, 
                TilesPassingCostContainer.GetTilePassingCost,
                GameConstants.GLOBAL_MAP_DISTANCE_HEURISTICS_MODIFIER);

        public Settlment GetSettlment(uint settlmentId) => Array.Find(GetSettlments(), s => s.Id == settlmentId);

        private void OnCharacterTurnFinish()
        {
            ActiveCharacter.OnTurnFinish -= OnCharacterTurnFinish;
            activeCharacterIndex++;

            if (activeCharacterIndex >= Characters.Length)
            {
                activeCharacterIndex %= Characters.Length;
            }

            if (activeCharacterIndex == 0)
            {
                CurrentTurn++;
            }

            ActiveCharacter.OnTurnFinish += OnCharacterTurnFinish;
            ActiveCharacter.StartTurn(CurrentTurn == 0);

            OnTurnFinished?.Invoke();
        }

        private Settlment[] GetSettlments() =>
            Castles.Cast<Settlment>().Concat(Villages.Cast<Settlment>()).ToArray();
    }
}
