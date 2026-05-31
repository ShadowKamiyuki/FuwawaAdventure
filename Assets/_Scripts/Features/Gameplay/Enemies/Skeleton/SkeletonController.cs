using System.Collections.Generic;
using UnityEngine;

namespace Skeleton
{
    public class SkeletonController : MonoBehaviour
    {
        [Header("Line of sight settings")]
        [SerializeField] private Transform target;
        [SerializeField] private float sightDistance;
        [SerializeField] private float sightAngle;
        [SerializeField] private LayerMask obstacles;

        [Header("Enemy settings")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationSpeed;

        private Rigidbody rb;
        private LineOfSight sight;

        public State CurrentState { get; private set; }

        private IState _currentState;
        private Dictionary<State, IState> states;

        private void Awake()
        {
            sight = new LineOfSight();
            rb = GetComponent<Rigidbody>();

            states = new Dictionary<State, IState>
            {
                { State.Wander, new WanderState()},
                { State.Chase, new ChaseState(transform)}
            };
        }

        public void CalculatePath()
        {
            
        }
    }

}