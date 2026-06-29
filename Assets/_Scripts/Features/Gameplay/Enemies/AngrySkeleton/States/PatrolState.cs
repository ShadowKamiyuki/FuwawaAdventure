using UnityEngine;

namespace AngrySkeleton
{
    public class PatrolState : IState
    {
        private AngrySkeletonController controller;
        private int currentWaypoint;

        public PatrolState(AngrySkeletonController controller)
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

            Vector3 dir = targetPos - controller.transform.position;
            dir.y = 0;
            dir = dir.normalized;

            controller.LookTowards(dir);
            controller.Move(dir);

            if (Vector3.Distance(controller.transform.position, targetPos) < 0.2f)
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
