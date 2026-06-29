using UnityEngine;

namespace GoldenSkeleton
{
    public class ChaseState : IState
    {
        private GoldenSkeletonController controller;

        public ChaseState(GoldenSkeletonController controller)
        {
            this.controller = controller;
        }

        public void Enter() { }

        public void Update()
        {
            if (!controller.CanSeeTarget())
            {
                controller.ChangeState(State.Investigate);
                return;
            }

            controller.UpdateLastSeenPosition();

            float distance = Vector3.Distance(controller.transform.position, controller.Target.position);

            if (distance <= controller.ChaseStopDistance)
            {
                controller.Stop();

                Vector3 lookDir = controller.Target.position - controller.transform.position;

                controller.LookTowards(lookDir);

                return;
            }

            Rigidbody targetRB = controller.Target.GetComponent<Rigidbody>();

            Vector3 steering = SteearingBehaviours.Pursue(controller.transform, controller.Target, targetRB, 1f, controller.MoveSpeed);

            controller.Move(steering);
            controller.LookTowards(steering);
        }

        public void FixedUpdate() { }

        public void Exit() { }
    }
}