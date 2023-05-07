namespace Rillium.Script
{
    public class ExpressionStatement : Statement
    {
        public Expression Expression { get; }

        public ExpressionStatement(Expression expression)
        {
            Expression = expression;
        }

        public override T Accept<T>(IStatementVisitor<T> visitor)
        {
            return visitor.VisitExpressionStatement(this);
        }
    }

}
