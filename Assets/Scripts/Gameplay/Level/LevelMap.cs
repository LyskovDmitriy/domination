using Domination.Data;
using Domination.LevelLogic;
using System.Linq;
using System.Collections.Generic;


namespace Domination.Generator
{
    public class LevelMap
    {
        public Tile[,] map;
        public TileType[,] simpleMap;
        public Castle[] castles;
        public Village[] villages;


        public LevelMap() { }

        public LevelMap(MapData data)
        {
            simpleMap = data.tiles;
            map = new Tile[simpleMap.GetLength(0), simpleMap.GetLength(1)];

            for (int x = 0; x < simpleMap.GetLength(0); x++)
            {
                for (int y = 0; y < simpleMap.GetLength(1); y++)
                {
                    map[x, y] = new Tile(simpleMap[x, y]);
                }
            }

            var castlesList = new List<Castle>();
            var villagesList = new List<Village>();

            foreach (var settlmentData in data.settlments)
            {
                switch (settlmentData.type)
                {
                    case SettlmentType.Castle:
                        castlesList.Add(new Castle(settlmentData));
                        break;

                    case SettlmentType.Village:
                        villagesList.Add(new Village(settlmentData));
                        break;
                }
            }

            castles = castlesList.ToArray();
            villages = villagesList.ToArray();
        }


        public MapData GetData() => new MapData
        {
            tiles = simpleMap,
            settlments = castles.Select(c => c.GetData())
            .Concat(
            villages.Select(v => v.GetData()))
            .ToArray()
        };
    }
}
