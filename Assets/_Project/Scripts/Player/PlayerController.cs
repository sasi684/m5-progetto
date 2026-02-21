using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    
    private PlayerInput _input;
    private NavMeshAgent _agent;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (_input.MoveToPoint)
        {
            if (Physics.Raycast(_input.MousePosition, out RaycastHit hit))
            {
                _agent.SetDestination(hit.point);
            }
        }
    }

}
