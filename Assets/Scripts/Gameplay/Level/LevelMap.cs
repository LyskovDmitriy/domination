using Domination.Data;
using Domination.LevelLogic;
using System.Linq;


namespace Domination.Generator
{
    public class LevelMap
    {
        public Tile[,] map;
        public TileType[,] simpleMap;
        public Castle[] castles;
        public Village[] villages;


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
