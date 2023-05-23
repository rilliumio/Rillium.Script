using System.Diagnostics.CodeAnalysis;

namespace Rillium.Script.Expressions
{
    public abstract class Expression
    {
        protected readonly Token token;

        public Expression(Token token)
        {
            this.token = token;
        }

        public abstract Expression Evaluate(Scope scope);

        [DoesNotReturn]
        public void ThrowScriptException<T>(string message) where T : ScriptException
        {
            token.ThrowScriptException<T>(message);
        }
    }
}
