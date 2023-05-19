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
            LevelSystem ls = XpBar.GetComponent<LevelSystem>();

            //health bar
            maxHealth = SetMaxHealthFromHealthLevel(ls.level);
            currentHealth = maxHealth;
            healthbar.SetMaxHealth(maxHealth);
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
        }

        public void TakeDamage(float damage)
        {
            currentHealth = currentHealth - damage;
            healthbar.SetCurrentHealth(currentHealth);

            if (currentHealth <= 0f)
            {
                if(uiManager == null)
                {
                    print("Game over");
                    uiManager = UiCanvas.GetComponent<UiManager>();
                }
                if (uiManager != null) uiManager.DeathUI();
            }
        }

        public void Die()
        {
            // everything is handled in UiCanvas so far
        }

        public void Healing(int healAmount)
        {
            if (currentHealth <= maxHealth)
            {
                currentHealth += healAmount;
                healthbar.SetCurrentHealth(currentHealth);
            }
        }
    }