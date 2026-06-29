using UnityEngine;

namespace Skeleton
{
    public class SkeletonAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private Rigidbody rb;
        private SkeletonController controller;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            controller = GetComponent<SkeletonController>();
        }

        private void Update()
        {
            UpdateMovementAnimation();
            UpdateStateAnimation();
        }

        private void UpdateMovementAnimation()
        {
            float speed = controller.MoveDirection.magnitude;

            animator.SetFloat("Speed", speed);
        }

        private void UpdateStateAnimation()
        {
            bool isChasing =
                controller.CurrentState == State.Chase;

            bool isAlert =
                controller.CanSeeTarget();

            animator.SetBool("IsChasing", isChasing);
            animator.SetBool("IsAlert", isAlert);
        }
    }
}