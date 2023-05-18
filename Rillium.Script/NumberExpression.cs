namespace Rillium.Script
{
    public class NumberExpression : Expression
    {
        public double Value { get; }

        public NumberExpression(double value)
        {
            Value = value;
        }

        public override Expression Evaluate(Scope scope) => this;

    }
}
