namespace Rillium.Script.Expressions
{
    internal class IdentifierExpression : Expression
    {
        public string Name => token.Value;

        public IdentifierExpression(Token token)
            : base(token)
        {
        }

        public override Expression Evaluate(Scope scope)
        {
            if (scope.TryGet(token.Value, out var o) && o != null)
            {
                if (o is NumberExpression numberExpression) { return numberExpression; }
            };

            return this;
        }
    }
}
