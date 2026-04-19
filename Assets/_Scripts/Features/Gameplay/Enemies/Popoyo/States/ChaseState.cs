using UnityEngine;
using Popoyo;

public class ChaseState : IState
{
    private PopoyoController enemy;

    public ChaseState(PopoyoController enemy)
    {
        this.enemy = enemy;
    }

    public void Enter() { }

    public void Update()
    {
        if (!enemy.CanSeeTarget())
        {
            enemy.ChangeState(State.Wander);
            return;
        }

        if (enemy.InAttackRange() && enemy.CanDash())
        {
            enemy.ChangeState(State.Telegraph);
        }
    }

    public void FixedUpdate()
    {
        Vector3 dir = SteearingBehaviours.Pursue(enemy.transform, enemy.Target, enemy.RB, 5f, enemy.MoveSpeed);
        enemy.Move(dir);
    }

    public void Exit() { }
}
