namespace Rillium.Script
{
    public class VariableExpression : Expression
    {
        public Token Name { get; }

        public VariableExpression(Token name)
        {
            Name = name;
        }
        public override LiteralExpression Evaluate() =>
           throw new NotImplementedException();

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.VisitVariableExpression(this);
        }
    }

}
