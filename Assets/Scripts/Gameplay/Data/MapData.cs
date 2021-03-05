using System;


namespace Domination.Data
{
    [Serializable]
    public class MapData
    {
        public TileType[,] tiles;
        public SettlmentData[] settlments;
    }
}
