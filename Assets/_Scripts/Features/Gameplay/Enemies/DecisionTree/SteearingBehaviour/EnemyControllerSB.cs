using UnityEngine;

public enum Mode
{
    Seek,
    Flee,
    Arrive,
    Pursue,
    Evade,
    Wander
}

public class EnemyControllerSB : MonoBehaviour
{
    private Rigidbody rb;
    private LineOfSight los;
    private EnemyContext enemyContext;
    private EnemySBDecisionTree decisionTree;

    private Vector3 wanderDirection;
    private float wanderTime;

    [Header("Enemy settings")]
    [SerializeField] private Mode mode;
    [SerializeField] private float wanderChangeInterval;
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField, Range(0f, 5f)] private float arriveDistance;

    [Header("Line of sight settings")]
    [SerializeField] private Transform target;
    [SerializeField] private float distance;
    [SerializeField] private float angle;
    [SerializeField] private LayerMask obstacles;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        decisionTree = GetComponent<EnemySBDecisionTree>();
        los = new LineOfSight();
        wanderDirection = transform.forward;
        wanderTime = 0f;

        enemyContext = new EnemyContext { self = transform, target = target, los = los, distance = distance, angle = angle, obstacles = obstacles, attackRange = arriveDistance };
    }

    private void Update()
    {
        decisionTree.Evaluate(this, enemyContext);
    }

    private void FixedUpdate()
    {
        Vector3 dir = Vector3.zero;

        switch (mode)
        {
            case Mode.Seek:
                dir = SteearingBehaviours.Seek(transform, target.position);
                break;
            case Mode.Flee:
                dir = SteearingBehaviours.Flee(transform, target.position);
                break;
            case Mode.Arrive:
                dir = SteearingBehaviours.Arrive(transform, target.position, arriveDistance);
                break;
            case Mode.Pursue:
                dir = SteearingBehaviours.Pursue(transform, target, rb, 5f);
                break;
            case Mode.Evade:
                dir = SteearingBehaviours.Evade(transform, target, rb, 5f);
                break;
            case Mode.Wander:
                wanderTime -= Time.deltaTime;

                if (wanderTime <= 0f)
                {
                    wanderDirection = SteearingBehaviours.Wander(wanderDirection, 180f);
                    wanderTime = wanderChangeInterval;
                }
                dir = wanderDirection;
                break;
            default:
                break;
        }

        Move(dir);
    }

    private void Move(Vector3 dir)
    {
        rb.linearVelocity = dir.normalized * speed;

        if (dir != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, dir, rotationSpeed * Time.deltaTime);
        }
    }

    public void ChangeMode(Mode newMode)
    {
        mode = newMode;
    }
}
