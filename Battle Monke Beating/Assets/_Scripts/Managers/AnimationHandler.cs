using System.Collections;
using UnityEngine;


public class AnimationHandler : MonoBehaviour
{
    [Header("Variables")]
    private float horizontalInput;
    private float verticalInput;
    public float AnimationPerCent;
    public bool wallrunning = false;
    public bool climbing = false;
    public bool isAttacking = false;
    public bool isReloading = false;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("References")]
    private Animator anim;
    private BananaGun bananaGun;
    private Shooting shooting;
    public Transform orientation;
    [SerializeField] private GameObject _weaponHolder;
    public Camera cam;
    public Vector3 moveDirection;



    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        CheckAnimation();
        MyInput();
        HandleMovementWeapons();
    }

    private void CheckAnimation()
    {
        if (Input.GetKeyDown("r") && !isReloading)
        {
            anim.SetBool("isReloading", true);
            isReloading = true;

            SetUp();
            anim.Play("Reload");
            
            StartCoroutine(ResetBools(1f));
        }
    }

    private IEnumerator ResetBools(float delay)
    {
        yield return new WaitForSeconds(delay);
        anim.SetBool("isReloading", false);
        isReloading = false;
    }
    private void SetUp()
    {
        bananaGun = GetComponentInChildren<BananaGun>();
        shooting = GetComponentInChildren<Shooting>();

        if (shooting != null)
        {
            shooting.Reload();
        }
        if(bananaGun != null)
        {
            bananaGun.Reload();
        }
    }
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
    }


    private void HandleMovementWeapons()
    {
        // if youre attacking/reloading return - avoids anim cancel
        if(isReloading) return;

        if (climbing == true)
        {
            anim.SetFloat("Speed", 1.33f, 0.01f, Time.deltaTime);
            _weaponHolder.SetActive(false);
        }
        else
        {
            _weaponHolder.SetActive(true);
            if (moveDirection == Vector3.zero) // Idle
            {
                anim.SetFloat("Speed", 0, 0.01f, Time.deltaTime);
            }
            else if (moveDirection != Vector3.zero) // Walk
            {
                anim.SetFloat("Speed", 0.33f, 0.01f, Time.deltaTime);
            }
        }
    }
}
