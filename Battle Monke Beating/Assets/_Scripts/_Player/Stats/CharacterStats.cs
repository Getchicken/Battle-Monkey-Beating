using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Health")]
    public float healthLevel = 1;
    public float maxHealth;
    public float currentHealth;

    [Header("Player Damage")]
    public float _kunaiDamage = 8f;
    public float _kunaiElementalDamage = 8f;
    public float _swordSpecialDamage = 35f;
    public float _shortSwordDamage = 15f;
    public float _knockbackForce = 2f;
}

public class EnemyStats : CharacterStats
{
    [Header("Enemy Damage")]
    public float _eMaxHealth = 100;
    public float _eCurrentHealth;
}
