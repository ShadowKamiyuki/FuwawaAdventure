using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    private IInputService _input;

    [SerializeField] private Transform target;

    [Header("Distance")]
    [SerializeField] private float distance = 5f;
    [SerializeField] private float height = 2f;

    [Header("Rotation")]
    [SerializeField] private float sensitivity = 3f;
    [SerializeField] private float minY = -30f;
    [SerializeField] private float maxY = 60f;

    [Header("Smoothing")]
    [SerializeField] private float smoothTime = 0.05f;

    private float _yaw;
    private float _pitch;

    void Awake()
    {
        _input = ServiceLocator.Get<IInputService>();
    }

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        _yaw = angles.y;
        _pitch = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        if (target == null || _input == null) return;

        Rotate();
        Follow();
    }

    private void Rotate()
    {
        Vector2 look = _input.GetSmoothLook(smoothTime);

        _yaw += look.x * sensitivity;
        _pitch -= look.y * sensitivity;
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