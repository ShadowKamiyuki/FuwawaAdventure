using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerLS : MonoBehaviour
{
    [Header("Patrol behaviour settings")]
    [SerializeField] private Transform player;
    [SerializeField] private List<Transform> patrolPoints;
    [SerializeField] private int currentPatrolIndex = 0;

    [Header("Enemy settings")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float predictionTime = 0.5f;
    [SerializeField] private float evadeDistance = 3f;
    [SerializeField] private float patrolPointReachDistance = 0.5f;

    [Header("Line of sight settings")]
    [SerializeField] private float distance;
    [SerializeField] private float angle;
    [SerializeField] private LayerMask obstacles;

    private Rigidbody playerRB;
    private LineOfSight los;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerRB = player.GetComponent<Rigidbody>();
        los = new LineOfSight();
    }

    private void Update()
    {
        bool canSeePlayer = los.IsInRange(transform, player, distance) && los.IsInAngle(transform, player, angle) && los.CheckObstacles(transform, player, obstacles);

        bool isClose = Vector3.Distance(transform.position, player.position) <= evadeDistance;

        List<ActionOption> actionOptions = BuildAction(canSeePlayer, isClose);

        ActionOption bestAction = selectBestAction(actionOptions);
        bestAction.action?.Invoke();
    }

    private ActionOption selectBestAction(List<ActionOption> actionOptions)
    {
        ActionOption best = null;
        float bestScore = float.MinValue;

        foreach (ActionOption action in actionOptions)
        {
            if (action.score > bestScore)
            {
                bestScore = action.score;
                best = action;
            }
        }

        return best;
    }

    private List<ActionOption> BuildAction(bool canSeePlayer, bool isClose)
    {
        List<ActionOption> actions = new List<ActionOption>();

        actions.Add(new ActionOption("Patrol", canSeePlayer ? 5f : 30f, Patrol));
        actions.Add(new ActionOption("Pursue", canSeePlayer && !isClose ? 80f : 0f, Pursue));
        actions.Add(new ActionOption("Evade", canSeePlayer && isClose ? 100f : 0f, Evade));

        return actions;
    }

    private void Evade()
    {
        Vector3 dir = SteearingBehaviours.Evade(transform, player, playerRB, predictionTime);
        Move(dir);
    }

    private void Pursue()
    {
        Vector3 dir = SteearingBehaviours.Pursue(transform, player, playerRB, predictionTime);
        Move(dir);
    }

    private void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Count == 0)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        Transform target = patrolPoints[currentPatrolIndex];

        Vector3 dir = target.position - transform.position;
        dir.y = 0f;

        if (dir.magnitude < patrolPointReachDistance)
        {
            rb.linearVelocity = Vector3.zero;
            currentPatrolIndex++;

            if (currentPatrolIndex >= patrolPoints.Count)
            {
                Shuffle(patrolPoints);
                currentPatrolIndex = 0;
            }

            return;
        }

        Move(dir.normalized);
    }

    private void Move(Vector3 dir)
    {
        Vector3 velocity = dir * speed;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;

        if (dir != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * rotationSpeed);
        }
    }

    private void Shuffle(List<Transform> patrolPoints)
    {
        for (int i = patrolPoints.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            
            Transform temp = patrolPoints[i];
            patrolPoints[i] = patrolPoints[j];
            patrolPoints[j] = temp;
        }
    }
}
