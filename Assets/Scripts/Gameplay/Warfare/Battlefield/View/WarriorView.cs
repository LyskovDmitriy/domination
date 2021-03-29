using UnityEngine;


namespace Domination.Battle.View
{
    public class WarriorView : MonoBehaviour
    {
        [SerializeField] private new SpriteRenderer renderer = default;

        public void Init(bool isFacingLeft)
        {
            if (isFacingLeft)
            {
                renderer.transform.localEulerAngles = Vector3.up * 180;
            }
        }
    }
}
