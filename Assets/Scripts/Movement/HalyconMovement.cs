using UnityEngine;
using ProjectChild.Inputs;

namespace ProjectChild.Movement
{
    public class HalyconMovement : CharacterMovement
    {
        [SerializeField] private float movementThreshold = .1f;

        [Header("Ground Check")]
        [SerializeField] private float groundDistance = .4f;
        [SerializeField] LayerMask groundMask;

        public float speedBase = Stat.BASE_MOVEMENT_SPEED;
        public float speedMultiplierDash = 200f;
        public float speedMultiplierDashExit = -100f;
        public float gravity = -9.81f;
        public float jumpHeight = 10f;
        public float groundedOffset = .1f;

        private float angularVelocity;
        private float angularVelocitySmoothTime = .1f;

        private void Update()
        {
            // TODO: Handle input via an input manager
            var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            Move(new MovementInput());
           
            animator.SetFloat("Speed", characterController.velocity.magnitude);
        }

        public override void Move(MovementInput input)
        {
            // get movement inputs
            var direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            var movement = direction.normalized;
            var jump = Input.GetButtonDown("Jump");
            input.dash = Input.GetButtonDown("Dash");

            var grounded = Grounded();
            var groundedAnimator = animator.GetBool("isGrounded");

            Vector3 velocity = new Vector3();
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref angularVelocity, angularVelocitySmoothTime);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Vector3 xzMovement = moveDir.normalized * speedBase * Time.deltaTime;


            if(grounded)
            {
                animator.SetBool("isGrounded", true);

                if (velocity.y < 0f) velocity.y = 0;
                else if(jump)
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                    animator.SetTrigger("Jump");
                    animator.SetBool("isGrounded", false);
                }
            }
            else if (grounded && !groundedAnimator)
            {
                animator.SetBool("isGrounded", false);
                animator.SetTrigger("Jump");
            }

            velocity.y += gravity * Time.deltaTime;

            // handle dashing inputs
            if (input.dash)
            {
                animator.SetTrigger("Dash");
            }
            else if (input.dashing)
            {
                xzMovement *= speedMultiplierDash;
            }
            else if(input.exitingDash)
            {
                xzMovement *= speedMultiplierDashExit;
            }

            // Debug.Log($"{velocity}");
            animator.SetFloat("DirectionX", movement.x);
            if (direction.magnitude >= movementThreshold)
            {
                characterController.Move(velocity * Time.deltaTime + xzMovement);
            }
            else
            {
                characterController.Move(velocity);
            }

            transform.rotation = Quaternion.Euler(0, angle, 0);
        }

        public override bool Grounded()
        {
            Debug.DrawRay(transform.position, Vector3.down * ((characterController.height / 2f) + groundedOffset), Color.red);
            return Physics.Raycast(transform.position, Vector3.down, (characterController.height / 2f) + groundedOffset);
        }
    }
}
