using UnityEngine;

public class OnHitCall : MonoBehaviour
{
    public float _damage;
    public float _heal;

    public void SetValues(float _attackDamage, float _healAmmount)
    {
        _damage = _attackDamage;
        _heal = _healAmmount;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerObj"))
        {
            PlayerStats playerStats = other.gameObject.GetComponentInParent<PlayerStats>();
            if (playerStats == null) print("no playeStats");
            if(playerStats != null)
            {
                Debug.Log("_player hit");
                playerStats.TakeDamage(_damage);
            }
        }

        if(other.gameObject.CompareTag("Allies"))
        {
            EnemyTarget enemyTarget = other.gameObject.GetComponent<EnemyTarget>();
            if (enemyTarget == null) print("no playeStats");
            if (enemyTarget != null)
            {
                Debug.Log("healed");
                enemyTarget.Heal(_heal);
            }
        }
    }
}
