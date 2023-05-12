namespace Rillium.Script
{
    public class ExpressionStatement : Statement
    {
        public Expression Expression { get; }

        public ExpressionStatement(Expression expression)
        {
            Expression = expression;
        }

        public override void Execute(Scope scope)
        {
            var e = Expression.Evaluate();

            if (e is AssignmentExpression ae)
            {
                ae.Set(scope);
                return;
            }

            // TODO: Should this be supported?
            // example: var x = 1; x; where this handles the final x;
            if (e is VariableExpression ve)
            {
                scope.Set(Constants.OutputValueKey, scope.Get(ve.Name.Value));
                return;
            }

            if (e is NumberExpression ne)
            {
                scope.Set(Constants.OutputValueKey, ne.Value);
                return;
            }

            throw new NotImplementedException("No handled");
        }

        public override T Accept<T>(IStatementVisitor<T> visitor)
        {
            return visitor.VisitExpressionStatement(this);
        }
    }

}
