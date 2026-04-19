using UnityEngine;

namespace Predicter
{
    public class ChaseState : IState
    {
        private PredicterController enemy;

        public ChaseState(PredicterController enemy)
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
        }

        public void FixedUpdate()
        {
            Vector3 dir = SteearingBehaviours.Pursue(enemy.transform, enemy.Target, enemy.RB, 5f);
            enemy.Move(dir);
        }

        public void Exit() { }
    }
}
