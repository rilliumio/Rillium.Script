using Rillium.Script.Exceptions;
using Rillium.Script.Expressions;

namespace Rillium.Script.Statements
{
    internal class ExpressionStatement : Statement
    {
        public Expression Expression { get; }

        public ExpressionStatement(Expression expression)
        {
            this.Expression = expression;
        }

        public override void Execute(Scope scope)
        {
            var e = this.Expression.Evaluate(scope);
            this.HandleEvaluated(e, scope);
        }

        public override async Task ExecuteAsync(Scope scope)
        {
            var e = await this.Expression.EvaluateAsync(scope);

            if (e is AssignmentExpression ae)
            {
                await ae.SetAsync(scope);
                return;
            }

            this.HandleEvaluatedNonAssignment(e, scope);
        }

        private void HandleEvaluated(Expression e, Scope scope)
        {
            if (e is AssignmentExpression ae)
            {
                ae.Set(scope);
                return;
            }

            this.HandleEvaluatedNonAssignment(e, scope);
        }

        private void HandleEvaluatedNonAssignment(Expression e, Scope scope)
        {
            if (e is NumberExpression ne)
            {
                scope.Set(Constants.OutputValueKey, ne.Value);
                return;
            }

            if (e is LiteralExpression le)
            {
                le.ShouldNotBeUnassigned();

                scope.Set(Constants.OutputValueKey, le.Value.Value);
                return;
            }

            if (e is ArrayExpression aa)
            {
                scope.Set(Constants.OutputValueKey, GetValues(aa, scope));
                return;
            }

            if (e is IdentifierExpression ie)
            {
                throw new BadNameException(
                    $"Line {ie.Token.Line + 1}. {string.Format(Constants.ExceptionMessages.NameDoesNotExist, ie.Name)}");
            }

            throw new ScriptException(
                  $"Line {e.Token.Line + 1}. " +
                  $"Expression type {e.GetType().Name} not handled.");
        }

        private static List<object> GetValues(ArrayExpression aa, Scope scope)
        {
            var values = new List<object>();
            scope.Set(Constants.OutputValueKey, values);
            foreach (var v in aa.Value)
            {
                var r = v.Evaluate(scope);

                if (r is NumberExpression ne0)
                {
                    values.Add(ne0.Value);
                    continue;
                }

                throw new ScriptException("Could not evaluate array expression.");
            }

            return values;
        }
    }
}
