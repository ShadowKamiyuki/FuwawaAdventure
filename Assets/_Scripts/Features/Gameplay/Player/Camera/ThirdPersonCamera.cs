using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Header("Distance")]
    [SerializeField] private float distance = 5f;
    [SerializeField] private float height = 2f;

    [Header("Rotation")]
    [SerializeField] private float sensitivity = 0.15f;
    [SerializeField] private float minY = -30f;
    [SerializeField] private float maxY = 60f;

    private float _yaw;
    private float _pitch;

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        _yaw = angles.y;
        _pitch = angles.x;
    }

    private void LateUpdate()
    {
        Follow();
    }

    public void AddRotation(Vector2 delta)
    {
        _yaw += delta.x * sensitivity;
        _pitch -= delta.y * sensitivity;
        _pitch = Mathf.Clamp(_pitch, minY, maxY);
    }

    private void Follow()
    {
        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);

        Vector3 offset = rotation * new Vector3(0, 0, -distance);
        Vector3 targetPos = target.position + Vector3.up * height;

        transform.position = targetPos + offset;
        transform.LookAt(targetPos);
    }
}