using Domination.Battle.Logic;
using System.Threading.Tasks;
using UnityEngine;


namespace Domination.Battle.View
{
    public class WarriorView : MonoBehaviour
    {
        [SerializeField] private new SpriteRenderer renderer = default;

        private Warrior warrior;


        public void Init(Warrior warrior, bool isFacingLeft)
        {
            this.warrior = warrior;

            renderer.flipX = isFacingLeft;
        }

        public async Task ExecutePlan()
        {
            
        }
    }
}
