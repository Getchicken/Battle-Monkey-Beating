using UnityEngine;


public class PlayerStats : CharacterStats
{
    [Header("References")]
    public GameObject Player;
    public GameObject ArmsPivot;
    public GameObject UiCanvas;
    public GameObject XpBar;
    public UiManager uiManager;
    PlayerMovement pm;
    LevelSystem ls;

    [Header("Detection")]
    Animator animator;

    [Header("Healing")]
    public HealthBar healthbar;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        uiManager = UiCanvas.GetComponent<UiManager>();
        // for the death
        pm = GetComponentInParent<PlayerMovement>();
        pm.enabled = true;
    }

    void Start()
    {
        // Ui
        ls = XpBar.GetComponentInChildren<LevelSystem>();

        //health bar
        maxHealth = SetMaxHealthFromHealthLevel(ls.level);
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
        _swordSpecialDamage = 15f;
    }

    private float SetMaxHealthFromHealthLevel(int level)
    {
        maxHealth = (healthLevel * 0.1f) * ((100f - level));
        return maxHealth;
    }

    public void IncreaseMaxHealthLevel(int level)
    {
        healthLevel++;
        maxHealth = (healthLevel * 0.1f) * ((100 - level));
        currentHealth = maxHealth;
        healthbar.SetCurrentHealth(maxHealth);
    }

    public void BuffMaxHealth(float amount)
    {
        maxHealth += amount * ls.level;
        healthbar.SetCurrentHealth(maxHealth);
    }

    public void IncreaseDamage(float _levelMultiplier)
    {
        _swordSpecialDamage += (_levelMultiplier * ls.level);
        _bananaDamage += (_levelMultiplier * ls.level);
        _coconutDamage += (_levelMultiplier * ls.level);
    }

    public void TakeDamage(float damage)
    {
        currentHealth = currentHealth - damage;
        healthbar.SetCurrentHealth(currentHealth);

        if (currentHealth <= 0f)
        {
            if(uiManager == null)
            {
                uiManager = UiCanvas.GetComponent<UiManager>();
            }
            if (uiManager != null) uiManager.DeathUI();
        }
    }


    public void Healing(float healAmount)
    {
        if (currentHealth <= maxHealth)
        {
            currentHealth += healAmount;
            healthbar.SetCurrentHealth(currentHealth);
        }
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
            healthbar.SetCurrentHealth(currentHealth);
        }
    }
}
