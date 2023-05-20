using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Shooting : MonoBehaviour
{
    public float damage = 100f;
    public float range = 10000f;
    public float fireRate = 15f;
    public float impactForce = 30f;
    public int maxAmmo = 10;
    public int currentAmmo;
    public float bulletSpeed = 60f;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private bool Blaster;
    [SerializeField] private bool Launcher;
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

        _uiManager = GameObject.Find("UI_Canvas").GetComponent<UiManager>();

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
            anim.SetBool("Reload", true);
            Reload();
        }
    }

    void Reload()
    {
        currentAmmo = maxAmmo;
        _uiManager.UpdateBlasterAmmo(currentAmmo);
        _uiManager.UpdateCocoAmmo(currentAmmo);
        anim.SetBool("inNormalState", false);
        StartCoroutine(ReloadDelay(1.5f));
    }

    IEnumerator ReloadDelay(float nextTimeToFire)
    {
        yield return new WaitForSeconds(nextTimeToFire);
        anim.SetBool("Reload", false);
        anim.SetBool("inNormalState", true);
    }

    void Shoot()
    {
        GameObject Flash = Instantiate(muzzleFlash, bulletSpawnPoint);
        Destroy(Flash, 1.3f);
        pistolShot_fx.Play();

        currentAmmo--;
        if(!Launcher)
            _uiManager.UpdateBlasterAmmo(currentAmmo);
        if (!Blaster && Launcher)
            _uiManager.UpdateCocoAmmo(currentAmmo);

        //Instantiate Projectile 
        GameObject bulletPrefabInstance = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

        // add force
        Rigidbody bulletPrefabRigidbody = bulletPrefabInstance.GetComponent<Rigidbody>();
        bulletPrefabRigidbody.AddForce(Camera.main.transform.forward * 100f, ForceMode.Impulse);
    }
}
