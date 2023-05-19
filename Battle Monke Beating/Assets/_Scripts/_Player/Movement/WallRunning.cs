using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float maxWallRunTime;
    public float wallClimbSpeed;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    private float wallRunTimer;

    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;
    public KeyCode wallRunningJumpKey = KeyCode.Space;
    public KeyCode upwardsRunningKey = KeyCode.LeftShift;
    public KeyCode downwardsRunningKey = KeyCode.LeftControl;
    private bool upwardsRunning;
    private bool downwardsRunning;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minjumpHeight;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Gravity")]
    public bool useGravity;
    public float counterForce;

    [Header("Detection")]
    public bool exitingWall;
    public float exitingWallTime;
    private float exitingWallTimer;

    [Header("References")]
    public Transform orientation;
    private PlayerMovement pm;
    private AnimationHandler ah;
    private Rigidbody rb;
    public PlayerCam cam;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
        ah = GetComponent<AnimationHandler>();
    }

    void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if(pm.wallrunning)
            WallrunMovement();
    }

    void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minjumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        // Inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        upwardsRunning = Input.GetKeyDown(upwardsRunningKey);
        downwardsRunning = Input.GetKeyDown(downwardsRunningKey);

        // Wallrunning 
        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitingWall)
        { 
           if(!pm.wallrunning)
                StartWallrun();

           // wallrun time
            if (wallRunTimer > 0f)
                wallRunTimer -= Time.deltaTime;

            // wallrun Must end
            if(wallRunTimer <= 0f && pm.wallrunning)
            {
                exitingWall = true;
                exitingWallTimer = exitingWallTime;
            }
            // jump while wallrunning
            if (Input.GetKey(wallRunningJumpKey)) WallJump();
        }
        else if(exitingWall)
        {
            if(pm.wallrunning)
                StopWallrun();

            if (exitingWallTimer > 0)
                exitingWallTimer -= Time.deltaTime;

            if (exitingWallTimer <= 0f)
                exitingWall = false;
        }
        // state: no state
        else
        {
            if (pm.wallrunning)
                StopWallrun();
        }
    }

    private void StartWallrun()
    {
        pm.wallrunning = true;
        ah.wallrunning = true;
        wallRunTimer = maxWallRunTime;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

    }

    private void WallrunMovement()
    {
        rb.useGravity = useGravity;

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;
        
        // Push Forward
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);
        // camera tilt and fov
        CamShaker.DoFov(100f);
        if (wallLeft) CamShaker.DoLeaning(-5f);
        if (wallRight) CamShaker.DoLeaning(5f);

        // upwards/downwards force
        if (upwardsRunning)
            rb.velocity = new Vector3(rb.velocity.x, wallClimbSpeed * 2f, rb.velocity.z);
        if (downwardsRunning)
            rb.velocity = new Vector3(rb.velocity.x, -wallClimbSpeed, rb.velocity.z);
        // Push against Wall
        if (!(wallLeft && horizontalInput > 0) && !(wallRight && verticalInput < 0))
            rb.AddForce(-wallNormal * 100f, ForceMode.Force);

        if (useGravity)
            rb.AddForce(transform.up * counterForce * 1.3f, ForceMode.Force);
    }
    /*
    if (upwardsRunning)
            rb.AddForce(Vector3.up * wallClimbSpeed * 1.35f, ForceMode.Force);
        if (downwardsRunning)
            rb.AddForce(Vector3.down * wallClimbSpeed, ForceMode.Force);
    */
    private void StopWallrun()
    {
        pm.wallrunning = false;
        ah.wallrunning = false;
        // stop cam effects
        CamShaker.DoFov(80f);
        CamShaker.DoLeaning(0f);
    }

    private void WallJump()
    {
        // enter exiting wall state
        exitingWall = true;
        exitingWallTimer = exitingWallTime;
        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        // reset y velocity and add force
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
        CamShaker.DoOnShake(0.3f, 0.004f);
    }
}
