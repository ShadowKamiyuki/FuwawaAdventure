using System;

namespace Poyo
{
    public abstract class DecisionNode
    {
        public abstract void Evaluate(PoyoController enemy, Context ctx);
    }

    public class QuestionNode : DecisionNode
    {
        private Func<Context, bool> question;
        private DecisionNode trueNode;
        private DecisionNode falseNode;

        public QuestionNode(Func<Context, bool> question, DecisionNode trueNode, DecisionNode falseNode)
        {
            this.question = question;
            this.trueNode = trueNode;
            this.falseNode = falseNode;
        }

        public override void Evaluate(PoyoController enemy, Context ctx)
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
        private Action<PoyoController> action;

        public ActionNode(Action<PoyoController> action)
        {
            this.action = action;
        }

        public override void Evaluate(PoyoController enemy, Context ctx)
        {
            action(enemy);
        }
    }
}
