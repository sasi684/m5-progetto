using UnityEngine;
using UnityEngine.AI;

public enum ENEMY_STATE { IDLE, PATROL, CHASE }

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected ENEMY_STATE _currentState;
    [SerializeField] protected float _chaseUpdateTime;
    [SerializeField] protected float _waitTime;
    [SerializeField] protected float _chaseToPatrolTime;

    protected Coroutine _chasingCoroutine;

    protected NavMeshAgent _agent;

    protected FieldOfView _fieldOfView;
    protected float _lastPlayerInSight;
    protected LineRenderer _lineRenderer;

    protected abstract void IdleUpdate();
    protected abstract void PatrolUpdate();
    protected abstract void ChaseUpdate();

    protected abstract void OnEnterIdle();
    protected abstract void OnExitIdle();

    protected abstract void OnEnterPatrol();
    protected abstract void OnExitPatrol();

    protected abstract void OnEnterChase();
    protected abstract void OnExitChase();

    protected void OnEnterState(ENEMY_STATE state)
    {
        switch (state)
        {
            case ENEMY_STATE.IDLE:
                OnEnterIdle();
                break;
            case ENEMY_STATE.PATROL:
                OnEnterPatrol();
                break;
            case ENEMY_STATE.CHASE:
                OnEnterChase();
                break;
        }
    }

    protected void OnExitState(ENEMY_STATE state)
    {
        switch (state)
        {
            case ENEMY_STATE.IDLE:
                OnExitIdle();
                break;
            case ENEMY_STATE.PATROL:
                OnExitPatrol();
                break;
            case ENEMY_STATE.CHASE:
                OnExitChase();
                break;
        }
    }

    protected void ChangeState(ENEMY_STATE newState)
    {
        OnExitState(_currentState);
        _currentState = newState;
        OnEnterState(newState);
    }

}
