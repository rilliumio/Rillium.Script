using Rillium.Script.Expressions;

namespace Rillium.Script.Statements
{
    internal class IfStatement : Statement
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

        public override void Execute(Scope scope)
        {
            if (Condition.EvaluateToBool(scope))
            {
                ThenStatement.Execute(scope);
            }
            else if (ElseStatement != null)
            {
                ElseStatement.Execute(scope);
            }
        }
    }
}
