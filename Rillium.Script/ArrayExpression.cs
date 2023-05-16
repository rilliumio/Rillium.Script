namespace Rillium.Script
{
    public class ArrayExpression : Expression
    {
        public List<Expression> Value { get; }

        public ArrayExpression(List<Expression> array)
        {
            Value = array;
        }

        public override Expression Evaluate(Scope scope) => this;

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.VisitArrayExpression(this);
        }
    }
}
