using Domination.LevelLogic;
using UnityEngine;


namespace Domination.LevelView
{
    public class SettlmentView : MonoBehaviour
    {
        public Settlment Settlment { get; private set; }


        public void Init(Settlment settlment)
        {
            Settlment = settlment;
        }
    }
}
