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
    }
}
