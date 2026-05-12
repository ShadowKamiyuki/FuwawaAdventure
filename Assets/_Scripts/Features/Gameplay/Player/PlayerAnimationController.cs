using UnityEngine;

public class PlayerAnimationController : MonoBehaviour, IUpdatable
{
    [SerializeField] private Animator animator;
    private Rigidbody rb;
    private PlayerActions playerActions;

    [Header("Settings")]
    [SerializeField] private float speedMultiplier = 1f;

    private void Awake()
    {
        ServiceLocator.Get<IUpdateService>().Register(this);
        rb = GetComponent<Rigidbody>();
        playerActions = GetComponent<PlayerActions>();
    }

    public void Tick(float deltaTime)
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

    private void OnDestroy()
    {
        IUpdateService updateManager = ServiceLocator.Get<IUpdateService>();
        if (updateManager != null)
        {
            updateManager.Unregister(this);
        }
    }
}
