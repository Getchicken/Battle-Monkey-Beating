using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BananaGun : MonoBehaviour
{
    public float damage = 100f;
    public float range = 10000f;
    public float fireRate = 15f;
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
            anim.Play("Reload");
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
        GameObject Flash = Instantiate(muzzleFlash, bulletSpawnPoint);
        Destroy(Flash, 1.3f);
        pistolShot_fx.Play();

        RaycastHit hit;
        currentAmmo--;
        _uiManager.UpdateAmmo(currentAmmo);

        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, range, Enemy))
        {
            Target target = hit.transform.GetComponent<Target>();

            // Damage????
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
            Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }
}
