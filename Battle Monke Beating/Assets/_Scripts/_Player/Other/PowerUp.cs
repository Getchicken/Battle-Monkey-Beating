using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpEffect powerUpEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerObj"))
        {
            powerUpEffect.Apply(other.gameObject);
            Destroy(gameObject);
        }
    }
}
