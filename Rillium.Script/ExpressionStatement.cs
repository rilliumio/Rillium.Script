namespace Rillium.Script
{
    public class ExpressionStatement : Statement
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

            // TODO: Should this be supported?
            // example: var x = 1; x; where this handles the final x;
            if (e is VariableExpression ve)
            {
                var ev = ve.Evaluate(scope);
                if (ev is NumberExpression ne1)
                {
                    scope.Set(Constants.OutputValueKey, ne1.Value);
                    return;
                }


                if (ev is ArrayExpression aa0)
                {
                    scope.Set(Constants.OutputValueKey, GetValues(aa0, scope));
                }
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

            throw new NotImplementedException("Not handled");
        }

        private static List<object> GetValues(ArrayExpression aa, Scope scope)
        {
            var values = new List<object>();
            scope.Set(Constants.OutputValueKey, values);
            foreach (var v in aa.Value)
            {
                var r = v.Evaluate(scope);

                if (r is VariableExpression ve)
                {
                    var ev = ve.Evaluate(scope);
                    if (ev is NumberExpression ne1)
                    {
                        values.Add(ne1.Value);
                        continue;
                    }
                }

                if (r is NumberExpression ne0)
                {
                    values.Add(ne0.Value);
                    continue;
                }

                throw new NotImplementedException("Could not evaluate array expression.");
            }

            return values;
        }
    }
}
