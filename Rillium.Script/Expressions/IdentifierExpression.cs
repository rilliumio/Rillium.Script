namespace Rillium.Script.Expressions
{
    public class IdentifierExpression : Expression
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
                if (o is ArrayExpression arrayExpression) { return arrayExpression; };
                ;
                throw new ArgumentException($"could not evaluate '{token.Value}'. Line number: {token.Line}.");
            };

            return this;
        }
    }
}
