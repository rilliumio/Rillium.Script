namespace Rillium.Script
{
    public class LiteralExpression : Expression
    {
        public LiteralValue Value { get; }

        public LiteralExpression(LiteralValue value)
        {
            Value = value;
        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.VisitLiteralExpression(this);
        }
    }
}
