using System.Collections.Generic;
using UnityEngine;

namespace ScaredEnemy
{
    public class ScaredEnemyController : MonoBehaviour
    {
        [Header("Line of sight settings")]
        [SerializeField] private Transform target;
        [SerializeField] private float sightDistance;
        [SerializeField] private float sightAngle;
        [SerializeField] private LayerMask obstacles;

        [Header("Enemy settings")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationSpeed;

        [Header("Wander")]
        [SerializeField] private float wanderChangeInterval;

        private Vector3 wanderDirection;

        private Rigidbody rb;
        private LineOfSight sight;

        public State CurrentState { get; private set; }

        private IState _currentState;
        private Dictionary<State, IState> states;

        public Transform Target => target;
        public Rigidbody RB => rb;

        private void Awake()
        {
            sight = new LineOfSight();
            rb = GetComponent<Rigidbody>();

            states = new Dictionary<State, IState>
            {
                { State.Wander, new WanderState(this)},
                { State.Flee, new FleeState(this)},
            };

            wanderDirection = transform.forward;

            ChangeState(State.Wander);
        }

        private void Update()
        {
            _currentState?.Update();
        }

        private void FixedUpdate()
        {
            _currentState?.FixedUpdate();
        }

        public void ChangeState(State newState)
        {
            Debug.Log($"Cambio: {CurrentState} -> {newState}");

            if (_currentState != null && CurrentState == newState) return;

            _currentState?.Exit();

            if (!states.TryGetValue(newState, out IState nextState))
            {
                Debug.LogError("Estado no encontrado" + newState);
                return;
            }

            CurrentState = newState;
            _currentState = nextState;

            _currentState.Enter();
        }

        public void Move(Vector3 dir)
        {
            rb.velocity = dir.normalized * moveSpeed;

            if (dir.sqrMagnitude > 0.001f)
            {
                Quaternion rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
            }
        }

        public bool CanSeeTarget()
        {
            return target != null &&
                   sight.IsInRange(transform, target, sightDistance) &&
                   sight.IsInAngle(transform, target, sightAngle) &&
                   sight.CheckObstacles(transform, target, obstacles);
        }

        public Vector3 GetWanderDir(ref float timer)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                wanderDirection = SteearingBehaviours.Wander(wanderDirection, 180f).normalized;
                timer = wanderChangeInterval;
            }

            return wanderDirection;
        }
    }
}
