using UnityEngine;
using Popoyo;

public class CooldownState : IState
{
    private PopoyoController enemy;

    public CooldownState(PopoyoController enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.SetVelocity(Vector3.zero);
    }

    public void Update()
    {
        if (!enemy.CanSeeTarget())
        {
            enemy.ChangeState(State.Wander);
            return;
        }

        enemy.ChangeState(State.Chase);
    }

    public void FixedUpdate()
    {

    }

    public void Exit() { }
}
