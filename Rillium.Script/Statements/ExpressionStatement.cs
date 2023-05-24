using Rillium.Script.Exceptions;
using Rillium.Script.Expressions;

namespace Rillium.Script.Statements
{
    internal class ExpressionStatement : Statement
    {
        public Expression Expression { get; }

        public ExpressionStatement(Expression expression)
        {
            Expression = expression;
        }

        public override void Execute(Scope scope)
        {
            var e = Expression.Evaluate(scope);

            if (e is AssignmentExpression ae)
            {
                ae.Set(scope);
                return;
            }

            if (e is NumberExpression ne)
            {
                scope.Set(Constants.OutputValueKey, ne.Value);
                return;
            }

            if (e is ArrayExpression aa)
            {
                scope.Set(Constants.OutputValueKey, GetValues(aa, scope));
                return;
            }

            if (e is IdentifierExpression ie)
            {
                ie.ThrowScriptException<BadNameException>(
                    string.Format(Constants.ExceptionMessages.NameDoesNotExist, ie.Name));
            }

            e.ThrowScriptException<ScriptException>(
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
