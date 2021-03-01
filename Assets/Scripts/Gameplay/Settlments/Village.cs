using UnityEngine;


namespace Domination
{
    public class Village : Settlment
    {
        public override SettlmentType Type => SettlmentType.Village;


        public Village(Vector2Int position) : base(position) { }
    }
}
