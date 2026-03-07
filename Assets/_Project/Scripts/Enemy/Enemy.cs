using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public enum ENEMY_STATE { IDLE, PATROL, CHASE } // All possible states for enemies

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected ENEMY_STATE _currentState;
    [SerializeField] protected float _chaseUpdateTime; // Time to update the path in chase state
    [SerializeField] protected float _waitTime; // Time to set next destination in Patrol state
    [SerializeField] protected float _chaseToPatrolTime; // Time to go back to Patrol state from Chase

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

    // Generic on enter state method
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

    // Generic on exit state method
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

    // Generic change state method
    protected void ChangeState(ENEMY_STATE newState)
    {
        OnExitState(_currentState);
        _currentState = newState;
        OnEnterState(newState);
    }

    // If the enemy touches the player, then transition to the game over scene
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            ScreenFader.Instance.StartFadeToOpaque(GameOver);
        }
    }

    private void GameOver()
    {
        SceneManager.LoadScene("GameOver");
        ScreenFader.Instance.StartFadeToTransparent();
    }

}
