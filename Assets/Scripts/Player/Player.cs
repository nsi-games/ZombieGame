using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum MovementState
    {
        NULL = 0,
        IDLE = 1,
        CROUCHING = 2,
        WALKING = 3,
        RUNNING = 4,
        SPRINTING = 5
    }

    public PlayerCamera playerCamera;
    public float runSpeed = 7.5f;
    public float walkSpeed = 6f;
    public float gravity = 10f;
    public float crouchSpeed = 4f;
    public float jumpHeight = 20f;
    public bool isRunning = false;
    public bool isCrouching = false;
    public bool isJumping = false;

    private CharacterController controller;
    public Vector3 movement;

    public MovementState movementState;

    public PlayerIKHandling IK;

    private Animator anim;
    private Vector3 prevPosition;

    void Awake()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        prevPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    { 
        CharacterMovement();
        CameraMovement();
        Animation();
    }
    
    public void Move(Vector3 input, bool isCrouching, bool isRunning, bool isJumping)
    {
        this.isCrouching = isCrouching;
        this.isRunning = isRunning;
        this.isJumping = isJumping;
        if (input.magnitude != 0)
        {
            movementState = MovementState.WALKING;
        }
        else
        {
            movementState = MovementState.IDLE;
        }
        if (this.isRunning)
        {
            movementState = MovementState.SPRINTING;
        }
        if(this.isCrouching)
        {
            movementState = MovementState.CROUCHING;
        }
        float currentSpeed = isCrouching ? crouchSpeed : (isRunning ? runSpeed : walkSpeed);
        movement = input * currentSpeed * Time.deltaTime;
    }

    void CharacterMovement()
    {
        if (controller.isGrounded)
        {
            if (isJumping)
            {
                movement.y = jumpHeight;
            }
        }

        movement.y -= gravity * Time.deltaTime;
        if (movement.y < -gravity) movement.y = -gravity;

        controller.Move(movement);
    }

    void CameraMovement()
    {
        // If there is a playerCamera AND
        // playerCamera is not frozen AND
        // playerCamera is not free
        if (playerCamera != null && 
           !playerCamera.isCameraFrozen && 
           !playerCamera.isFreeCamera)
        {
            Vector3 camEuler = playerCamera.transform.eulerAngles;
            camEuler.x = 0f;
            camEuler.z = 0f;
            transform.localEulerAngles = camEuler;
        }
    }

    void Animation()
    {
        Vector3 displacement = movement;
        displacement.y = 0f;
        Vector3 localDirection = transform.InverseTransformDirection(displacement);
        Vector3 direction = localDirection.normalized;
        anim.SetInteger("MovementState", (int)movementState);
        anim.SetFloat("DirectionX", direction.x);
        anim.SetFloat("DirectionY", direction.z);
        anim.SetFloat("MovementSpeed", Mathf.Abs(displacement.magnitude));
        prevPosition = transform.position;
    }
}
