using UnityEngine;

namespace Predicter
{
    public class WanderState : IState
    {
        private PredicterController enemy;
        private float timer;

        public WanderState(PredicterController enemy)
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
                enemy.ChangeState(State.Chase);
        }

        public void FixedUpdate()
        {
            Vector3 dir = enemy.GetWanderDir(ref timer);
            enemy.Move(dir);
        }

        public void Exit() { }
    }
}
