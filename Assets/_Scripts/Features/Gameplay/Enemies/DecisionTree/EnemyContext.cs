using UnityEngine;

public class EnemyContext
{
    public Transform self;
    public Transform target;
    public LineOfSight los;
    public float distance;
    public float angle;
    public float attackRange;
    public LayerMask obstacles;
}
