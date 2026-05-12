//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//public class CrashController : MonoBehaviour
//{
//    CrashModel _model;
//    Animator _anim;
//    FSM<StatesEnum> _fsm;
//    ITreeNode _root;
//    StatePathfinding<StatesEnum> _statePathfinding;
//    public Node start;
//    public Node goal;
//    public Transform target;

//    void Awake()
//    {
//        _anim = GetComponent<Animator>();
//        _model = GetComponent<CrashModel>();
//    }
//    private void Start()
//    {
//        InitializeFSM();
//        InitializeTree();
//    }
//    void InitializeFSM()
//    {
//        _fsm = new FSM<StatesEnum>();
//        var idle = new CrashStateIdle<StatesEnum>(_anim);
//        _statePathfinding = new StatePathfinding<StatesEnum>(_model.transform, _model, _anim);

//        idle.AddTransition(StatesEnum.Waypoints, _statePathfinding);
//        _statePathfinding.AddTransition(StatesEnum.Idle, idle);
//        _fsm.SetInit(idle);
//    }
//    void InitializeTree()
//    {
//        var idle = new ActionNode(() => _fsm.Transition(StatesEnum.Idle));
//        var follow = new ActionNode(() => _fsm.Transition(StatesEnum.Waypoints));

//        var qFollowPoints = new QuestionNode(() => _statePathfinding.IsFinishPath, idle, follow);
//        _root = qFollowPoints;
//    }
//    private void Update()
//    {
//        _fsm.OnUpdate();
//        _root.Execute();
//    }
//    public void BFS()
//    {
//        _statePathfinding.start = start;
//        _statePathfinding.goal = goal;
//        _statePathfinding.SetPath();
//    }
//    public void DFS()
//    {
//        _statePathfinding.start = start;
//        _statePathfinding.goal = goal;
//        _statePathfinding.SetPathDFS();
//    }

//    public void Dijkstra()
//    {
//        _statePathfinding.start = start;
//        _statePathfinding.goal = goal;
//        _statePathfinding.SetPathDijkstra();
//    }

//    public void AStar()
//    {
//        _statePathfinding.start = start;
//        _statePathfinding.goal = goal;
//        _statePathfinding.SetPathAStar();
//    }
//}
