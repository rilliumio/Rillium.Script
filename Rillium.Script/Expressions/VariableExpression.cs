namespace Rillium.Script.Expressions
{
    internal class VariableExpression : Expression
    {
        public Token Name => token;

        public VariableExpression(Token name)
            : base(name) { }


        public override Expression Evaluate(Scope scope) =>
             (Expression)scope.Get(Name);

    }

}
