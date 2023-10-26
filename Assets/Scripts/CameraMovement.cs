using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Transform _target;
    [SerializeField] private float _smothing = 1f;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        var nextPosition = Vector3.Lerp(_camera.transform.position, _target.transform.position + _offset, Time.fixedDeltaTime * _smothing);

        _camera.transform.position = nextPosition;
    }
}