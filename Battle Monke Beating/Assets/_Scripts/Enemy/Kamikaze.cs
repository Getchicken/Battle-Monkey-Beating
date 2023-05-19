using UnityEngine;
using UnityEngine.AI;

public class Kamikaze : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float _sightRange;
    [SerializeField] private bool _playerInSightRange;
    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;

    [Header("Patroling")] 
    [SerializeField] private Vector3 walkPoint;
    private bool walkPointSet;
    [SerializeField] private float walkPointRange;

    [Header("References")]
    [SerializeField] NavMeshAgent _navAgent;
    [SerializeField] Transform _player;
    [SerializeField] GameObject _explosionEffect;
    PlayerStats _playerStats;
    Explosion _explosion;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerStats = _player.GetComponent<PlayerStats>();
        _navAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check if _player is in sight
        _playerInSightRange = Physics.CheckSphere(transform.position, _sightRange, whatIsPlayer);

        if (!_playerInSightRange)
        {
            Patroling();
        }

        if (_playerInSightRange)
        {
            ChasePlayer();
        }
    }

    private void Patroling()
    {
        // if no walkPoint then search one
        if (walkPointSet == false)
        {
            SearchWalkPoint();
        }

        //if there is one go there

        if (walkPointSet)
        {
            _navAgent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // walkPoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        // calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // is the walkpoint reachable or is there nothing?
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        _navAgent.SetDestination(_player.position);
        FaceTowards();
    }

    private void FaceTowards()
    {
        Vector3 lookVector = _player.transform.position - transform.position;
        lookVector.y = transform.position.y;
        Quaternion rot = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Explode();
        }
    }

    void Explode()
    {
        // Damage, screenshake?
        _playerStats.TakeDamage(80f);

        _explosion = GetComponent<Explosion>();
        _explosion.Explode();
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 position = transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(position, _sightRange);
    }
}
