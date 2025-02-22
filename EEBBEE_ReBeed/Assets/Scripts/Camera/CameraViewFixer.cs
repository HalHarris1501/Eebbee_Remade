using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewFixer : MonoBehaviour
{
    [Header("The Camera")]
    [SerializeField] private Camera _cam;

    [SerializeField] private float _orthographicSize = 5;
    [SerializeField] private float _aspect = 1.33333f;
    //[SerializeField] private List<Collider2D> _objectsToEncapsulate;
    //[SerializeField] private float _buffer;

    void Start()
    {
        Camera.main.projectionMatrix = Matrix4x4.Ortho(
                -_orthographicSize * _aspect, _orthographicSize * _aspect,
                -_orthographicSize, _orthographicSize,
                Camera.main.nearClipPlane, Camera.main.farClipPlane);

        //var (centre, size) = CalculateOrthoSize();
        //_cam.transform.position = centre;
        //_cam.orthographicSize = size;
    }

    //private (Vector3 centre, float size) CalculateOrthoSize()
    //{
    //    var bounds = new Bounds();

    //    foreach (var col in _objectsToEncapsulate) bounds.Encapsulate(col.bounds);

    //    bounds.Expand(_buffer);

    //    var vertical = bounds.size.y;
    //    var horizontal = bounds.size.x * _cam.pixelHeight / _cam.pixelWidth;

    //    var size = Mathf.Max(horizontal, vertical) * 0.5f;
    //    var centre = bounds.center + new Vector3(0, 0, -1);

    //    return (centre, size);
    //}
}
