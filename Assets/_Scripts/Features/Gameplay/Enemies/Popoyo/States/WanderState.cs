using UnityEngine;

namespace Popoyo
{
    public class WanderState : IState
    {
        private PopoyoController enemy;
        private float timer;

        public WanderState(PopoyoController enemy)
        {
            this.enemy = enemy;
        }

        public void Enter() { }

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
