using UnityEngine;

public class Drone : MonoBehaviour
{
    [Header("Variables")]
    public float _moveSpeed;
    private bool _playerInAttackRange = false;
    private float _attackRange = 10f;
    [SerializeField] private float _lowerRandom;
    [SerializeField] private float _upperRandom;

    [Header("References")]
    [SerializeField] private Transform[] _patrolArea;

    [SerializeField] private Vector3 _patrolRadius;
    public LayerMask whatIsPlayer;

    [SerializeField] private State state = State.Patrol;
    private enum State
    {
        Patrol,
        Attack
    }
    private Vector3 _targetPosition;

    void Start()
    {
        // set the first destination
        Transform randomTransform = _patrolArea[Random.Range(0, _patrolArea.Length)];
        _patrolRadius = new Vector3(Random.Range(_lowerRandom, _upperRandom), 0, Random.Range(_lowerRandom, _upperRandom));
        _targetPosition = randomTransform.position + _patrolRadius;
    }

    void Update()
    {
        StateMachine();
        CheckSourroundings();
    }
    void StateMachine()
    {
        switch (state)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Attack:
                DontMove();
                break;
        }
    }

    void CheckSourroundings()
    {
        _playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, whatIsPlayer);
        // if _player is in range -> DontMove
        if (_playerInAttackRange)
        {
            state = State.Attack;
        }
        else state = State.Patrol;
    }
    void Patrol()
    {
        // patrol the area -> In the area around A to B to C

        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _moveSpeed * Time.deltaTime);

        // If we have reached the target position, set the next target position
        if (Vector3.Distance(transform.position, _targetPosition) < 0.1f)
        {
            Transform randomTransform = _patrolArea[Random.Range(0, _patrolArea.Length)];
            _patrolRadius = new Vector3(Random.Range(_lowerRandom, _upperRandom), 0, Random.Range(_lowerRandom, _upperRandom));
            _targetPosition = randomTransform.position + _patrolRadius;
        }
    }

    void DontMove()
    {
        // stop moving
        transform.position = Vector3.MoveTowards(transform.position, transform.position, _moveSpeed * Time.deltaTime);
    }
}
