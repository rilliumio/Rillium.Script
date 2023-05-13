namespace Rillium.Script
{
    public class ReturnStatement : Statement
    {
        private Expression value;

        public ReturnStatement(Expression value)
        {
            this.value = value;
        }

        public override T Accept<T>(IStatementVisitor<T> visitor)
        {
            throw new NotImplementedException();
        }

        public object EvaluateReturnExpression() =>
            value.Evaluate();

        public override void Execute(Scope scope) =>
            throw new NotImplementedException();
    }
}
