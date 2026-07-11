using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    private Rigidbody rb;

    private Vector3 _moveDirection;
    private bool isJumpPressed;
    private float coyoteTimer;
    private float jumpBufferTimer;
    private bool jumpRequested;
    private Vector3 lastPlatformPosition;
    private MovingPlatform currentPlatform;

    [Header("Move settings")]
    [SerializeField] private float speed;
    [SerializeField] private float acceleration = 10f;

    [Header("Jump settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    [SerializeField, Range(0f, 1f)] private float coyoteTime = 0.2f;
    [SerializeField, Range(0f, 1f)] private float jumpBufferTime = 0.2f;

    [Header("Ground layer settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.2f;

    [Header("Camera settings")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Audio")]
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioSource audioSource;

    public bool IsGrounded => isGrounded;
    public Vector3 Velocity => rb.velocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        CheckGround();
        ApplyPlatformMovement();
        Move();
        RotateCharacter();
        UpdateTimers();
        HandleJump();
        BetterJump();
    }

    public void SetMoveDirection(Vector2 input)
    {
        if (cameraTransform == null)
        {
            Debug.LogWarning("CameraTransform no asignado");
            _moveDirection = new Vector3(input.x, 0f, input.y).normalized;
            return;
        }

        float cameraY = cameraTransform.eulerAngles.y;

        Vector3 inputDir = new Vector3(input.x, 0f, input.y);

        Quaternion rotation = Quaternion.Euler(0f, cameraY, 0f);

        _moveDirection = rotation * inputDir;
        _moveDirection.Normalize();
    }

    private void Move()
    {
        Vector3 velocity = rb.velocity;

        Vector3 target = _moveDirection * speed;

        velocity.x = Mathf.Lerp(velocity.x, target.x, acceleration * Time.fixedDeltaTime);
        velocity.z = Mathf.Lerp(velocity.z, target.z, acceleration * Time.fixedDeltaTime);

        rb.velocity = velocity;
    }

    private void ApplyPlatformMovement()
    {
        if (currentPlatform == null) return;

        Vector3 delta = currentPlatform.transform.position - lastPlatformPosition;

        rb.position += delta;

        lastPlatformPosition = currentPlatform.transform.position;
    }

    public void SetJumpPressed(bool pressed)
    {
        isJumpPressed = pressed;
    }

    public void RequestJump()
    {
        jumpRequested = true;
        jumpBufferTimer = jumpBufferTime;
    }

    private void BetterJump()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !isJumpPressed)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void HandleJump()
    {
        if (jumpBufferTimer > 0 && coyoteTimer > 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            audioSource.PlayOneShot(jumpClip);

            jumpBufferTimer = 0;
            coyoteTimer = 0;
            jumpRequested = false;
        }
    }

    private void UpdateTimers()
    {
        // Coyote time
        if (isGrounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.fixedDeltaTime;

        // Jump buffer
        if (jumpRequested)
            jumpBufferTimer -= Time.fixedDeltaTime;
    }

    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
    }

    private void RotateCharacter()
    {
        if (_moveDirection.sqrMagnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(_moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var platform = collision.collider.GetComponent<MovingPlatform>();

        if (platform != null)
        {
            currentPlatform = platform;
            lastPlatformPosition = platform.transform.position;
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.GetComponent<MovingPlatform>() == currentPlatform)
            currentPlatform = null;
    }
}
