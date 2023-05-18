namespace Rillium.Script
{
    public class ReturnStatement : Statement
    {
        private Expression value;

        public ReturnStatement(Expression value)
        {
            this.value = value;
        }

        public object EvaluateReturnExpression(Scope scope) =>
            value.Evaluate(scope);

        public override void Execute(Scope scope) =>
            throw new NotImplementedException();
    }
}
