using UnityEngine;

public class Jumppad : MonoBehaviour
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private float shakeStrength = 0.05f;
    [SerializeField] ParticleSystem _jumpParticle;
    Rigidbody _playerRB;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerObj"))
        {
            _playerRB = other.gameObject.GetComponentInParent<Rigidbody>();
            _playerRB.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);

            // Instantiate the particle system prefab
            ParticleSystem particleSystemInstance = Instantiate(_jumpParticle, other.gameObject.transform.position, transform.rotation, other.gameObject.transform);

            // Destroy the particle system after its duration
            float particleSystemDuration = particleSystemInstance.main.duration;
            Destroy(particleSystemInstance.gameObject, particleSystemDuration);

            //Cam shake
            CamShaker.DoOnShake(shakeDuration, shakeStrength);
        }
    }
}
