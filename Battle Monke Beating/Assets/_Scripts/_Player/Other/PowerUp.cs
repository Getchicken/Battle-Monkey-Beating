using UnityEngine;

public class PowerUp : MonoBehaviour
{
    //public PowerUpEffect powerUpEffect;
    public PowerUpEffect[] powerUpEffect;
    [SerializeField] private ParticleSystem particleSystem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerObj"))
        {
            foreach (PowerUpEffect buff in powerUpEffect)
            {
                // Apply the buff to the player
                buff.Apply(other.gameObject);
            }
            Instantiate(particleSystem, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
