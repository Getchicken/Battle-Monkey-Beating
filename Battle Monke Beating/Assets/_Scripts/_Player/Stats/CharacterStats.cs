using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Health")]
    public float healthLevel = 1;
    public float maxHealth;
    public float currentHealth;

    [Header("Player Damage")]
    public float _bananaBlasterDamage = 9f;
    public float _bananaGunDamage = 4f;
    public float _knockbackForce = 2f;
}

public class EnemyStats : CharacterStats
{
    [Header("Enemy Damage")]
    public float _eMaxHealth = 100;
    public float _eCurrentHealth;
}
