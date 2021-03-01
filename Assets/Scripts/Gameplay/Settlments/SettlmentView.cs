using UnityEngine;


namespace Domination
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
