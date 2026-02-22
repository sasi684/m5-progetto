using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _cameraRotationSpeed = 80f;

    [SerializeField] private float _zoomIncrease = 50f;
    [SerializeField] private float _minZoomDistance = 10f;
    [SerializeField] private float _maxZoomDistance = 45f;

    private PlayerInput _input;
    private CinemachineVirtualCamera _cinemachine;

    private Transform _target;

    private void Awake()
    {
        _input = FindAnyObjectByType<PlayerInput>();
        _cinemachine = GetComponent<CinemachineVirtualCamera>();

        _target = _cinemachine.Follow;
    }

    private void Update()
    {
        CameraRotation();

        if (_input.Zoom != 0f)
            CameraZoom();
    }

    private void CameraRotation()
    {
        if (_input.CameraRotateLeft)
            transform.RotateAround(_target.position, Vector3.up, -_cameraRotationSpeed * Time.deltaTime);

        if (_input.CameraRotateRight)
            transform.RotateAround(_target.position, Vector3.up, _cameraRotationSpeed * Time.deltaTime);
    }

    private void CameraZoom()
    {
        var transposer = _cinemachine.GetCinemachineComponent<CinemachineFramingTransposer>();

        float offset = transposer.m_CameraDistance;
        offset -= _input.Zoom * _zoomIncrease;
        offset = Mathf.Clamp(offset, _minZoomDistance, _maxZoomDistance);
        transposer.m_CameraDistance = offset;
    }

}
