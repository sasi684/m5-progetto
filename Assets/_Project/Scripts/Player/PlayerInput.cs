using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    private bool _moveToPoint;
    private bool _cameraRotateLeft;
    private bool _cameraRotateRight;
    private float _zoom;

    public bool MoveToPoint { get { return _moveToPoint; } }
    public bool CameraRotateLeft { get { return _cameraRotateLeft; } }
    public bool CameraRotateRight { get { return _cameraRotateRight; } }
    public float Zoom { get { return _zoom; } }

    private void Update()
    {
        _moveToPoint = Input.GetMouseButtonDown(2);

        _cameraRotateLeft = Input.GetMouseButton(0);
        _cameraRotateRight = Input.GetMouseButton(1);

        _zoom = Input.GetAxis("Mouse ScrollWheel");
    }
}
