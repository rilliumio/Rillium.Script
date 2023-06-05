using System.Diagnostics.CodeAnalysis;

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

        [DoesNotReturn]
        public void ThrowScriptException<T>(string message) where T : ScriptException
        {
            this.Token.ThrowScriptException<T>(message);
        }
    }
}
