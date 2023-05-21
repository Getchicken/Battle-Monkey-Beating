using UnityEngine;

public class Climbing : MonoBehaviour
{
    [Header("Reference")]
    public Transform orientation;
    public Rigidbody rb;
    public LayerMask whatIsWall;
    public PlayerMovement pm;

    [Header("Climbing")]
    public float climbSpeed;
    public float maxClimbTime;
    private float climbTimer;

    private bool climbing;

    [Header("Detection")]
    public float detectionLenght;
    public float sphereCastRadius;
    public float maxWallLookAngle;
    private float wallLookAngle;

    private RaycastHit frontWallHit;
    private bool wallFront;

    [Header("ClimbJump")]
    public float climbJumpUpForce;
    public float climbJumpBackForce;
    public KeyCode climbJumpKey = KeyCode.Space;
    public int climbJumps;
    public int climbJumpsLeft;

    [Header("Exiting")]
    public bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    private Transform lastWall;
    private Vector3 lastWallNormal;
    public float minWallNormalAngleChange;
    private AnimationHandler ah;


    void Start()
    {
        ah = FindObjectOfType<AnimationHandler>().GetComponent<AnimationHandler>();
    }

    void Update()
    {
        WallCheck();
        StateMachine();

        if(climbing && !exitingWall) ClimbingMovement();    
    }

    private void StateMachine()
    {
        // climbing
        if (wallFront && Input.GetKey(KeyCode.W) && wallLookAngle < maxWallLookAngle && !exitingWall)
        {
            if (!climbing && climbTimer > 0)
                StartClimb();

            // Timer
            if (climbTimer > 0f) climbTimer -= Time.deltaTime;
            if (climbTimer <= 0f) StopClimb();
        }
        else if(exitingWall)
        {
            if (climbing) StopClimb();

            if(exitWallTimer > 0) exitWallTimer -= Time.deltaTime;
            if(exitWallTimer < 0f) exitingWall = false;
        }
        // 3rd State - none
        else
        {
            if (climbing) StopClimb();
        }

        //climb jumps
        if (wallFront && Input.GetKey(climbJumpKey) && climbJumpsLeft > 0f) ClimbJump();
    }

    private void WallCheck()
    {
        wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLenght, whatIsWall);
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

        bool newWall = frontWallHit.transform != lastWall || Mathf.Abs(Vector3.Angle(lastWallNormal, frontWallHit.normal)) > minWallNormalAngleChange;

        if ((wallFront && newWall) || pm.grounded)
        {
            climbTimer = maxClimbTime;
            climbJumpsLeft = climbJumps;
        }
    }

    private void StartClimb()
    {
        climbing = true;
        pm.climbing = true;
        ah.climbing = true;

        CamShaker.DoFov(100f);
        lastWall = frontWallHit.transform;
        lastWallNormal = frontWallHit.normal;
    }

    private void ClimbingMovement()
    {
        rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
    }

    private void StopClimb()
    {
        climbing = false;
        pm.climbing = false;
        ah.climbing = false;
        CamShaker.DoFov(80f);
    }

    private void ClimbJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 forceToApply = transform.up * climbJumpUpForce + frontWallHit.normal * climbJumpBackForce;
        
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);

        climbJumpsLeft--;
    }
}
