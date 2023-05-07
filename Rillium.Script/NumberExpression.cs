namespace Rillium.Script
{
    public class NumberExpression : Expression
    {
        public double Value { get; }

        public NumberExpression(double value)
        {
            Value = value;
        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.VisitNumberExpression(this);
        }
    }
}
