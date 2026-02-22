using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _maxTravelDistance = 6f;
    
    private Camera _camera;

    private PlayerInput _input;
    private NavMeshAgent _agent;

    private Coroutine _pathCoroutine;

    private void Awake()
    {
        _camera = Camera.main;

        _input = GetComponent<PlayerInput>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (_input.MoveToPoint)
        {
            Ray mousePosition = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(mousePosition, out RaycastHit hit))
            {
                if (_pathCoroutine != null)
                    StopCoroutine(_pathCoroutine);

                _pathCoroutine = StartCoroutine(CalculatePlayerPath(hit));
            }
        }
    }

    private IEnumerator CalculatePlayerPath(RaycastHit destination)
    {
        _agent.SetDestination(destination.point);

        while (_agent.pathPending)
        {
            yield return null;
        }

        CheckDestinationDistance();
    }

    private void CheckDestinationDistance()
    {
        float distance = 0f;
        for (int i = _agent.path.corners.Length - 1; i > 0; i--)
        {
            distance += Vector3.Distance(_agent.path.corners[i], _agent.path.corners[i - 1]);
        }

        if (distance > _maxTravelDistance)
        {
            _agent.ResetPath();
            Debug.LogWarning($"Attenzione! La destinazione e' troppo lontana dal player. Distance: {distance}");
        }
    }
}
