using UnityEngine;
using Poyoyo;

public class DashState : IState
{
    private PoyoyoController enemy;

    public DashState(PoyoyoController enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.StartDash(out _);
        enemy.StartCooldown();
    }

    public void Update()
    {
        if (enemy.UpdateDash())
        {
            enemy.ChangeState(State.Cooldown);
        }
    }

    public void FixedUpdate()
    {
        Vector3 dir = enemy.RB.velocity.normalized;

        if (Physics.Raycast(enemy.transform.position, dir, out RaycastHit hit, 1.2f))
        {
            if (hit.transform.CompareTag("Player"))
            {
                enemy.ApplyKnockback(hit.transform, 8f);
            }
        }
    }

    public void Exit()
    {
        enemy.SetVelocity(Vector3.zero);
    }
}
