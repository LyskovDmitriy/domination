using Domination.Battle.Logic;
using Domination.Battle.Logic.Ai;
using System;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;


namespace Domination.Battle.View
{
    public class WarriorView : MonoBehaviour
    {
        public event Action<WarriorView> OnDied;

        [SerializeField] private new SpriteRenderer renderer = default;
        [Header("Animation")]
        [SerializeField] private Animator animator = default;
        [SerializeField] private float moveSpeed = default;
        [SerializeField] private float attackAnimationSecondsDuration = default;
        [SerializeField] private float deathAnimationDuration = default;

        private Warrior warrior;


        public void Init(Warrior warrior, bool isFacingLeft)
        {
            this.warrior = warrior;
            renderer.flipX = isFacingLeft;
            warrior.OnDied += _ => DestroyView();
        }

        public async Task ExecutePlan(Func<Vector2Int, Vector2> getTilePosition)
        {
            var action = warrior.planner.PlannedAction;

            switch (action)
            {
                case MoveAction moveAction:
                    //Add randomized delay
                    animator.SetBool("IsWalking", true);
                    //Move
                    Vector2 targetPosition = getTilePosition(moveAction.TargetPosition);
                    var movementDuration = (transform.position.ToVector2() - targetPosition).magnitude / moveSpeed;
                    await transform.DOMove(targetPosition, movementDuration).AsyncWaitForCompletion();
                    //Randomize speed
                    animator.SetBool("IsWalking", false);
                    break;

                case AttackAction _:
                    //Add randomized delay
                    animator.SetTrigger("Attack");
                    await Task.Delay(Mathf.RoundToInt(attackAnimationSecondsDuration * 1000));
                    break;

                case IdleAction _:
                    break;
            }
        }

        private async void DestroyView()
        {
            OnDied?.Invoke(this);

            animator.SetTrigger("Died");

            await Task.Delay(Mathf.RoundToInt(deathAnimationDuration * 1000));

            Destroy(gameObject);
        }
    }
}
