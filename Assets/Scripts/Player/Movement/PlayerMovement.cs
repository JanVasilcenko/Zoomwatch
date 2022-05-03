using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [Header("Movement")]
    private float moveSpeed;
    public float acceleration;
    public float walkSpeed;
    public float sprintSpeed;
    public float wallrunSpeed;
    public bool wallrunning;
    public float slideSpeed;
    public bool sliding;

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    
    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool readyToJump;
    public AudioClip jumpSound;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody rb;
    public PlayerCamera camera;

    public MovementState movementState;

 

    public enum MovementState {
        walking,
        sprinting,
        wallrunning,
        crouching,
        sliding,
        air
    }

    // Start is called before the first frame update
    private void Start() {
        rb = GetComponent<Rigidbody>();
       
        rb.freezeRotation = true;

        //to initialize jump
        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    private void Update() {
        //ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();
        
        //handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate() {
        MovePlayer();
    }

    private void MyInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //when to jump
        if (Input.GetButton("Jump") && readyToJump && grounded) {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        //start crouch
        if (Input.GetButtonDown("Crouch") && grounded) {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        //stop crouch
        if (Input.GetButtonUp("Crouch") && grounded) {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void StateHandler() {
        //Mode - Sliding
        // if (sliding)
        // {
        //     movementState = MovementState.sliding;
        //
        //     if (OnSlope() && rb.velocity.y < 0.1f)
        //     {
        //         desiredMoveSpeed = slideSpeed;
        //     }
        //
        //     else
        //     {
        //         desiredMoveSpeed = sprintSpeed;
        //     }
        // }
        
        //Mode - Wallrunning
        if (wallrunning)
        {
            movementState = MovementState.wallrunning;
            moveSpeed = wallrunSpeed;
        }
        
        //Mode - Crouching
        if (Input.GetButton("Crouch") && grounded) {
            movementState = MovementState.crouching;
            moveSpeed = crouchSpeed;
            //reset camera effects
            //camera.DoFov(60f);
        }

        //Mode - Sprinting
        else if (grounded && Input.GetButton("Sprint")) {
            movementState = MovementState.sprinting;
            //moveSpeed = sprintSpeed;
            moveSpeed += acceleration * Time.deltaTime;
            
            if (moveSpeed > sprintSpeed) {
                moveSpeed = sprintSpeed;
            }
            //camera effect for SPEEEEEDDD
            //camera.DoFov(80f);
        }

        //Mode - Walking
        else if (grounded) {
            movementState = MovementState.walking;
            moveSpeed = walkSpeed;
            //reset camera effects
            //camera.DoFov(60f);
        }

        //Mode - air
        else {
            movementState = MovementState.air;
            //reset camera effects
            //camera.DoFov(60f);
        }
        
        //check if desiredMoveSpeed has changed drastically
        // if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
        // {
        //     StopAllCoroutines();
        //     StartCoroutine(SmoothlyLerpMoveSpeed());
        // }
        //
        // else
        // {
        //     moveSpeed = desiredMoveSpeed;
        // }
        //
        // lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    // private IEnumerator SmoothlyLerpMoveSpeed()
    // {
    //     //smoothly lerp movementSpeed to desired value
    //     float time = 0;
    //     float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
    //     float startValue = moveSpeed;
    //
    //     while (time < difference)
    //     {
    //         moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
    //         time += Time.deltaTime;
    //         yield return null;
    //     }
    //
    //     moveSpeed = desiredMoveSpeed;
    // }
    
    private void MovePlayer() {
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //on slope
        if (OnSlope() && !exitingSlope) {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0) {
                rb.AddForce(Vector3.down * 100f, ForceMode.Force);
            }
        }

        //on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        //in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        //turn gravity off while on slope
        //rb.useGravity = !OnSlope();
    }

    private void SpeedControl() {
        //limiting speed on slope
        if (OnSlope() && !exitingSlope) {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        //limiting speed on ground or in air
        else {
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //limit velocity if needed
            if (flatVelocity.magnitude > moveSpeed) {
                Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
        }
    }

    private void Jump() {
        
        exitingSlope = true;

        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        AudioSource.PlayClipAtPoint(jumpSound, gameObject.transform.position);

    }

    private void ResetJump() {
        readyToJump = true;
        exitingSlope = false;
    }

    public bool OnSlope() {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f /*0.5f + 0.3f*/));
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction) {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
}
