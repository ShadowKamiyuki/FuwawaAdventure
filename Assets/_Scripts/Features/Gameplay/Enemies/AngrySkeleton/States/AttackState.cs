using UnityEngine;

namespace AngrySkeleton
{
    public class AttackState : IState
    {
        private AngrySkeletonController controller;

        public AttackState(AngrySkeletonController controller)
        {
            this.controller = controller;
        }

        public void Enter()
        {
            controller.Stop();
        }

        public void Update()
        {
            controller.Stop();

            Vector3 lookDir = controller.Target.position - controller.transform.position;

            lookDir.y = 0;

            controller.LookTowards(lookDir.normalized);
        }

        public void FixedUpdate() { }

        public void Exit() { }
    }
}