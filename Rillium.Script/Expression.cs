namespace Rillium.Script
{
    public abstract class Expression
    {
        public abstract Expression Evaluate(Scope scope);

        public abstract T Accept<T>(IExpressionVisitor<T> visitor);

    }
}
