using System.Collections.Generic;
using UnityEngine;

namespace GoldenSkeleton
{
    public class InvestigateState : IState
    {
        private GoldenSkeletonController controller;

        private List<Node> path;

        private int currentIndex;
        private float timer;
        private float minTime = 2f;

        public InvestigateState(GoldenSkeletonController controller)
        {
            this.controller = controller;
        }

        public void Enter()
        {
            path = controller.CalculatePath(controller.LastSeenPosition);
            currentIndex = 0;

            timer = 0f;
        }

        public void Update()
        {
            timer += Time.deltaTime;

            if (controller.CanSeeTarget())
            {
                controller.ChangeState(State.Chase);
                return;
            }

            if (path == null || path.Count == 0)
            {
                controller.ChangeState(State.Patrol);
                return;
            }

            if (currentIndex >= path.Count)
            {
                if (timer > minTime)
                    controller.ChangeState(State.Patrol);

                return;
            }

            Vector3 targetPos = path[currentIndex].transform.position;

            Vector3 steering = SteearingBehaviours.Arrive(controller.transform, targetPos, 2f);

            controller.Move(steering);
            controller.LookTowards(steering);

            if (Vector3.Distance(controller.transform.position, targetPos) < 0.3f)
            {
                currentIndex++;
            }

            Debug.DrawLine(controller.transform.position, targetPos, Color.green);
        }

        public void FixedUpdate() { }

        public void Exit() { }
    }
}