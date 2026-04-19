using System.Collections.Generic;
using UnityEngine;

namespace Popoyo
{
    public class PopoyoController : MonoBehaviour
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

        [Header("Wander")]
        [SerializeField] private float wanderChangeInterval;

        [Header("Enemy attack")]
        [SerializeField] private float dashForce;
        [SerializeField] private float dashCooldown;
        [SerializeField] private float dashDuration;

        private float dashTimer;
        private float cooldownTimer = 0f;

        private Vector3 wanderDirection;

        private Rigidbody rb;
        private LineOfSight sight;

        public State CurrentState { get; private set; }

        private IState _currentState;
        private Dictionary<State, IState> states;

        public Transform Target => target;
        public Rigidbody RB => rb;
        public bool IsDashing => CurrentState == State.Dash;
        public float MoveSpeed => moveSpeed;

        private void Awake()
        {
            sight = new LineOfSight();
            rb = GetComponent<Rigidbody>();

            states = new Dictionary<State, IState>
            {
                { State.Wander, new WanderState(this)},
                { State.Chase, new ChaseState(this)},
                { State.Dash, new DashState(this)},
                { State.Cooldown, new CooldownState(this)},
                { State.Telegraph, new TelegraphState(this)},
            };

            wanderDirection = transform.forward;

            ChangeState(State.Wander);
        }

        private void Update()
        {
            _currentState?.Update();
            UpdateCooldown();
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
            if (IsDashing) return;

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

        public bool InAttackRange()
        {
            return Vector3.Distance(transform.position, target.position) <= attackRange;
        }

        public bool CanDash()
        {
            Debug.Log("Cooldown: " + cooldownTimer);

            return cooldownTimer <= 0f;
        }

        public void StartCooldown()
        {
            cooldownTimer = dashCooldown;
        }

        public void UpdateCooldown()
        {
            if (cooldownTimer > 0f)
                cooldownTimer -= Time.deltaTime;
        }

        public void SetVelocity(Vector3 v)
        {
            rb.velocity = v;
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

        public void StartDash(out Vector3 dir)
        {
            dashTimer = dashDuration;
            dir = (target.position - transform.position).normalized;

            rb.velocity = Vector3.zero;
            rb.AddForce(dir * dashForce, ForceMode.VelocityChange);
        }

        public bool UpdateDash()
        {
            dashTimer -= Time.deltaTime;
            return dashTimer <= 0f;
        }

        public void FaceDirection(Vector3 dir)
        {
            dir.y = 0f;

            Quaternion targetRot = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotationSpeed * Time.deltaTime * 2f
            );
        }

        public void ApplyKnockback(Transform target, float force)
        {
            Vector3 dir = (target.position - transform.position).normalized;
            Rigidbody targetRb = target.GetComponent<Rigidbody>();

            targetRb.AddForce(dir * force, ForceMode.Impulse);
        }
    }
}
