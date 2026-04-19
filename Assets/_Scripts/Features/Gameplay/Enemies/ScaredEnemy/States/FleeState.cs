using UnityEngine;

namespace ScaredEnemy
{
    public class FleeState : IState
    {
        private ScaredEnemyController enemy;

        public FleeState(ScaredEnemyController enemy)
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
            Vector3 dir = SteearingBehaviours.Flee(enemy.transform, enemy.Target.position);
            enemy.Move(dir);
        }

        public void Exit() { }
    }
}
