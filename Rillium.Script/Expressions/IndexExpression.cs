namespace Rillium.Script.Expressions
{
    internal class IndexExpression : Expression
    {
        private readonly VariableExpression arrayVariable;
        private readonly Expression indexExpression;

        public IndexExpression(
            Token token,
            VariableExpression arrayVariableExpression,
            Expression indexExpression)
            : base(token)
        {
            this.arrayVariable = arrayVariableExpression;
            this.indexExpression = indexExpression;
        }

        public override Expression Evaluate(Scope scope)
        {
            if (this.arrayVariable.Evaluate(scope) is not ArrayExpression a)
            {
                throw new ScriptException(
                    $"Cannot apply indexing with [] on " +
                    $"variable '{this.arrayVariable.Name.Value}' of type " +
                    $"'{this.arrayVariable.Name.Id}'. Line number: {this.Token.Line}.");
            }

            if (this.indexExpression.Evaluate(scope) is not NumberExpression i)
            {
                throw new ScriptException(
                   $"Invalid indexing value. Expected a number.");
            }

            return a.Value[(int)i.Value];
        }
    }
}
