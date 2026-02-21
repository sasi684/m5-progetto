using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    private Ray _mousePosition;
    private bool _moveToPoint;
    private bool _cameraRotateLeft;
    private bool _cameraRotateRight;

    private Camera _mainCamera;

    public Ray MousePosition { get { return _mousePosition; } }
    public bool MoveToPoint { get { return _moveToPoint; } }
    public bool CameraRotateLeft { get { return _cameraRotateLeft; } }
    public bool CameraRotateRight { get { return _cameraRotateRight; } }

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        _moveToPoint = Input.GetMouseButtonDown(2);

        if (_moveToPoint) _mousePosition = _mainCamera.ScreenPointToRay(Input.mousePosition);

        _cameraRotateLeft = Input.GetMouseButton(0);
        _cameraRotateRight = Input.GetMouseButton(1);
    }
}
