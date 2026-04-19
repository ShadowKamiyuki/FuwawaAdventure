using Popoyo;
using UnityEngine;

public class TelegraphState : IState
{
    private PopoyoController enemy;
    private float timer = 0.6f;

    private Vector3 dir;

    public TelegraphState(PopoyoController enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.SetVelocity(Vector3.zero);
        dir = (enemy.Target.position - enemy.transform.position).normalized;
        enemy.FaceDirection(dir); // opcional
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            enemy.ChangeState(State.Dash);
        }
    }

    public void FixedUpdate() { }
    public void Exit() { }
}