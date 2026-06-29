using UnityEngine;

public class EnemySBDecisionTree : MonoBehaviour
{
    private DecisionNode rootNode;

    private void Awake()
    {
        ActionNode patrolNode = new ActionNode(enemy => enemy.ChangeMode(Mode.Wander));
        ActionNode pursuitNode = new ActionNode(enemy => enemy.ChangeMode(Mode.Pursue));
        ActionNode attackNode = new ActionNode(enemy => enemy.ChangeMode(Mode.Arrive));

        QuestionNode attackRangeNode = new QuestionNode(ctx => Vector3.Distance(ctx.self.position, ctx.target.position) < ctx.attackRange, attackNode, pursuitNode);

        rootNode = new QuestionNode(
            ctx =>
                ctx.los.IsInRange(ctx.self, ctx.target, ctx.distance) &&
                ctx.los.IsInAngle(ctx.self, ctx.target, ctx.angle) &&
                ctx.los.CheckObstacles(ctx.self, ctx.target, ctx.obstacles),
            attackRangeNode,
            patrolNode
        );
    }

    public void Evaluate(EnemyControllerSB enemyController, EnemyContext ctx)
    {
        rootNode.Evaluate(enemyController, ctx);
    }
}
