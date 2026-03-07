using UnityEngine;

public class FieldOfView : MonoBehaviour
{

    [SerializeField] private float _viewDistance;
    [SerializeField] private float _viewAngle;
    [SerializeField] private Transform _viewStart;
    [SerializeField] private LayerMask _whatIsObstacle;

    private Transform _player;

    public float ViewDistance { get { return _viewDistance; } }
    public float ViewAngle { get { return _viewAngle; } }
    public Transform Player { get { return _player; } }

    private void Awake()
    {
        _player = FindAnyObjectByType<PlayerInput>().GetComponent<Transform>();
    }

    public bool IsPlayerInSight()
    {
        Vector3 toPlayer = _player.position - transform.position;
        float distance = toPlayer.magnitude;

        if (distance > _viewDistance)
            return false;

        toPlayer /= distance;
        if (Vector3.Dot(transform.forward, toPlayer) < Mathf.Cos(_viewAngle * Mathf.Deg2Rad))
            return false;

        if (Physics.Linecast(_viewStart.position, _player.position, _whatIsObstacle))
            return false;

        return true;
    }

}
