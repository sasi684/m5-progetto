using System.Collections;
using UnityEngine;

public class LineRendererHandler : MonoBehaviour
{

    [SerializeField] private int _subdivisions;
    [SerializeField] private LayerMask _whatIsObstacle;
    [SerializeField] private float _interval;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private FieldOfView _fieldOfView;

    private void OnEnable()
    {
        StartCoroutine(EvaluateFieldOfView());
    }

    private IEnumerator EvaluateFieldOfView()
    {
        while (true)
        {
            yield return new WaitForSeconds(_interval);

            ElaborateFieldOfView(_subdivisions);
        }
    }

    private void ElaborateFieldOfView(int subdivisions)
    {
        _lineRenderer.positionCount = subdivisions + 1;

        float startAngle = -_fieldOfView.ViewAngle;

        Vector3 lineOrigin = transform.position;
        Vector3 raycastOrigin = transform.position;
        Vector3 forward = transform.forward;

        _lineRenderer.SetPosition(0, lineOrigin);

        float deltaAngle = (2 * _fieldOfView.ViewAngle / subdivisions);

        for (int i = 0; i < subdivisions; i++)
        {
            float currentAngle = startAngle + deltaAngle * i;
            Vector3 lineDirection = Quaternion.Euler(0, currentAngle, 0) * forward;
            Vector3 point = transform.position + lineDirection * _fieldOfView.ViewDistance;

            if (Physics.Raycast(raycastOrigin, lineDirection, out RaycastHit hit, _fieldOfView.ViewDistance, _whatIsObstacle))
            {
                point = hit.point;
            }

            _lineRenderer.SetPosition(i + 1, point);
        }
    }

}
