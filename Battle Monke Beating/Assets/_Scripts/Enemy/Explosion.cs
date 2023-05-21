using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private GameObject parentObject;
    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private float shakeStrength = 0.05f;

    [SerializeField] ParticleSystem _explosionEffect;
    public void Explode()
    {
        // Particle System
        Instantiate(_explosionEffect, transform.position, Quaternion.identity);

        // Sound
        //AudioManager.Instance.Play("Explosion", 1, 1); I dont have this sound yet


        // Screenshake
        CamShaker.DoOnShake(shakeDuration, shakeStrength);

        //Destroy
        Invoke("Destroy", 0.03f);
    }

    void Destroy()
    {
        Destroy(parentObject);
    }
}
