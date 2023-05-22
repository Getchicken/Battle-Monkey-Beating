using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Health")]
    public float healthLevel = 1;
    public float maxHealth;
    public float currentHealth;

    [Header("Player Damage")]
    public float _bananaBlasterDamage = 6f;
    public float _bananaGunDamage = 8f;
    public float _knockbackForce = 2f;
}

public class EnemyStats : CharacterStats
{
    [Header("Enemy Damage")]
    public float _ehealthLevel = 1;
    public float _eMaxHealth;
    public float _eCurrentHealth;
}
