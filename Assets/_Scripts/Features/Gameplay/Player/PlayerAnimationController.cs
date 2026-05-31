using UnityEngine;

public class PlayerAnimationController : MonoBehaviour, IUpdatable
{
    [SerializeField] private Animator animator;
    private Rigidbody rb;
    private PlayerActions playerActions;

    [Header("Settings")]
    [SerializeField] private float speedMultiplier = 1f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private bool isGrounded;

    private void Awake()
    {
        ServiceLocator.Get<IUpdateService>().Register(this);

        rb = GetComponent<Rigidbody>();
        playerActions = GetComponent<PlayerActions>();
    }

    public void Tick(float deltaTime)
    {
        UpdateGroundCheck();
        UpdateAnimations();
    }

    private void UpdateGroundCheck()
    {
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundDistance,
            groundLayer
        );
    }

    private void UpdateAnimations()
    {
        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0f;

        float speed = horizontalVelocity.magnitude;

        animator.SetFloat("Speed", speed * speedMultiplier);

        animator.SetBool("IsGrounded", isGrounded);

        animator.SetFloat("VerticalVelocity", rb.velocity.y);
    }

    private void OnDestroy()
    {
        IUpdateService updateManager = ServiceLocator.Get<IUpdateService>();

        if (updateManager != null)
        {
            updateManager.Unregister(this);
        }
    }
}