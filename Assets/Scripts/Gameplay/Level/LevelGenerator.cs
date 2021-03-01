using Domination.LevelLogic;
using UnityEngine;


namespace Domination.Generator
{
    public static class LevelGenerator
    {
        public static LevelMap Generate()
        {
            LevelMap levelMap = new LevelMap();

            var resolution = MapGenerationSettings.MapResolution;
            var tiles = WaveFunctionCollapse.GenerateMap(resolution, MapGenerationSettings.TileTexture);
            levelMap.simpleMap = tiles;
            levelMap.map = new Tile[resolution.x, resolution.y];

            for (int x = 0; x < resolution.x; x++)
            {
                for (int y = 0; y < resolution.y; y++)
                {
                    levelMap.map[x, y] = new Tile(tiles[x, y]);
                }
            }

            var settlments = SettlmentsGenerator.Generate(tiles);
            Vector2Int[] castles = settlments.castles;
            levelMap.castles = new Castle[settlments.castles.Length];

            for (int i = 0; i < castles.Length; i++)
            {
                levelMap.castles[i] = new Castle(castles[i]);
            }

            Vector2Int[] villages = settlments.villages;
            levelMap.villages = new Village[settlments.villages.Length];

            for (int i = 0; i < villages.Length; i++)
            {
                levelMap.villages[i] = new Village(villages[i]);
            }

            return levelMap;
        }
    }
}
