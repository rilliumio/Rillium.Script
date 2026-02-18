namespace Rillium.Script.Expressions
{
    internal abstract class Expression
    {
        public readonly Token Token;

        public Expression(Token token)
        {
            token.ShouldNotBeNull();
            this.Token = token;
        }

        public abstract Expression Evaluate(Scope scope);

        public virtual Task<Expression> EvaluateAsync(Scope scope)
        {
            return Task.FromResult(this.Evaluate(scope));
        }
    }
}
