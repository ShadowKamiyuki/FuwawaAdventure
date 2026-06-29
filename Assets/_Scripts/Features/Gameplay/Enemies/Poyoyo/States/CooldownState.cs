using UnityEngine;
using Poyoyo;

public class CooldownState : IState
{
    private PoyoyoController enemy;

    public CooldownState(PoyoyoController enemy)
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
