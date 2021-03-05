using Domination.Data;
using Domination.Generator;
using Domination.LevelLogic;
using Domination.Utils;
using Utils;
using System;
using System.Linq;


namespace Domination
{
    public class Level
    {
        public event Action OnTurnFinished;

        private LevelMap levelMap;

        private int activeCharacterIndex;

        private Character neutralCharacter; //To place garrisons in neutral villages


        public Character Player => Characters[0];
        public Character[] Characters { get; private set; } = new Character[2];

        public Tile[,] Map => levelMap.map;

        public Castle[] Castles => levelMap.castles;
        public Village[] Villages => levelMap.villages;

        public int CurrentTurn { get; private set; }
        public Character ActiveCharacter => Characters[activeCharacterIndex];


        public Level()
        {
            levelMap = LevelGenerator.Generate();

            Characters[0] = new Player();
            Characters[0].AddSettlment(levelMap.castles[0]);

            Characters[1] = new AiCharacter();
            Characters[1].AddSettlment(levelMap.castles[1]);

            LevelWrapper wrapper = new LevelWrapper(this);

            BuildingSystem.Init(wrapper);
            RecruitmentSystem.Init(wrapper);

            neutralCharacter = new Character();

            foreach (var village in levelMap.villages)
            {
                neutralCharacter.AddSettlment(village);
                RecruitmentSystem.SetupNeutralVillageArmy(village);
            }

            Characters[0].OnTurnFinish += OnCharacterTurnFinish;
            Characters[0].StartTurn(true);
        }


        public LevelData GetData() => new LevelData
        {
            activeCharacterIndex = activeCharacterIndex,
            currentTurn = CurrentTurn,

            actingCharacters = Characters.Select(c => c.GetData()).ToArray(),
            neutralCharacter = neutralCharacter.GetData(),

            mapData = levelMap.GetData()
        };

        public float CalculateDistanceBetweenSettlments(Settlment startingSettlment, Settlment targetSettlment) => 
            Pathfinding.GetDistance(startingSettlment.Position, targetSettlment.Position, levelMap.simpleMap, TilesPassingCostContainer.GetTilePassingCost);

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
    }
}
