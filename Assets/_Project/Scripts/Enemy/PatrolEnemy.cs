using System.Collections;
using System.Collections.Generic;
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
        // TODO: SE VEDE IL PLAYER, LO INSEGUE (FOV DA IMPLEMENTARE)

        if (_waypoints == null || _isWaiting)
            return;

        if(!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            _waitingCoroutine = StartCoroutine(WaitingCoroutine());
    }

    protected override void ChaseUpdate()
    {
        // TODO: SE IL PLAYER NON SI VEDE, DOPO UN PO' TORNA IN IDLE

        if(Time.time - _lastChaseUpdate > _chaseUpdateTime)
        {
            // TODO: SEGUE IL PLAYER

            _lastChaseUpdate = Time.time;
        }
    }
    #endregion

    #region OnEnter/OnExit States
    protected override void OnEnterIdle() { }
    protected override void OnExitIdle() { }

    protected override void OnEnterPatrol()
    {
        _agent.SetDestination(_waypoints[_index].position);
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
        _lastChaseUpdate = 0f;
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
}
