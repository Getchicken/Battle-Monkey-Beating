using System.Collections;
using UnityEngine;
using TMPro;

public class AnimationHandler : MonoBehaviour
{
    [Header("Variables")]
    private int currentAnimation_Katana = 1;
    private float horizontalInput;
    private float verticalInput;
    public float AnimationPerCent;
    public bool wallrunning = false;
    public bool climbing = false;
    public bool isAttacking = false;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("References")]
    private Animator anim;
    private TrailRenderer trailRenderer;
    public Transform orientation;
    public GameObject Katana;
    public GameObject Sheath;
    public TextMeshProUGUI Q_CD_Text;
    public Camera cam;
    RotateSword rotateSword;
    public Vector3 moveDirection;
    public CombatState cState = CombatState.normal;

    [Header("Special Ability")]
    public KeyCode specialKey = KeyCode.Q;
    public float specialCDDuration = 10f;
    public float specialCD;
    public float specialDuration = 8f;
    public bool inSpecial = false;


    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= AnimationPerCent)
        {
            isAttacking = false;
        }

        MyInput();
        HandleMovementKunai();
        HanldeMovementKatana();
        HandleAttackAnimationsKatana();
        DisplayCD();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (Input.GetKeyDown(specialKey) && specialCD <= 0)
        {
            inSpecial = true;
            CombatStateHandler();
            StartCoroutine(SpecialCoroutine());
            StartCoroutine(SpecialCDCoroutine());
        }
    }

    public enum CombatState
    {
        normal,
        special,
    }

    private void CombatStateHandler()
    {
        if (inSpecial)
        {
            // if the Energy is full and q pressed and cd is up activate special
            cState = CombatState.special;
            anim.SetBool("inNormalState", false);

            // Equip anim
            anim.Play("Katana Equip_02", 0, 0);
            isAttacking = true;

            // Deactivate cState Normal Weapons - Enable cState Special Weapon
            Katana.SetActive(true);
            Sheath.SetActive(true);
            Invoke("TurnOff", 0.8f);
        }
    }

    private void HandleMovementKunai()
    {
        // if your attacking return - avoids anim cancel
        if (isAttacking) return;
        // return if your in cState Special
        if (cState == CombatState.special) return;

        if (climbing == true)
        {
            anim.SetFloat("Speed", 1.33f, 0.01f, Time.deltaTime);
        }
        else if (wallrunning == true)
        {
            anim.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
        }
        else
        {
            if (moveDirection == Vector3.zero)
            {
                anim.SetFloat("Speed", 0, 0.5f, Time.deltaTime);
            }
            else if (moveDirection != Vector3.zero && !Input.GetKey(sprintKey))
            {
                anim.SetFloat("Speed", 0.33f, 0.6f, Time.deltaTime);
            }
            else if (moveDirection != Vector3.zero && Input.GetKey(sprintKey))
            {
                anim.SetFloat("Speed", 0.66f, 0.33f, Time.deltaTime);
            }
            //ShortSword.SetActive(true);
            //ShortSword2nd.SetActive(false);
            //Kunai.SetActive(true);
        }
    }

    private void HanldeMovementKatana()
    {
        if (isAttacking) return;
        if (cState == CombatState.normal) return;

        if (climbing == true)
        {
            Katana.SetActive(false);
            anim.SetFloat("Speed2", 1.5f, 0.01f, Time.deltaTime);
        }
        else if (wallrunning == true)
        {
            anim.SetFloat("Speed2", 1f, 0.1f, Time.deltaTime);
        }
        else
        {
            if (moveDirection == Vector3.zero)
            {
                anim.SetFloat("Speed2", 0, 0.25f, Time.deltaTime);
            }
            else if (moveDirection != Vector3.zero && !Input.GetKey(sprintKey))
            {
                anim.SetFloat("Speed2", 0.33f, 0.4f, Time.deltaTime);
            }
            else if (moveDirection != Vector3.zero && Input.GetKey(sprintKey))
            {
                anim.SetFloat("Speed2", 0.66f, 0.25f, Time.deltaTime);
            }
            Katana.SetActive(true);
        }
    }

    private void HandleAttackAnimationsKatana()
    {
        if (cState == CombatState.normal) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (currentAnimation_Katana == 1 && isAttacking == false)
            {
                anim.Play("Katana N1", 0, 0);

                isAttacking = true;
                currentAnimation_Katana = 2;
            }
            else if (currentAnimation_Katana == 2 && isAttacking == false)
            {
                anim.Play("Katana N2", 0, 0);

                isAttacking = true;
                currentAnimation_Katana = 3;
            }
            else if (currentAnimation_Katana == 3 && isAttacking == false)
            {
                anim.Play("Katana N3", 0, 0);

                isAttacking = true;
                currentAnimation_Katana = 4;
            }
            else if (currentAnimation_Katana == 4 && isAttacking == false)
            {
                anim.Play("Katana N4", 0, 0);

                isAttacking = true;
                currentAnimation_Katana = 1;
            }
        }
    }


    private IEnumerator SpecialCoroutine()
    {
        specialDuration = 12f;

        while (specialDuration > 0)
        {
            specialDuration -= Time.deltaTime;
            yield return null;
        }

        inSpecial = false;
        // if the duration ends go to normal state
        Katana.SetActive(false);
        cState = CombatState.normal;
        anim.SetBool("inNormalState", true);
    }

    private IEnumerator SpecialCDCoroutine()
    {
        specialCD = specialCDDuration;

        while (specialCD > 0)
        {
            specialCD -= Time.deltaTime;
            yield return null;
        }
        specialCD = 0f;
    }

    private void TurnOff()
    {
        Sheath.SetActive(false);
    }

    void DisplayCD()
    {
        Q_CD_Text.text = string.Format("{0:0.0}", specialCD);
    }
}
