using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Shooting : MonoBehaviour
{
    public float damage = 100f;
    public float range = 10000f;
    public float fireRate = 15f;
    public float impactForce = 30f;
    public int maxAmmo = 10;
    public int currentAmmo;
    public float bulletSpeed = 60f;

    public GameObject fpsCam, bulletPrefab;
    public GameObject muzzleFlash;
    public Animator anim;
    public Transform bulletSpawnPoint;
    public AudioSource pistolShot_fx;
    public GameObject impactEffect;


    private float nextTimeToFire = 0f;
    private UiManager _uiManager;

    void Start()
    {
        currentAmmo = maxAmmo;

        _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire && currentAmmo >= 1 && anim.GetBool("Reload") == false)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        if (Input.GetKeyDown("r"))
        {
            anim.Play("PistolReload");
            anim.SetBool("Reload", true);
        }
    }

    void Reload()
    {
        currentAmmo = maxAmmo;
        _uiManager.UpdateAmmo(currentAmmo);
        anim.SetBool("Reload", false);
    }

    void ResetReloadBool()
    {
        anim.SetBool("Reload", true);
    }

    void Shoot()
    {
        //var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        //bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
        GameObject Flash = Instantiate(muzzleFlash, bulletSpawnPoint);
        Destroy(Flash, 0.1f);
        pistolShot_fx.Play();

        //RaycastHit hit;
        currentAmmo--;
        _uiManager.UpdateAmmo(currentAmmo);
        /* I need to instantiate coconuts or bananas
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
            Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        }
        */
    }
}
