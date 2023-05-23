namespace Rillium.Script.Expressions
{
    public class IndexExpression : Expression
    {
        private VariableExpression arrayVariable;
        private Expression indexExpression;

        public IndexExpression(
            Token token,
            VariableExpression arrayVariableExpression,
            Expression indexExpression)
            : base(token)
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
                    $"'{arrayVariable.Name.Id}'. Line number: {token.Line}.");
            }

            if (indexExpression.Evaluate(scope) is not NumberExpression i)
            {
                throw new ArgumentException(
                   $"Invalid indexing value. Expected a number.");
            }

            return a.Value[(int)i.Value];
        }
    }
}
