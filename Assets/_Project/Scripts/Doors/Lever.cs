using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{

    [SerializeField] private UnityEvent _onLeverActivate;

    private Animator _animator;
    private bool _isPlayerInRange;

    private PlayerInput _input;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _input = FindAnyObjectByType<PlayerInput>();
    }

    private void Update()
    {
        if (_isPlayerInRange && _input.ActivateLever)
        {
            Debug.Log("OK");
            _animator.SetTrigger("Activate");
            _onLeverActivate?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = false;
        }
    }

}
