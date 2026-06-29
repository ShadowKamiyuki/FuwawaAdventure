using System;
using UnityEngine;

namespace GoldenSkeleton
{
    public class PatrolState : IState
    {
        private GoldenSkeletonController controller;
        private int currentWaypoint;

        public PatrolState(GoldenSkeletonController controller)
        {
            this.controller = controller;
        }

        public void Enter()
        {
            currentWaypoint = GetClosestWaypointIndex();
        }

        private int GetClosestWaypointIndex()
        {
            int closestIndex = 0;
            float minDistance = Mathf.Infinity;

            for (int i = 0; i < controller.PatrolRoute.Count; i++)
            {
                float dist = Vector3.Distance(controller.transform.position, controller.PatrolRoute[i].transform.position);

                if (dist < minDistance)
                {
                    minDistance = dist;
                    closestIndex = i;
                }
            }

            return closestIndex;
        }

        public void Update()
        {
            if (controller.PatrolRoute.Count == 0)
                return;

            Vector3 targetPos = controller.PatrolRoute[currentWaypoint].transform.position;

            Vector3 steering = SteearingBehaviours.Arrive(controller.transform, targetPos, 1.5f);

            controller.Move(steering);
            controller.LookTowards(steering);

            if (Vector3.Distance(controller.transform.position, targetPos) < 0.3f)
            {
                currentWaypoint++;

                if (currentWaypoint >= controller.PatrolRoute.Count)
                    currentWaypoint = 0;
            }
        }

        public void FixedUpdate() { }

        public void Exit() { }
    }
}