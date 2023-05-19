using UnityEngine;

public class EnemyTarget : EnemyStats, IHealable
{
    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject DamageText;
    AnimationHandler ah;
    Rigidbody rb;
    

    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
        ah = player.GetComponent<AnimationHandler>();
        _eCurrentHealth = _eMaxHealth;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Kunai"))
        {
            CharacterStats characterStats = other.GetComponent<CharacterStats>();
            if (characterStats == null) return;

            Destroy(other.gameObject);
        }
        if (other.CompareTag("Sword"))
        {
            CharacterStats characterStats = other.GetComponent<CharacterStats>();
            if (characterStats == null) return;

            if (ah.inSpecial)
            {
                TakeDamage(characterStats._swordSpecialDamage);
            }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        Debug.Log("your health is:" + _eCurrentHealth);

        _eCurrentHealth -= damageAmount;
        Instantiate(DamageText, transform.position, Quaternion.identity, transform).GetComponent<DamageNumbers>().Initialise(damageAmount);

        if (_eCurrentHealth <= 0)
        {
            // gain xp for _player
            Destroy(gameObject);
        }
    }

    private void TakeKnockback()
    {
        // apply Knockback on enemys
        Vector3 knockbackDirection = transform.position - player.transform.position;
        Vector3 knockbackForce = knockbackDirection.normalized * _knockbackForce;
        rb.AddForce(knockbackForce, ForceMode.Impulse);
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
