namespace Rillium.Script
{
    public class IfStatement : Statement
    {
        public Expression Condition { get; }
        public Statement ThenStatement { get; }
        public Statement ElseStatement { get; }

        public IfStatement(Expression condition, Statement thenStatement, Statement elseStatement = null)
        {
            Condition = condition;
            ThenStatement = thenStatement;
            ElseStatement = elseStatement;
        }

        public override void Execute()
        {
            if (Condition.EvaluateToBool())
            {
                ThenStatement.Execute();
            }
            else if (ElseStatement != null)
            {
                ElseStatement.Execute();
            }
        }

        public override T Accept<T>(IStatementVisitor<T> visitor)
        {
            return visitor.VisitIfStatement(this);
        }
    }
}
