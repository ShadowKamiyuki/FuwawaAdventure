using System.Collections.Generic;
using UnityEngine;

namespace GoldenSkeleton
{
    public class GoldenSkeletonController : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform target;

        [Header("Sight")]
        [SerializeField] private float sightDistance = 10f;
        [SerializeField] private float sightAngle = 60f;
        [SerializeField] private LayerMask obstacles;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private float chaseStopDistance = 1.5f;

        [Header("Patrol")]
        [SerializeField] private List<Node> patrolRoute;

        private Rigidbody rb;
        private LineOfSight sight;

        public Transform Target => target;
        public float MoveSpeed => moveSpeed;
        public List<Node> PatrolRoute => patrolRoute;
        public Vector3 MoveDirection { get; private set; }
        public Vector3 LastSeenPosition { get; private set; }
        public float ChaseStopDistance => chaseStopDistance;

        public State CurrentState { get; private set; }

        private IState _currentState;
        private Dictionary<State, IState> states;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            sight = new LineOfSight();

            states = new Dictionary<State, IState>
            {
                { State.Patrol, new PatrolState(this)},
                { State.Chase, new ChaseState(this)},
                { State.Investigate, new InvestigateState(this) }
            };
        }

        private void Start()
        {
            ChangeState(State.Patrol);
        }

        private void Update()
        {
            if (CanSeeTarget())
            {
                LastSeenPosition = target.position;

                if (CurrentState == State.Patrol)
                    ChangeState(State.Chase);
            }

            _currentState?.Update();
        }

        public void ChangeState(State newState)
        {
            if (_currentState != null && CurrentState == newState)
                return;

            _currentState?.Exit();

            CurrentState = newState;
            _currentState = states[newState];

            _currentState.Enter();
        }

        public void UpdateLastSeenPosition()
        {
            LastSeenPosition = target.position;
        }

        public bool CanSeeTarget()
        {
            return sight.IsInRange(transform, target, sightDistance)
                && sight.IsInAngle(transform, target, sightAngle)
                && sight.CheckObstacles(transform, target, obstacles);
        }

        #region AStar
        public List<Node> CalculatePath()
        {
            Node start = GetClosestNode(transform.position);
            Node goal = GetClosestNode(target.position);

            if (start == null || goal == null)
                return new List<Node>();

            if (start == goal)
                return new List<Node> { start };

            return AStar.Run(
                start,
                node => node == goal,
                GetConnections,
                GetCosts,
                node => Heuristic(node, goal)
            );
        }

        // Sobrecarga para calcular camino a cualquier posición
        public List<Node> CalculatePath(Vector3 destination)
        {
            Node start = GetClosestNode(transform.position);
            Node goal = GetClosestNode(destination);

            if (start == null || goal == null)
                return new List<Node>();

            if (start == goal)
                return new List<Node>() { start };

            List<Node> result = AStar.Run(
                start,
                node => node == goal,
                GetConnections,
                GetCosts,
                node => Heuristic(node, goal)
            );

            return result;
        }

        private Node GetClosestNode(Vector3 position)
        {
            Node closest = null;

            Collider[] nodos = Physics.OverlapSphere(position, 10, LayerMask.GetMask("Node"));

            float nearDistance = Mathf.Infinity;

            for (int i = 0; i < nodos.Length; i++)
            {
                Node newNode = nodos[i].gameObject.GetComponent<Node>();
                if (newNode == null) continue;

                float distance = Vector3.Distance(position, nodos[i].transform.position);

                if (distance < nearDistance)
                {
                    Vector3 direction = nodos[i].transform.position - position;
                    if (Physics.Raycast(position, direction.normalized, distance, LayerMask.GetMask("Ground"))) continue;
                    nearDistance = distance;
                    closest = newNode;
                }
            }
            return closest;

        }

        private List<Node> GetConnections(Node node)
        {
            return node.neightbourds;
        }

        private float GetCosts(Node a, Node b)
        {
            return Vector3.Distance(a.transform.position, b.transform.position);
        }

        private float Heuristic(Node node, Node goal)
        {
            return Vector3.Distance(node.transform.position, goal.transform.position);
        }
        #endregion

        public void Move(Vector3 dir)
        {
            dir.y = 0;
            dir = dir.normalized;

            MoveDirection = dir;

            rb.MovePosition(rb.position + dir * moveSpeed * Time.deltaTime);
        }

        public void LookTowards(Vector3 direction)
        {
            direction.y = 0;

            if (direction == Vector3.zero)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime));
        }

        public void Stop()
        {
            MoveDirection = Vector3.zero;
        }
    }
}
