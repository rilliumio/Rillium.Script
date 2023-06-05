namespace Rillium.Script.Expressions
{
    internal class NumberExpression : Expression
    {
        public double Value { get; }

        public NumberExpression(Token token, double value)
            : base(token)
        {
            Value = value;
        }

        public override Expression Evaluate(Scope scope) => this;

    }
}
