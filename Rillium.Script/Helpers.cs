namespace Rillium.Script
{
    internal static class Helpers
    {
        public static bool EvaluateToBool(this Expression ex, Scope scope)
        {
            var e = ex;
            while (true)
            {
                if (e is NumberExpression ne)
                {
                    return IsTrue(ne.Value);
                }
                if (e is LiteralExpression le)
                {
                    return IsTrue(le.Value);
                }

                e = ex.Evaluate(scope);
            }
        }

        public static ArraySummaryId GetArraySummaryId(this Token token)
        {
            return (ArraySummaryId)Enum.Parse(typeof(ArraySummaryId), token.Value);
        }

        private static bool IsTrue(LiteralValue literalValue) => IsTrue(literalValue.Value);

        private static bool IsTrue(object? value)
        {
            if (value == null) { return false; }
            if (value is bool b) { return b; }
            if (value is double d) { return d != 0; }

            throw new ArgumentException($"Could not evaluate bool from '{value}'.");
        }
    }
}
