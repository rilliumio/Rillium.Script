namespace Rillium.Script.Expressions
{
    internal class NumberExpression : Expression
    {
        public double Value { get; private set; }

        public NumberExpression(Token token, double value)
            : base(token)
        {
            this.Value = value;
        }

        public override Expression Evaluate(Scope scope) => this;

        public void Increment() => this.Value++;
        public void Decrement() => this.Value--;
    }
}
