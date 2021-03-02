﻿using UnityEngine;


namespace Domination.LevelLogic
{
    public class Village : Settlment
    {
        public override SettlmentType Type => SettlmentType.Village;


        public Village(Vector2Int position) : base(position) { }
    }
}
