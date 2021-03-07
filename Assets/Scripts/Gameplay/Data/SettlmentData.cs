using System;
using UnityEngine;


namespace Domination.Data
{
    [Serializable]
    public class SettlmentData
    {
        public uint id;
        public SettlmentType type;

        public Vector2Int position;

        public BuildingData[] buildings;
    }
}
