namespace Rillium.Script.Expressions
{
    internal class ArraySummaryExpression : Expression
    {
        private readonly Expression array;
        private readonly ArraySummaryId arraySummaryId;

        public ArraySummaryExpression(Token token, Expression array, ArraySummaryId arraySummaryId)
            : base(token)
        {
            this.array = array;
            this.arraySummaryId = arraySummaryId;
        }

        public override Expression Evaluate(Scope scope)
        {
            var e = this.array.Evaluate(scope);
            if (e is not ArrayExpression ae)
            {
                throw new ScriptException(
                    $"Line {this.Token.Line + 1}. Expected array.");
            }

            switch (this.arraySummaryId)
            {
                case ArraySummaryId.Length:
                    return new NumberExpression(this.Token, ae.Value.Count);

                case ArraySummaryId.Sum:
                    return new NumberExpression(this.Token, this.GetList(ae, scope).Sum());

                case ArraySummaryId.Max:
                    return new NumberExpression(this.Token, this.GetList(ae, scope).Max());

                case ArraySummaryId.Min:
                    return new NumberExpression(this.Token, this.GetList(ae, scope).Min());

                case ArraySummaryId.Average:
                    return new NumberExpression(this.Token, this.GetList(ae, scope).Average());

                default: throw new ScriptException($"Line {this.Token.Line + 1}. Invalid aggregate identifier '{this.arraySummaryId}'.");
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
                    throw new ScriptException(
                        $"Line {this.Token.Line + 1}. Could not " +
                        $"perform aggregate on non numeric array.");
                }

                m.Add(ne.Value);
            }

            return m;
        }
    }
}
