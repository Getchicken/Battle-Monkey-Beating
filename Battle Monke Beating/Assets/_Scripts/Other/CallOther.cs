using UnityEngine;

public class CallOther : MonoBehaviour
{
    public float radius;

    private void OnTriggerEnter(Collider other)
    {
        // touching an item
        if (other.CompareTag("Enemy") || other.CompareTag("Ground"))
        {
            GetComponent<Explosion>().Explode();
        }

        // Get the center position of the sphere (in this case, the position of the GameObject this script is attached to)
        Vector3 center = transform.position;

        // Perform the overlap sphere check
        Collider[] colliders = Physics.OverlapSphere(center, radius);

        // Iterate through the colliders
        foreach (Collider collider in colliders)
        {
            // Perform actions or checks on the overlapping objects
            // For example, you can check the tag of the collider's GameObject
            if (collider.CompareTag("Enemy"))
            {
                // Do something with the enemy object
                EnemyTarget enemyTarget = collider.GetComponent<EnemyTarget>();
                PlayerStats _playerStats = FindObjectOfType<PlayerStats>();
                enemyTarget.TakeDamage(_playerStats._bananaBlasterDamage);
            }
        }
    }
}
