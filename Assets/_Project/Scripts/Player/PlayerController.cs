using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _maxTravelDistance; // Max distance customizable in the inspector

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
        if (_input.MoveToPoint) // If the player chooses a destination
        {
            Ray mousePosition = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(mousePosition, out RaycastHit hit))
            {
                if (_pathCoroutine != null) // Stop the coroutine if already running
                    StopCoroutine(_pathCoroutine);

                _pathCoroutine = StartCoroutine(CalculatePlayerPath(hit)); // Start the coroutine to calculate the path
            }
        }
    }

    private IEnumerator CalculatePlayerPath(RaycastHit destination)
    {
        _agent.SetDestination(destination.point); // Set the destination

        while (_agent.pathPending) // Wait until the path has been calculated
        {
            yield return null;
        }

        CheckDestinationDistance(); // Check the distance
    }

    private void CheckDestinationDistance()
    {
        float distance = 0f;
        for (int i = _agent.path.corners.Length - 1; i > 0; i--) // Sum the distance between each corner (using remainingDistance was unreliable)
        {
            distance += Vector3.Distance(_agent.path.corners[i], _agent.path.corners[i - 1]);
        }

        if (distance > _maxTravelDistance) // If total distance is bigger than max travel distance, then reset the path
        {
            _agent.ResetPath();
            Debug.LogWarning($"Attenzione! La destinazione e' troppo lontana dal player. Distance: {distance}");
        }
    }
}
