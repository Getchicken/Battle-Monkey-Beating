using UnityEngine;

public class EnemyTarget : EnemyStats, IHealable
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject DamageText;
    AnimationHandler ah;
    Rigidbody rb;
    BuffSpawner buffspawner;
    

    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
        ah = FindObjectOfType<AnimationHandler>();
        _eCurrentHealth = _eMaxHealth;
        buffspawner = GetComponent<BuffSpawner>();
        SetMaxHealthFromHealthLevel();
    }

    private void SetMaxHealthFromHealthLevel()
    {
        _eMaxHealth = _ehealthLevel * 10;
    }

    public void TakeDamage(float damageAmount)
    {
        //Debug.Log("your health is:" + _eCurrentHealth);

        _eCurrentHealth -= damageAmount;
        Instantiate(DamageText, transform.position, Quaternion.identity, transform).GetComponent<DamageNumbers>().Initialise(damageAmount);

        if (_eCurrentHealth <= 0)
        {
            // gain xp for _player
            buffspawner.SpawnBuff();
            Destroy(gameObject);
        }
    }

    public void Heal(float healAmount)
    {
        if (_eCurrentHealth < _eMaxHealth)
        {
            _eCurrentHealth += healAmount;
        }
        else if(_eCurrentHealth > _eMaxHealth)
        {
            _eCurrentHealth = _eMaxHealth;
        }
    }
}
