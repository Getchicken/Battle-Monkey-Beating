using UnityEngine;

public class CallOther : MonoBehaviour
{
    public float radius;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with an object on the "Enemy" or "Ground" layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") || collision.gameObject.layer == LayerMask.NameToLayer("whatIsGround"))
        {
            GetComponent<Explosion>().Explode();
        }

        Vector3 center = transform.position;

        // Perform the overlap sphere check
        Collider[] colliders = Physics.OverlapSphere(center, radius);

        // Iterate through the colliders
        foreach (Collider collider in colliders)
        {
            // If the layer is enemy apply damage
            if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                // Do something with the enemy object
                EnemyTarget enemyTarget = collider.GetComponent<EnemyTarget>();
                PlayerStats _playerStats = FindObjectOfType<PlayerStats>();
                enemyTarget.TakeDamage(_playerStats._bananaBlasterDamage);
            }
        }
    }

}
