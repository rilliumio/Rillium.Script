using Rillium.Script.Expressions;

namespace Rillium.Script.Statements
{
    internal class ReturnStatement : Statement
    {
        private readonly Expression value;

        public ReturnStatement(Expression value)
        {
            this.value = value;
        }

        public Expression EvaluateReturnExpression(Scope scope)
        {
            var expression = this.value.Evaluate(scope);

            if (expression is LiteralExpression literalExpression)
            {
                if (literalExpression.Value.TypeId == LiteralTypeId.UnAssigned)
                {
                    literalExpression.ThrowScriptException<ScriptException>(
                        string.Format(Constants.ExceptionMessages.UnassignedLocalVariable, expression.Token.Value));
                }
            }

            return expression;
        }

        public override void Execute(Scope scope) =>
            throw new NotImplementedException();
    }
}
