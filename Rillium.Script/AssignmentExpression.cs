namespace Rillium.Script
{
    public class AssignmentExpression : Expression
    {
        public VariableExpression Target { get; }
        public Expression Value { get; }

        public AssignmentExpression(VariableExpression target, Expression value)
        {
            Target = target;
            Value = value;
        }

        public override Expression Evaluate() => this;


        public void Set(Scope scope)
        {
            scope.Set(Target.Name.Value, Value.Evaluate());
        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.VisitAssignmentExpression(this);
        }
    }
}
