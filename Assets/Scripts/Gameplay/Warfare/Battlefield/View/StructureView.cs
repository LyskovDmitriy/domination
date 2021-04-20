using Domination.Battle.Logic;
using UnityEngine;


namespace Domination.Battle.View
{
    public class StructureView : MonoBehaviour
    {
        public void Init(Structure structure)
        {
            structure.OnDestroyed += _ => Destroy(gameObject);
        }
    }
}
