using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private Rigidbody rb;
    private PlayerActions playerActions;

    [Header("Settings")]
    [SerializeField] private float speedMultiplier = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerActions = GetComponent<PlayerActions>();
    }

    private void Update()
    {
        UpdateAnimations();
    }

    private void UpdateAnimations()
    {
        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0f;

        float speed = horizontalVelocity.magnitude;

        animator.SetFloat("Speed", speed * speedMultiplier);
    }
}
