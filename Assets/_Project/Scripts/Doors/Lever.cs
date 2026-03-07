using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{

    [SerializeField] private UnityEvent _onLeverActivate;
    [SerializeField] private UnityEvent<bool> _onPlayerInRange;

    private Animator _animator;
    private bool _isPlayerInRange;
    private bool _isPulled;

    private PlayerInput _input;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _input = FindAnyObjectByType<PlayerInput>();
    }

    private void Update()
    {
        if (_isPlayerInRange && _input.ActivateLever && !_isPulled) // If player is in range and player is pressing E and the lever hasn't been pulled
        {
            _animator.SetTrigger("Activate"); // Start the pulling animation
            _onLeverActivate?.Invoke(); // Invoke all methods for this event (OpenDoor in Door.cs script)
            _isPulled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isPulled) // If the player enters the trigger zone and the lever hasn't been pulled yet
        {
            _isPlayerInRange = true; // Set this bool to true
            _onPlayerInRange?.Invoke(_isPlayerInRange); // Invoke all methods for this event (Show UI to interact with lever)
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // If player exits the trigger zone
        {
            _isPlayerInRange = false; // Set this bool to false
            _onPlayerInRange?.Invoke(_isPlayerInRange); // Invoke all methods for this event (Hide UI to interact with lever)
        }
    }

}
