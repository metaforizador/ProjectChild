using UnityEngine;
using ProjectChild.Inputs;

namespace ProjectChild.Movement
{
    public class IshinMovement : CharacterMovement
    {
        [SerializeField] private float speedBase = 1f;
        [SerializeField] private float speedMultiplier = 1f;
        public float Speed
        {
            get { return speedBase * speedMultiplier; }
        }


        [SerializeField] private float speedTurn = 1f;

        public override void Move(MovementInput input)
        {
            if (input.direction == Vector3.zero)
            {
                animator.SetBool("Moving", false);
                return;
            }

            // update animator state 
            animator.SetBool("Moving", true);

            var forward = Vector3.RotateTowards(transform.forward, input.direction, speedTurn * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(forward);

            transform.position += transform.forward * Speed * Time.deltaTime;
        }

        public override void Attack(AttackInput input)
        {
            animator.SetBool("Shooting", input.melee);
        }

    }
}
