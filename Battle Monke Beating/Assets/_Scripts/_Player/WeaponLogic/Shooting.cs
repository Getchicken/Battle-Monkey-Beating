using UnityEngine;


public class Shooting : MonoBehaviour
{
    public float range = 10000f;
    public float fireRate;
    public float impactForce = 30f;
    public int maxAmmo;
    public int currentAmmo;
    [SerializeField] private float bulletSpeed;

    [SerializeField] private GameObject bulletPrefab;
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

        _uiManager = FindObjectOfType<UiManager>();

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextTimeToFire && currentAmmo >= 1 && anim.GetBool("isReloading") == false)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    public void Reload()
    {
        currentAmmo = maxAmmo;
        _uiManager.UpdateBlasterAmmo(currentAmmo);
    }

    void Shoot()
    {
        GameObject Flash = Instantiate(muzzleFlash, bulletSpawnPoint);
        Destroy(Flash, 1.3f);
        pistolShot_fx.Play();

        currentAmmo--;
        _uiManager.UpdateBlasterAmmo(currentAmmo);

        //Instantiate Projectile 
        GameObject bulletPrefabInstance = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

        // add force
        Rigidbody bulletPrefabRigidbody = bulletPrefabInstance.GetComponent<Rigidbody>();
        bulletPrefabRigidbody.AddForce(Camera.main.transform.forward * bulletSpeed * 1.3f, ForceMode.Impulse);
    }
}
