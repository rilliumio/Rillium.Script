namespace Rillium.Script.Expressions
{
    internal class LiteralExpression : Expression
    {
        public LiteralValue Value { get; }

        public LiteralExpression(Token token, LiteralValue value)
            : base(token)
        {
            Value = value;
        }

        public override Expression Evaluate(Scope scope)
        {
            switch (Value.TypeId)
            {
                case LiteralTypeId.String:
                case LiteralTypeId.Unknown:
                    return this;
                default:
                    throw new InvalidOperationException(
                        $"Could not evaluate literal expression with value type '{Value.TypeId}'.");
            }
        }
    }
}
