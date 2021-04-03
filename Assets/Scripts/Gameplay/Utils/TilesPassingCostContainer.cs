using System;
using UnityEngine;


namespace Utils
{
    [CreateAssetMenu]
    public class TilesPassingCostContainer : ScriptableObject
    {
        [Serializable]
        private class TilePassingCost
        {
            public TileType tile;
            public float cost;
        }


        private static readonly ResourceAsset<TilesPassingCostContainer> asset = new ResourceAsset<TilesPassingCostContainer>("TilesPassingCostContainer");

        [SerializeField] private TilePassingCost[] tilesPassingCosts = default;


        public static float GetTilePassingCost(TileType tile) => Array.Find(asset.Value.tilesPassingCosts, (info) => info.tile == tile).cost;
    }
}
