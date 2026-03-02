using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PatrolEnemy : Enemy
{
    private WaypointsManager _waypointsManager;
    private Transform[] _waypoints;
    private int _index = 0;

    private void Awake()
    {
        _waypointsManager = FindAnyObjectByType<WaypointsManager>();

        _agent = GetComponent<NavMeshAgent>();

        _fieldOfView = GetComponent<FieldOfView>();

        _lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    private void Start()
    {
        _currentState = ENEMY_STATE.IDLE;

        _waypoints = _waypointsManager.GetPath(_agentId);
        if(_waypoints != null)
            _agent.SetDestination(_waypoints[_index].position);
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

        if (_waypoints == null || _isWaiting)
            return;

        if(!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            _waitingCoroutine = StartCoroutine(WaitingCoroutine());
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
        if(_chasingCoroutine != null)
        {
            StopCoroutine(_chasingCoroutine);
            _chasingCoroutine = null;
        }

        _agent.SetDestination(_waypoints[_index].position);

        _lineRenderer.startColor = Color.green;
        _lineRenderer.endColor = Color.green;
    }
    protected override void OnExitPatrol() { }

    protected override void OnEnterChase() 
    {
        if(_agent.isStopped)
            _agent.isStopped = false;

        if(_waitingCoroutine != null)
        {
            StopCoroutine(_waitingCoroutine);
            _waitingCoroutine = null;
            _isWaiting = false;
        }

        _lineRenderer.startColor = Color.red;
        _lineRenderer.endColor = Color.red;
    }
    protected override void OnExitChase() { }
    #endregion

    private IEnumerator WaitingCoroutine()
    {
        _isWaiting = true;
        _agent.isStopped = true;
        yield return new WaitForSeconds(_waitTime);

        _index = (_index + 1) % _waypoints.Length;
        _agent.SetDestination(_waypoints[_index].position);

        _agent.isStopped = false;
        _isWaiting = false;
        _waitingCoroutine = null;
    }

    private IEnumerator ChasingCoroutine()
    {
        while (true)
        {
            _agent.SetDestination(_fieldOfView.Player.position);
            yield return new WaitForSeconds(_chaseUpdateTime);
        }
    }
}
