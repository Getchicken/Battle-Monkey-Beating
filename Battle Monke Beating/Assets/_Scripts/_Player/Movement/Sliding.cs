using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Sliding : MonoBehaviour
{
    [Header("Reference")]
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    private PlayerMovement pm;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;
    public TextMeshProUGUI SlideText;

    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    public KeyCode slideKey = KeyCode.LeftControl;
    public KeyCode slideJumpKey = KeyCode.Space;
    private float horizontalInput;
    private float verticalInput;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

        startYScale = playerObj.localScale.y;
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0))
            StartSlide();

        if (Input.GetKeyUp(slideKey) && pm.sliding)
            StopSlide();

        if (pm.sliding)
            SlidingMovement();
    }

    void StartSlide()
    {
        pm.sliding = true;

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        rb.AddForce(Vector3.down * 14f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
        DisplaySlideTime();
    }

    void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // Slide Time indicator 
        DisplaySlideTime();

        // sliding normal
        if (!pm.OnSlope() || rb.velocity.y > -0.1f )
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
            slideTimer -= Time.deltaTime;
        }
        // sliding down Slopes
        else if(pm.OnSlope())
        {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }

        if(slideTimer <= 0)
            StopSlide();
    }

    void StopSlide()
    {
        pm.sliding = false;
        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
        slideTimer = maxSlideTime;
        DisplaySlideTime();
    }

    void DisplaySlideTime()
    {
        SlideText.text = string.Format("{0:0.0}", slideTimer);
    }
}
