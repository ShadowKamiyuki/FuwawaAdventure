//using System;

//public abstract class DecisionNode
//{
//    public abstract void Evaluate(EnemyController enemy, EnemyContext ctx);
//}

//public class QuestionNode : DecisionNode
//{
//    private Func<EnemyContext, bool> question;
//    private DecisionNode trueNode;
//    private DecisionNode falseNode;

//    public QuestionNode(Func<EnemyContext, bool> question, DecisionNode trueNode, DecisionNode falseNode)
//    {
//        this.question = question;
//        this.trueNode = trueNode;
//        this.falseNode = falseNode;
//    }

//    public override void Evaluate(EnemyController enemy, EnemyContext ctx)
//    {
//        if (question(ctx))
//        {
//            trueNode.Evaluate(enemy, ctx);
//        }
//        else
//        {
//            falseNode.Evaluate(enemy, ctx);
//        }
//    }
//}

//public class ActionNode : DecisionNode
//{
//    private Action<EnemyController> action;

//    public ActionNode(Action<EnemyController> action)
//    {
//        this.action = action;
//    }

//    public override void Evaluate(EnemyController enemy, EnemyContext ctx)
//    {
//        action(enemy);
//    }
//}

//public class WeightedRandomActionNode : DecisionNode
//{
//    private (float weight, Action<EnemyController> action)[] options;

//    public WeightedRandomActionNode((float weight, Action<EnemyController> action)[] options)
//    {
//        this.options = options;
//    }

//    public override void Evaluate(EnemyController enemy, EnemyContext ctx)
//    {
//        float totalWeight = 0f;

//        foreach (var option in options)
//        {
//            totalWeight += option.weight;
//        }

//        float randomValue = UnityEngine.Random.Range(0, totalWeight);
//        float currentWeight = 0f;

//        foreach (var option in options)
//        {
//            currentWeight += option.weight;

//            if (randomValue <= currentWeight)
//            {
//                option.action(enemy);
//                return;
//            }
//        }
//    }
//}
