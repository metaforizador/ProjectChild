using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Animator animator;

    //only used for test purposes
    public GameObject masterCanvas;
    private bool menu = false;

    public float speed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 10f;

    private Vector3 xzMovement;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    void Update()
    {
        //checks if player is the air
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //sets animator attributes
        animator.SetFloat("Speed", GetComponent<CharacterController>().velocity.magnitude);

        if (Input.GetButton("Fire1")){

            animator.SetBool("Shooting", true);
        }
        else
        {
            animator.SetBool("Shooting", false);
        }

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            xzMovement = moveDir.normalized * speed * Time.deltaTime;
        }

        if (isGrounded)
        {
            animator.SetBool("Jumping", false);

            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                animator.SetTrigger("Jump");
                animator.SetBool("Jumping", true);
            }
        }
        
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime + xzMovement);
        xzMovement = new Vector3(0, 0, 0);

        //toggle button for dialogue system (demo only)
        if (Input.GetButtonDown("test"))
        {
            menu = !menu;

            Debug.Log(menu);

            masterCanvas.SetActive(menu);
        }
    }
}
