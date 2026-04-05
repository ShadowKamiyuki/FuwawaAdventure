using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    private Rigidbody rb;

    private Vector3 _moveDirection;
    private bool isJumpPressed;
    private float coyoteTimer;
    private float jumpBufferTimer;
    private bool jumpRequested;

    [Header("Move settings")]
    [SerializeField] private float speed;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 15f;

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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        CheckGround();
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
            _moveDirection = new Vector3(input.x, 0f, input.y).normalized;
            return;
        }

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * input.y + right * input.x;

        _moveDirection = move.normalized;
    }

    private void Move()
    {
        Vector3 velocity = rb.velocity;

        Vector3 target = _moveDirection * speed;

        velocity.x = Mathf.Lerp(velocity.x, target.x, acceleration * Time.fixedDeltaTime);
        velocity.z = Mathf.Lerp(velocity.z, target.z, acceleration * Time.fixedDeltaTime);

        rb.velocity = velocity;
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

    public void Interact()
    {
        //_currentInteractable?.Interact(this);
    }

    public void Attack()
    {
        // agregar el ataque
        Debug.Log("Attack");
    }

    private void OnTriggerEnter(Collider other)
    {
        //_currentInteractable = other.GetComponent<IInteractable>();
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.GetComponent<IInteractable>() == _currentInteractable)
        //    _currentInteractable = null;
    }
}
