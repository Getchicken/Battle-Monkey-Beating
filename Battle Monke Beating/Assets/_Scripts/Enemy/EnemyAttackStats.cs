using UnityEngine;

public class EnemyAttackStats : MonoBehaviour
{
    [Header("DontMove Stats")]
    public float _attackDamage = 6f;
    public float _attackRange = 10f;
    public float _fireRate = 1f;
    public float _nextTimeToFire;
    public bool _playerInAttackRange;
}
