using System.Collections.Generic;
using UnityEngine;

namespace Skeleton
{
    public class ChaseState : IState
    {
        private SkeletonController controller;

        private List<Node> path;
        private int currentIndex;

        private float repathTimer;
        private const float repathInterval = 0.5f;

        public ChaseState(SkeletonController controller)
        {
            this.controller = controller;
        }

        public void Enter()
        {
            path = null;
            currentIndex = 0;
            repathTimer = 0f;
        }

        // CASO 1: visión directa -> PURSUE (sin A*)
        // CASO 2: sin visión -> A* + steering
        public void Update()
        {
            if (controller.CanSeeTarget())
            {
                PursueTarget();
                return;
            }

            repathTimer += Time.deltaTime;

            if (path == null || path.Count == 0 || repathTimer >= repathInterval)
            {
                RecalculatePath();
                repathTimer = 0f;
            }

            FollowPath();
        }

        public void FixedUpdate() { }

        public void Exit() { }

        // PURSUE
        private void PursueTarget()
        {
            float distance = Vector3.Distance(controller.transform.position, controller.Target.position);

            if (distance <= controller.ChaseStopDistance)
            {
                controller.Stop();

                Vector3 lookDir = controller.Target.position - controller.transform.position;

                lookDir.y = 0;

                controller.LookTowards(lookDir.normalized);
                return;
            }

            Rigidbody targetRB = controller.Target.GetComponent<Rigidbody>();

            Vector3 dir = SteearingBehaviours.Pursue(
                controller.transform,
                controller.Target,
                targetRB,
                1f,
                controller.MoveSpeed
            );

            controller.Move(dir);
            controller.LookTowards(dir);
        }

        // A* PATHFINDING
        private void RecalculatePath()
        {
            path = controller.CalculatePath();
            currentIndex = 0;
        }

        // FOLLOW PATH + ARRIVE
        private void FollowPath()
        {
            if (path == null || path.Count == 0)
                return;

            if (currentIndex >= path.Count)
                return;

            Vector3 targetPos = path[currentIndex].transform.position;

            Vector3 steering = SteearingBehaviours.Arrive(controller.transform, targetPos, slowRadius: 2f);

            controller.Move(steering);
            controller.LookTowards(steering);

            if (Vector3.Distance(controller.transform.position, targetPos) < 0.4f)
            {
                currentIndex++;
            }
        }
    }
}