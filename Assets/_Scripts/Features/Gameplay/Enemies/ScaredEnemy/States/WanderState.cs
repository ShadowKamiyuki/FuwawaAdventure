using UnityEngine;

namespace ScaredEnemy
{
    public class WanderState : IState
    {
        private ScaredEnemyController enemy;
        private float timer;

        public WanderState(ScaredEnemyController enemy)
        {
            this.enemy = enemy;
        }

        public void Enter()
        {
            timer = 0f;
        }

        public void Update()
        {
            if (enemy.CanSeeTarget())
                enemy.ChangeState(State.Flee);
        }

        public void FixedUpdate()
        {
            Vector3 dir = enemy.GetWanderDir(ref timer);
            enemy.Move(dir);
        }

        public void Exit() { }
    }
}
