using UnityEngine;

namespace ProjectChild.Movement
{
    public class PlayerMovement : CharacterMovement
    {
        [SerializeField] private float movementThreshold = .1f;

        [Header("Ground Check")]
        [SerializeField] private float groundDistance = .4f;
        [SerializeField] LayerMask groundMask;

        public float speed = Stat.BASE_MOVEMENT_SPEED;
        public float gravity = -9.81f;
        public float jumpHeight = 10f;
        public float groundedOffset = .1f;

        private void Update()
        {
            // TODO: Handler input via an input manager
            var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            Move(input);
           
            animator.SetFloat("Speed", characterController.velocity.magnitude);
        }

        public override void Move(Vector3 direction)
        {
            // get movement inputs
            var movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            var jump = Input.GetButtonDown("Jump");
            // var grounded = Physics.CheckBox(Vector3.zero, new Vector3(3, groundDistance, 3), Quaternion.identity, groundMask);
            var grounded = Grounded();
            var groundedAnimator = animator.GetBool("isGrounded");

            Vector3 velocity = new Vector3();
            float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Vector3 xzMovement = moveDir.normalized * speed * Time.deltaTime;


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


            animator.SetFloat("DirectionX", movement.x);
            if (direction.magnitude >= movementThreshold)
            {
                characterController.Move(velocity * Time.deltaTime + xzMovement);
            }
        }

        public override void Fire(Vector3 direction)
        {
            
        }

        public override bool Grounded()
        {
            Debug.DrawRay(transform.position, Vector3.down * ((characterController.height / 2f) + groundedOffset));
            return Physics.Raycast(transform.position, Vector3.down, (characterController.height / 2f) + groundedOffset);
        }
    }
}
