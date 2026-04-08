using UnityEngine;

namespace Poyo
{
    public class DecisionTree : MonoBehaviour
    {
        private DecisionNode rootNode;

        // First Actions, then Questions and Root at last (the tree must be created from below to above)
        private void Awake()
        {
            ActionNode patrolNode = new ActionNode(enemy => enemy.ChangeMode(Mode.Wander));
            ActionNode pursueNode = new ActionNode(enemy => enemy.ChangeMode(Mode.Pursue));
            ActionNode attackNode = new ActionNode(enemy => enemy.ChangeMode(Mode.Arrive));

            QuestionNode attackRangeNode = new QuestionNode(ctx => Vector3.Distance(ctx.Self.position, ctx.Target.position) < ctx.AttackRange, attackNode, pursueNode);

            rootNode = new QuestionNode(ctx => 
                ctx.Sight.IsInRange(ctx.Self, ctx.Target, ctx.Distance) &&
                ctx.Sight.IsInAngle(ctx.Self, ctx.Target, ctx.Angle) &&
                ctx.Sight.CheckObstacles(ctx.Self, ctx.Target, ctx.Obstacles),
                attackRangeNode, patrolNode
            );
        }

        public void Evaluate(PoyoController poyoController, Context ctx)
        {
            rootNode.Evaluate(poyoController, ctx);
        }
    }
}
