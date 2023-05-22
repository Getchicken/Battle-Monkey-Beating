using UnityEngine;
using UnityEngine.AI;

public class Hunter : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float _sightRange, _attackRange;
    [SerializeField] private bool _playerInSightRange, _playerInAttackRange;
    [SerializeField] private LayerMask whatIsGround, whatIsPlayer;

    [Header("Patroling")]
    [SerializeField] private Vector3 walkPoint;
    private bool walkPointSet;
    [SerializeField] private float _walkPointRange;
    [SerializeField] private float _minWalkPointRange;

    [Header("References")]
    [SerializeField] NavMeshAgent _navAgent;
    [SerializeField] Transform _player;
    [SerializeField] Animator anim;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _navAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //Check if _player is in sight
        _playerInSightRange = Physics.CheckSphere(transform.position, _sightRange, whatIsPlayer);
        _playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, whatIsPlayer);

        if (!_playerInSightRange && !_playerInAttackRange)
        {
            Patroling();
        }

        if (_playerInSightRange && !_playerInAttackRange)
        {
            ChasePlayer();
        }
        if(_playerInSightRange && _playerInAttackRange)
        {
            Attack();
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
        float randomZ = Random.Range(-_walkPointRange + _minWalkPointRange, _walkPointRange);
        float randomX = Random.Range(-_walkPointRange + _minWalkPointRange, _walkPointRange);

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

    private void Attack()
    {
        // make enemy stop moving
        _navAgent.SetDestination(transform.position);
        // Attacks are handled by EnemyBaseBehaviour script
    }

    private void FaceTowards()
    {
        Vector3 lookVector = _player.transform.position - transform.position;
        lookVector.y = transform.position.y;
        Quaternion rot = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1);
    }
}
