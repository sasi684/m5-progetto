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
        if (_isPlayerInRange && _input.ActivateLever && !_isPulled)
        {
            _animator.SetTrigger("Activate");
            _onLeverActivate?.Invoke();
            _isPulled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isPulled)
        {
            _isPlayerInRange = true;
            _onPlayerInRange?.Invoke(_isPlayerInRange);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = false;
            _onPlayerInRange?.Invoke(_isPlayerInRange);
        }
    }

}
