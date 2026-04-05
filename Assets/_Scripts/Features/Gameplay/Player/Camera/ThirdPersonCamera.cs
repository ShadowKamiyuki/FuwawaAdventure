using UnityEngine;

[System.Obsolete]
public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Distance")]
    [SerializeField] private float distance = 5f;
    [SerializeField] private float height = 2f;

    [Header("Rotation")]
    [SerializeField] private float mouseSensitivity = 3f;
    [SerializeField] private float minY = -30f;
    [SerializeField] private float maxY = 60f;

    private float _yaw;
    private float _pitch;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        _yaw = angles.y;
        _pitch = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        Rotate();
        Follow();
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        _yaw += mouseX;
        _pitch -= mouseY;
        _pitch = Mathf.Clamp(_pitch, minY, maxY);
    }

    private void Follow()
    {
        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);

        Vector3 offset = rotation * new Vector3(0, 0, -distance);
        Vector3 targetPosition = target.position + Vector3.up * height;

        transform.position = targetPosition + offset;
        transform.LookAt(targetPosition);
    }
}
