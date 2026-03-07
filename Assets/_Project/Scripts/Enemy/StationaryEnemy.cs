using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class StationaryEnemy : Enemy
{
    [SerializeField] private float _rotationAngle;

    private Vector3 _startPosition;
    private Coroutine _rotateCoroutine;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        _fieldOfView = GetComponent<FieldOfView>();

        _lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    private void Start()
    {
        _startPosition = transform.position;
        _currentState = ENEMY_STATE.IDLE;
    }

    private void Update()
    {
        switch (_currentState)
        {
            case ENEMY_STATE.IDLE:
                IdleUpdate();
                break;
            case ENEMY_STATE.PATROL:
                PatrolUpdate();
                break;
            case ENEMY_STATE.CHASE:
                ChaseUpdate();
                break;
        }
    }

    #region States Updates
    protected override void IdleUpdate()
    {
        ChangeState(ENEMY_STATE.PATROL);
    }

    protected override void PatrolUpdate()
    {
        if (_fieldOfView.IsPlayerInSight())
        {
            _lastPlayerInSight = Time.time;
            ChangeState(ENEMY_STATE.CHASE);
            return;
        }

        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance && _rotateCoroutine == null)
            _rotateCoroutine = StartCoroutine(RotateCoroutine());
    }

    protected override void ChaseUpdate()
    {
        if (Time.time - _lastPlayerInSight > _chaseToPatrolTime)
        {
            ChangeState(ENEMY_STATE.PATROL);
            return;
        }

        if (_fieldOfView.IsPlayerInSight())
            _lastPlayerInSight = Time.time;

        if (_chasingCoroutine == null)
            _chasingCoroutine = StartCoroutine(ChasingCoroutine());
    }
    #endregion

    #region OnEnter/OnExit States
    protected override void OnEnterIdle() { }
    protected override void OnExitIdle() { }

    protected override void OnEnterPatrol()
    {
        if (_chasingCoroutine != null)
        {
            StopCoroutine(_chasingCoroutine);
            _chasingCoroutine = null;
        }

        _agent.SetDestination(_startPosition);

        _lineRenderer.startColor = Color.green;
        _lineRenderer.endColor = Color.green;
    }
    protected override void OnExitPatrol() { }

    protected override void OnEnterChase()
    {
        if(_rotateCoroutine != null)
        {
            StopCoroutine(_rotateCoroutine);
            _rotateCoroutine = null;
        }

        _lineRenderer.startColor = Color.red;
        _lineRenderer.endColor = Color.red;
    }
    protected override void OnExitChase() { }
    #endregion

    private IEnumerator RotateCoroutine()
    {
        while (true)
        {
            Vector3 deltaAngle = Vector3.up * Time.deltaTime * _rotationAngle;
            transform.Rotate(deltaAngle);
            yield return null;
        }
    }

    private IEnumerator ChasingCoroutine()
    {
        var waitTime = new WaitForSeconds(_chaseUpdateTime);
        while (true)
        {
            _agent.SetDestination(_fieldOfView.Player.position);
            yield return waitTime;
        }
    }
}
