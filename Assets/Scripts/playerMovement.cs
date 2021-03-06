﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Camera cam;
    public Animator animator;
    public Player playerScript;

    public GameObject crosshair;

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

    public GameObject bullet;
    public GameObject bulletPoint;

    //these need to be retrieved from the player script at some point
    public float bulletSpeed;
    public float firingSpeed;

    private float fireCounter = 0;

    void Start() {
        playerScript = GetComponent<Player>();
    }

    void Update()
    {
        //player control variables
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //checks if player is the air
        isGrounded = Physics.CheckBox(groundCheck.position, new Vector3(5, groundDistance, 5), Quaternion.identity, groundMask);

        //sets animator attributes
        animator.SetFloat("Speed", GetComponent<CharacterController>().velocity.magnitude);

        animator.SetFloat("DirectionX", direction.x);

        if (Input.GetButton("Fire1")){

            animator.SetBool("Shooting", true);

            if(direction.x != 0 && direction.z == 0)
            {
                animator.SetBool("Strafing", true);
            }
            else
            {
                animator.SetBool("Strafing", false);
            }
        }
        else
        {
            animator.SetBool("Shooting", false);
            animator.SetBool("Strafing", false);
        }

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }

        //movement

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;

            if (animator.GetBool("Shooting") == false)
            {
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }

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

        //Shooting mechanics

        if (animator.GetBool("Shooting"))
        {
            playerScript.shooting = true;

            //player turns towards camera while shooting
            float targetAngle = Mathf.Atan2(0, 1) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            Debug.Log(direction.x + " " + direction.z);
            //set rotation to angle for smoothing effect
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
        else
        {
          playerScript.shooting = false;
        }

        //if raycast doesnt hit, get a point along the ray

        if(fireCounter < 0)
        {
            Vector3 crosshairPoint = new Vector3(0, 0, 0);
            Vector3 bulletDirection = new Vector3(0, 0, 0);

            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(crosshair.transform.position);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~9))
            {
                crosshairPoint = hit.point;
                bulletDirection = crosshairPoint - bulletPoint.transform.position;
            }
            else
            {
                crosshairPoint = ray.GetPoint(1000);
                bulletDirection = crosshairPoint - bulletPoint.transform.position;
            }

            GameObject thisBullet = Instantiate(bullet);
            thisBullet.transform.position = bulletPoint.transform.position;
            thisBullet.transform.rotation = bulletPoint.transform.rotation;
            thisBullet.GetComponent<Rigidbody>().velocity = bulletDirection.normalized * bulletSpeed;

            fireCounter = firingSpeed;
        }

        //resets firing interval counter after shooting
        if(animator.GetBool("Shooting") == false)
        {
            fireCounter = 0;
        }
    }
}
