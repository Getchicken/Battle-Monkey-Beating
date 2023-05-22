using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EZCameraShake;
using System;
using Unity.VisualScripting;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float wallrunSpeed;
    public float groundDrag;
    public float airDrag;
    public float airMultiplier;
    public float FallMultiplier;
    public float slideSpeed;
    public float climbSpeed;
    private float desiredMoveSpeed;
    [HideInInspector] public float lastDesiredMoveSpeed;

    [Header("Jump")]
    private bool readyToJump = true;
    public float jumpForce;
    public float jumpfCD;
    public int jumpsLeft;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("CamShake")]
    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private float shakeStrength = 0.2f;

    [Header("Slope")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;
    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;
    [HideInInspector] public float horizontalInput;
    [HideInInspector] public float verticalInput;

    [Header("GroundCheck")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    public TextMeshProUGUI speedText;
    public Transform orientation;
    public Transform playerObj;

    private Vector3 moveDirection;
    private Rigidbody rb;
    private AudioManager am;
    public Climbing climbingScript;

    private float randomVolume;
    private float randomPitch;

    public MovementState state;
    public bool sliding;
    public bool wallrunning;
    public bool climbing;

    public enum MovementState
    {
        walking,
        sprinting,
        wallrunning,
        climbing,
        crouching,
        sliding,
        air,
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        startYScale = transform.localScale.y;
    }

    void Update()
    {
        // whatIsGround Check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        DisplaySpeed();
        MyInput();
        SpeedControl();
        StateHandler();
        SoundControl();
        // apply whatIsGround drag - speed control
        if (grounded)
            rb.drag = groundDrag;   
        else
            rb.drag = airDrag;
    }

    void FixedUpdate()
    {
        MovePlayer();

        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * FallMultiplier * Time.deltaTime;
        }
    }

    void StateHandler()
    {
        // climbing
        if(climbing)
        {
            state = MovementState.climbing;
            desiredMoveSpeed = climbSpeed;
        }
        // wallrun mode
        else if(wallrunning)
        {
            state = MovementState.wallrunning;
            desiredMoveSpeed = wallrunSpeed;
        }

        // slide mode
        else if(sliding)
        {
            state = MovementState.sliding;

            if (OnSlope() && rb.velocity.y < 0.1f)
            {
                desiredMoveSpeed = slideSpeed;
            }
            else
                desiredMoveSpeed = sprintSpeed;
        }
        // crouch mode
        else if(Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }
        // Sprint mode
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }
        // walk mode
        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }
        // air mode
        else
            state = MovementState.air;

        // check if the desiredMoveSpeed has changed drastically 
        if(Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = lastDesiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smooth lerp the moveSpeed to the desiredSpeed
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while(time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
                time += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }

    void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        ResetJumpsLeft();

        // jump
        if(Input.GetKey(jumpKey) && readyToJump && jumpsLeft > 0)
        {
            readyToJump = false;

            Jump();
            Invoke(nameof(ResetJump), jumpfCD);
        }

        // start crouch
        if(Input.GetKeyDown(crouchKey))
        {
            playerObj.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            playerObj.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    void MovePlayer()
    {
        // fix running while walljumping
        if (climbingScript.exitingWall) return;

        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope 
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            CamShaker.DoLeaning(1.5f); //lean on slopes
                                       
            if (rb.velocity.y < 0)
            {
                rb.AddForce(Vector3.down * 50f, ForceMode.Force);
            }
            
        }
        // on whatIsGround
        else if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            CamShaker.DoLeaning(1.5f);
        }
        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        if(!wallrunning) rb.useGravity = !OnSlope();
    }

    void SpeedControl()
    {
        // limitng speed on slope
        if(OnSlope() && !exitingSlope)
        {
            if(rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }
        // limiting speed on whatIsGround and or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if to fast
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    void Jump()
    {
        exitingSlope = true;
        jumpsLeft--;
        //reset y Velocity?
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        CamShaker.DoOnShake(shakeDuration, shakeStrength);  
    }

    void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    void ResetJumpsLeft()
    {
        if(grounded || state == MovementState.wallrunning || state == MovementState.climbing && state != MovementState.air)
        {
            jumpsLeft = 1;
        }
    }

    public bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    private void DisplaySpeed()
    {
        speedText.text = string.Format("{0:0.0}", moveSpeed);
    }
    
    private void SoundControl()
    {
        //check if an AudioManager exists
        if(AudioManager.Instance == null)
        {
            Debug.Log("AudioManager is null");
            return;
        }
        //check if the _player is not moving
        if (state != MovementState.walking && state != MovementState.sprinting)
        {
            AudioManager.Instance.Stop("Footsteps");
            return;
        }
        // if you are moving and on the whatIsGround
        if (grounded && Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            // check if the footsteps sound is already playing
            if (!AudioManager.Instance.sounds[1].source.isPlaying)
            {
                // check the state to set the volume and pitch
                randomVolume = state == MovementState.sprinting ? UnityEngine.Random.Range(0.5f, 0.6f) : UnityEngine.Random.Range(0.45f, 0.5f);
                randomPitch = UnityEngine.Random.Range(0.85f, 0.95f);

                AudioManager.Instance.Play("Footsteps", randomVolume, randomPitch);
            }
        }
        else
        {
            AudioManager.Instance.Stop("Footsteps");
        }
    }

    public void BuffSpeed(float buffAmount)
    {
        int randomIndex = UnityEngine.Random.Range(0, 5); // generate a random integer between 0 and 4
        switch (randomIndex)
        {
            case 0:
                walkSpeed += buffAmount;
                break;
            case 1:
                sprintSpeed += buffAmount;
                break;
            case 2:
                wallrunSpeed += buffAmount;
                break;
            case 3:
                slideSpeed += buffAmount;
                break;
            case 4:
                climbSpeed += buffAmount;
                break;
        }
    }
}
