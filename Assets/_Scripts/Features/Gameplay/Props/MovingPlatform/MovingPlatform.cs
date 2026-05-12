using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float moveSpeed;

    private Rigidbody rb;
    private Vector3 target;
    private Vector3 lastPosition;
    public Vector3 PlatformVelocity { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        target = pointB.position;
    }

    private void FixedUpdate()
    {
        Vector3 newPos = Vector3.MoveTowards(rb.position, target, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        PlatformVelocity = (newPos - lastPosition) / Time.fixedDeltaTime;
        lastPosition = newPos;

        if (Vector3.Distance(rb.position, target) < 0.1f)
        {
            target = target == pointA.position ? pointB.position : pointA.position;
        }
    }
}
