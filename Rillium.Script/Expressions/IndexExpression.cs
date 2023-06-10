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
                   $"Line {this.Token.Line + 1}. Invalid array index value.");
            }

            var index = (int)i.Value;
            if (index < 0)
            {
                throw new ScriptException(
                  $"Line {this.Token.Line + 1}. Invalid indexing an array with a " +
                  $"negative number. Array indices should not be less than zero.");
            }

            if (index > a.Value.Count)
            {
                throw new ScriptException(
                  $"Line {this.Token.Line + 1}. Index was outside the bounds of the array.");
            }

            return a.Value[(int)i.Value];
        }
    }
}
