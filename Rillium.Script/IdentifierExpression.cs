namespace Rillium.Script
{
    public class IdentifierExpression : Expression
    {
        public string Name { get; }

        public IdentifierExpression(string name)
        {
            Name = name;
        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.VisitIdentifierExpression(this);
        }
    }
}
