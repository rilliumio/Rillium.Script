namespace Rillium.Script.Expressions
{
    internal class ArraySummaryExpression : Expression
    {
        private Expression array;
        private ArraySummaryId arraySummaryId;

        public ArraySummaryExpression(Token token, Expression array, ArraySummaryId arraySummaryId)
            : base(token)
        {
            this.array = array;
            this.arraySummaryId = arraySummaryId;
        }

        public override Expression Evaluate(Scope scope)
        {
            var e = array.Evaluate(scope);
            if (e is not ArrayExpression ae)
            {
                throw new ArgumentException("Expected array expression.");
            }

            switch (arraySummaryId)
            {
                case ArraySummaryId.Length:
                    return new NumberExpression(token, ae.Value.Count);

                case ArraySummaryId.Sum:
                    return new NumberExpression(token, GetList(ae, scope).Sum());

                case ArraySummaryId.Max:
                    return new NumberExpression(token, GetList(ae, scope).Max());

                case ArraySummaryId.Min:
                    return new NumberExpression(token, GetList(ae, scope).Min());

                case ArraySummaryId.Average:
                    return new NumberExpression(token, GetList(ae, scope).Average());

                default: throw new NotImplementedException($"Invalid aggregate identifier '{arraySummaryId}'.");
            }
        }

        private List<double> GetList(ArrayExpression ex, Scope scope)
        {
            var m = new List<double>();
            foreach (var e in ex.Value)
            {
                var r = e.Evaluate(scope);
                if (r is not NumberExpression ne)
                {
                    throw new ArgumentException("Could not perform aggregate on non numeric array.");
                }

                m.Add(ne.Value);
            }

            return m;
        }
    }
}
