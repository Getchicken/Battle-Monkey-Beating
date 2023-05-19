using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionDamage;
    Rigidbody _playerRB;
    Explosion _explosion;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerObj"))
        {
            _playerRB = other.gameObject.GetComponentInParent<Rigidbody>();
            _playerRB.AddForce(Vector3.up * _explosionForce, ForceMode.Impulse);

            //Explosion
            other.gameObject.GetComponentInParent<PlayerStats>().TakeDamage(_explosionDamage);

            _explosion = GetComponent<Explosion>();
            _explosion.Explode();
        }
    }
}
