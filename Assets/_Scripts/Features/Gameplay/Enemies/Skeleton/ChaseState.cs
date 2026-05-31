using System.Collections.Generic;
using UnityEngine;

namespace Skeleton
{
    public class ChaseState : IState
    {
        public ChaseState(Transform self)
        {

        }

        public void Enter()
        {
            throw new System.NotImplementedException();
        }

        public void Exit()
        {
            throw new System.NotImplementedException();
        }

        public void FixedUpdate()
        {
            throw new System.NotImplementedException();
        }

        public void Update()
        {
            SetPathAStar();
        }

        private void SetPathAStar()
        {
            //Node inicio = GetClosestNode(_entity.transform.position);
            //List<Node> path = AStar.Run(inicio, IsSatisfied, GetConnections, GetCosts, Heuristic);
            //List<Vector3> points = new List<Vector3>();

            //for (int i = 0; i < path.Count; i++)
            //{
            //    points.Add(path[i].transform.position);
            //}

            //SetWaypoints(points);
        }

        Node GetClosestNode(Vector3 position)
        {
            Node closest = null;

            Collider[] nodos = Physics.OverlapSphere(position, 10, LayerMask.GetMask("Node"));

            float nearDistance = Mathf.Infinity;

            for (int i = 0; i < nodos.Length; i++)
            {
                Node newNode = nodos[i].gameObject.GetComponent<Node>();
                if (newNode == null) continue;

                float distance = Vector3.Distance(position, nodos[i].transform.position);

                if ((distance < nearDistance))
                {
                    Vector3 direction = nodos[i].transform.position - position;
                    if (Physics.Raycast(position, direction.normalized, distance, LayerMask.GetMask("Wall"))) continue;
                    nearDistance = distance;
                    closest = newNode;
                }
            }
            return closest;

        }

        public bool IsSatisfied(Node node)
        {
            //return node == goal;
            return false;
        }

        public List<Node> GetConnections(Node node)
        {
            return node.neightbourds;
        }

        public float Heuristic(Node node)
        {
            float h = 0;
            //h += Vector3.Distance(node.transform.position, goal.transform.position);
            return h;
        }

        public float GetCosts(Node node1, Node node2)
        {
            float costs = 0;
            costs += Vector3.Distance(node1.transform.position, node2.transform.position);
            if (node2.hasTrap)
            {
                costs += 100;
            }
            return costs;
        }
    }
}
