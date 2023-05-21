using UnityEngine;

public class BananaGun : MonoBehaviour
{
    public float range = 10000f;
    public float fireRate;
    public float impactForce = 30f;
    public int maxAmmo = 10;
    public int currentAmmo;
    public float bulletSpeed = 60f;

    [SerializeField] private GameObject playerCam;
    [SerializeField] private LayerMask Enemy;
    public GameObject muzzleFlash;
    public Animator anim;
    public Transform bulletSpawnPoint;
    public AudioSource pistolShot_fx;
    public GameObject impactEffect;


    private float nextTimeToFire = 0f;
    private UiManager _uiManager;
    private PlayerStats _playerStats;

    void Start()
    {
        currentAmmo = maxAmmo;

        _uiManager = FindObjectOfType<UiManager>();
        _playerStats = FindObjectOfType<PlayerStats>();
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
        _uiManager.UpdateAmmo(currentAmmo);
    }

    void Shoot()
    {
        GameObject Flash = Instantiate(muzzleFlash, bulletSpawnPoint);
        Destroy(Flash, 1.3f);
        pistolShot_fx.Play();

        RaycastHit hit;
        currentAmmo--;
        _uiManager.UpdateAmmo(currentAmmo);

        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, range, Enemy))
        {
            EnemyTarget enemyTarget = hit.transform.GetComponent<EnemyTarget>();

            enemyTarget.TakeDamage(_playerStats._bananaGunDamage);
            // Damage????
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
            Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }
}
