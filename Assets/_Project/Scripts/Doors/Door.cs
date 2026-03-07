using UnityEngine;

public class Door : MonoBehaviour
{

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OpenDoor() // This method is called by an event
    {
        _animator.SetTrigger("Open"); // Start the door opening animation
    }

}
