using System;

public abstract class DecisionNode
{
    public abstract void Evaluate(EnemyControllerSB enemy, EnemyContext ctx);
}

public class QuestionNode : DecisionNode
{
    private Func<EnemyContext, bool> question;
    private DecisionNode trueNode;
    private DecisionNode falseNode;

    public QuestionNode(Func<EnemyContext, bool> question, DecisionNode trueNode, DecisionNode falseNode)
    {
        this.question = question;
        this.trueNode = trueNode;
        this.falseNode = falseNode;
    }

    public override void Evaluate(EnemyControllerSB enemy, EnemyContext ctx)
    {
        if (question(ctx))
        {
            trueNode.Evaluate(enemy, ctx);
        }
        else
        {
            falseNode.Evaluate(enemy, ctx);
        }
    }
}

public class ActionNode : DecisionNode
{
    private Action<EnemyControllerSB> action;

    public ActionNode(Action<EnemyControllerSB> action)
    {
        this.action = action;
    }

    public override void Evaluate(EnemyControllerSB enemy, EnemyContext ctx)
    {
        action(enemy);
    }
}

public class WeightedRandomActionNode : DecisionNode
{
    private (float weight, Action<EnemyControllerSB> action)[] options;

    public WeightedRandomActionNode((float weight, Action<EnemyControllerSB> action)[] options)
    {
        this.options = options;
    }

    public override void Evaluate(EnemyControllerSB enemy, EnemyContext ctx)
    {
        float totalWeight = 0f;

        foreach (var option in options)
        {
            totalWeight += option.weight;
        }

        float randomValue = UnityEngine.Random.Range(0, totalWeight);
        float currentWeight = 0f;

        foreach (var option in options)
        {
            currentWeight += option.weight;

            if (randomValue <= currentWeight)
            {
                option.action(enemy);
                return;
            }
        }
    }
}
