using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private GameObject parentObject;
    [SerializeField] private AudioSource source;
    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private float shakeStrength = 0.05f;

    [SerializeField] ParticleSystem _explosionEffect;
    public void Explode()
    {
        // Particle System
        Instantiate(_explosionEffect, transform.position, Quaternion.identity);

        // Sound
        AudioManager.Instance.Play("Explosion1", 0.05f, 1f);


        // Screenshake
        CamShaker.DoOnShake(shakeDuration, shakeStrength);

        //Destroy
        Invoke("Destroy", 0.02f);
    }

    void Destroy()
    {
        Destroy(parentObject);
    }
}
