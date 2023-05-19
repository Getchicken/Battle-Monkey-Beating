using System.Collections;
using UnityEngine;

public class EnemyBaseBehaviour : EnemyAttackStats
{
    [Header("Ammo Stats")]
    public int _maxAmmo = 10;
    public float _reloadTime = 2f;
    private int _currentAmmo;
    private bool _reloading = false;

    [Header("Shake")]
    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private float shakeStrength = 0.05f;

    [Header("References")]
    public GameObject _attackEffect;
    public GameObject _impactEffect;
    public Transform _player;
    public Transform _bulletSpawnPoint;
    public LayerMask whatIsPlayer;

    [SerializeField] private State state = State.Wait;
    private enum State
    {
        Wait,
        Attack,
        Reloading
    }
    void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _currentAmmo = _maxAmmo;
    }

    void Update()
    {
        StateMachine();
    }
    
    void StateMachine()
    {
        switch (state)
        {
            case State.Wait:
                CheckSurroundings();
                break;
            case State.Attack:
                Attack();
                break;
            case State.Reloading:
                if (!_reloading)
                    StartCoroutine(Reload());
                break;
        }
    }
    
    void CheckSurroundings()
    {
        _playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, whatIsPlayer);

        if (_playerInAttackRange)
        {
            state = State.Attack;
        }
        else
        {
            state = State.Wait;
        }
    }

    void Attack()
    {
        if (_currentAmmo > 0 && Time.time >= _nextTimeToFire && !_reloading)
        {
            // Decrease ammo and set the fire rate
            _nextTimeToFire = Time.time + 1f / _fireRate;
            _currentAmmo--;

            // look at the _player
            FaceTowards(_player);

            // Calculate direction -> Spawn -> Shoot particle
            RaycastHit hit;
            Vector3 offset = Random.insideUnitSphere * 0.5f;
            Vector3 direction = ((_player.position + offset) - _bulletSpawnPoint.position).normalized;
            //direction += _patrolRadius.normalized;
            Physics.Raycast(_bulletSpawnPoint.position, direction, out hit, _attackRange, whatIsPlayer);
            Debug.DrawLine(transform.position, hit.point, Color.green);

            Instantiate(_attackEffect, _bulletSpawnPoint.position, Quaternion.identity);
            Instantiate(_impactEffect, hit.point, Quaternion.identity);

            // Get PlayerStats of hit -> check for null -> TakeDamage
            if (hit.collider != null)
            {
                PlayerStats playerStats = hit.collider.GetComponentInParent<PlayerStats>();
                if(playerStats != null)
                {
                    playerStats?.TakeDamage(_attackDamage);
                    CamShaker.DoOnShake(shakeDuration, shakeStrength);
                }
            }
        }
        else if (_currentAmmo <= 0)
        {
            state = State.Reloading;
        }
        else
        {
            state = State.Wait;
        }
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(_reloadTime);
        _currentAmmo = _maxAmmo;
        _reloading = false;
        state = State.Wait;
    }

    private void FaceTowards(Transform target)
    {
        Vector3 lookVector = target.transform.position - transform.position;
        lookVector.y = transform.position.y;
        Quaternion rotate = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotate, 1);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 position = transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(position, _attackRange);
    }
}

