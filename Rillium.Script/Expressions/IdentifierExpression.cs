namespace Rillium.Script.Expressions
{
    internal class IdentifierExpression : Expression
    {
        public string Name => Token.Value;

        public IdentifierExpression(Token token)
            : base(token)
        {
        }

        public override Expression Evaluate(Scope scope)
        {
            if (scope.TryGet(Token.Value, out var o) && o != null)
            {
                if (o is NumberExpression numberExpression) { return numberExpression; }
            };

            return this;
        }
    }
}
