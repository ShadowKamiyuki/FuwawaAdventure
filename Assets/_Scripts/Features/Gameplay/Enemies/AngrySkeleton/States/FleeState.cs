using UnityEngine;

namespace AngrySkeleton
{
    public class FleeState : IState
    {
        private AngrySkeletonController controller;

        public FleeState(AngrySkeletonController controller)
        {
            this.controller = controller;
        }

        public void Enter() { }

        public void Update()
        {
            Vector3 fleeDir =
                controller.transform.position -
                controller.Target.position;

            fleeDir.y = 0;

            if (fleeDir.sqrMagnitude < 0.01f)
                return;

            controller.Move(fleeDir.normalized);

            // Sigue mirando al jugador mientras retrocede
            Vector3 lookDir = controller.Target.position - controller.transform.position;

            controller.LookTowards(lookDir);
        }

        public void FixedUpdate() { }

        public void Exit() { }
    }
}