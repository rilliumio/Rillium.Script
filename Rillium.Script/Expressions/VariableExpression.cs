namespace Rillium.Script.Expressions
{
    internal class VariableExpression : Expression
    {
        public Token Name => this.Token;

        public VariableExpression(Token name)
            : base(name)
        {

        }

        public override Expression Evaluate(Scope scope) =>
            (Expression)scope.Get(this.Token);
    }

}
