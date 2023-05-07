namespace Rillium.Script
{
    public abstract class Expression
    {
        public abstract T Accept<T>(IExpressionVisitor<T> visitor);
    }
}
