using UnityEngine;

namespace Poyo
{
    public class PoyoController : MonoBehaviour
    {
        [Header("Line of sight settings")]
        [SerializeField] private Transform target;
        [SerializeField] private float sightDistance;
        [SerializeField] private float sightAngle;
        [SerializeField] private LayerMask obstacles;

        [Header("Enemy settings")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float attackRange;
        [SerializeField] private Mode mode;
        [SerializeField] private float wanderChangeInterval;

        private Vector3 wanderDirection;
        private float wanderTime;

        private Rigidbody rb;
        private LineOfSight sight;
        private DecisionTree tree;
        private Context ctx;

        private void Awake()
        {
            sight = new LineOfSight();
            rb = GetComponent<Rigidbody>();
            tree = GetComponent<DecisionTree>();

            ctx = new Context { Self = transform, Target = target, Sight = sight, Distance = sightDistance, Angle = sightAngle, Obstacles = obstacles, AttackRange = attackRange };
        }

        private void Update()
        {
            tree.Evaluate(this, ctx);
        }

        private void FixedUpdate()
        {
            Vector3 dir = Vector3.zero;

            switch (mode)
            {
                case Mode.Arrive:
                    dir = SteearingBehaviours.Arrive(transform, target.position, 3f);
                    break;
                case Mode.Pursue:
                    dir = SteearingBehaviours.Pursue(transform, target, rb, 5f);
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
            rb.velocity = dir.normalized * moveSpeed;

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
}
