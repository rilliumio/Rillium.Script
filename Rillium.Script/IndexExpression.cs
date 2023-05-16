namespace Rillium.Script
{
    public class IndexExpression : Expression
    {
        private VariableExpression arrayVariable;
        private Expression indexExpression;

        public IndexExpression(VariableExpression arrayVariableExpression, Expression indexExpression)
        {
            arrayVariable = arrayVariableExpression;
            this.indexExpression = indexExpression;
        }

        public override Expression Evaluate(Scope scope)
        {
            if (arrayVariable.Evaluate(scope) is not ArrayExpression a)
            {
                throw new ArgumentException(
                    $"Cannot apply indexing with [] on " +
                    $"variable '{arrayVariable.Name.Value}' of type " +
                    $"'{arrayVariable.Name.Type}'.");
            }

            if (indexExpression.Evaluate(scope) is not NumberExpression i)
            {
                throw new ArgumentException(
                   $"Invalid indexing value. Expected a number.");
            }

            return a.Value[(int)i.Value];
        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.VisitIndexExpression(this);
        }
    }
}
