//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using static UnityEditor.PlayerSettings;

//public class StatePathfinding<T> : StateFollowPoints<T>
//{
//    IMove _move;
//    Animator _anim;
//    public Node start;
//    public Node goal;
//    public Transform target;
//    public StatePathfinding(Transform entity, IMove move, Animator anim, float distanceToPoint = 0.2F) : base(entity, distanceToPoint)
//    {
//        _move = move;
//        _anim = anim;
//    }
//    public StatePathfinding(Transform entity, IMove move, Animator anim, List<Vector3> waypoints, float distanceToPoint = 0.2f) : base(entity, waypoints, distanceToPoint)
//    {
//        _move = move;
//        _anim = anim;
//    }

//    protected override void OnMove(Vector3 dir)
//    {
//        base.OnMove(dir);
//        _move.Move(dir);
//        _move.LookDir(dir);
//    }
//    protected override void OnStartPath()
//    {
//        base.OnStartPath();
//        //_move.SetPosition(_waypoints[0]);
//        _anim.SetFloat("Vel", 1);
//    }
//    protected override void OnFinishPath()
//    {
//        base.OnFinishPath();
//        _anim.SetFloat("Vel", 0);
//    }

//    public void SetPath()
//    {
//        List<Node> path = BFS.Run(start, IsSatisfied, GetConnections);
//        List<Vector3> points = new List<Vector3>();

//        for (int i = 0; i < path.Count; i++)
//        {
//            points.Add(path[i].transform.position);
//        }

//        _move.SetPosition(start.transform.position);
//        SetWaypoints(points);
//    }

//    public void SetPathDijkstra()
//    {
//        List<Node> path = Dijkstra.Run(start, IsSatisfied, GetConnections, GetCosts);
//        List<Vector3> points = new List<Vector3>();

//        for (int i = 0; i < path.Count; i++)
//        {
//            points.Add(path[i].transform.position);
//        }

//        _move.SetPosition(start.transform.position);
//        SetWaypoints(points);
//    }

//    public void SetPathAStar()
//    {
//        Node inicio = GetClosestNode(_entity.transform.position);
//        List<Node> path = AStar.Run(inicio, IsSatisfied, GetConnections, GetCosts, Heuristic);
//        List<Vector3> points = new List<Vector3>();

//        for (int i = 0; i < path.Count; i++)
//        {
//            points.Add(path[i].transform.position);
//        }

//        //_move.SetPosition(start.transform.position);
//        SetWaypoints(points);
//    }

//    public void SetPathDFS()
//    {
//        List<Node> path = DFS.Run(start, IsSatisfied, GetConnections);
//        List<Vector3> points = new List<Vector3>();

//        for (int i = 0; i < path.Count; i++)
//        {
//            points.Add(path[i].transform.position);
//        }

//        _move.SetPosition(start.transform.position);
//        SetWaypoints(points);
//    }


//    Node GetClosestNode(Vector3 position)
//    {
//        Node closest = null;

//        Collider[] nodos = Physics.OverlapSphere(position, 10, LayerMask.GetMask("Node"));

//        float nearDistance = Mathf.Infinity;

//        for (int i = 0; i < nodos.Length; i++)
//        {
//            Node newNode = nodos[i].gameObject.GetComponent<Node>();
//            if (newNode == null) continue;

//            float distance = Vector3.Distance(position, nodos[i].transform.position);

//            if ((distance < nearDistance))
//            {
//                Vector3 direction = nodos[i].transform.position - position;
//                if (Physics.Raycast(position, direction.normalized, distance, LayerMask.GetMask("Wall"))) continue;
//                nearDistance = distance;
//                closest = newNode;
//            }
//        }
//        return closest;

//    }


//    public bool IsSatisfied(Node node)
//    {
//        return node == goal;
//    }

//    public List<Node> GetConnections(Node node)
//    {
//        return node.neightbourds;
//    }

//    public float Heuristic(Node node)
//    {
//        float h = 0;
//        h += Vector3.Distance(node.transform.position, goal.transform.position);
//        return h;
//    }

//    public float GetCosts(Node node1, Node node2)
//    {
//        float costs = 0;
//        costs += Vector3.Distance(node1.transform.position, node2.transform.position);
//        if (node2.hasTrap)
//        {
//            costs += 100;
//        }
//        return costs;
//    }

//}
