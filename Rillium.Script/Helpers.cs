using Rillium.Script.Expressions;

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

        public static double EvaluateToDouble(this Expression ex, Scope scope)
        {
            var e = ex;
            while (true)
            {
                if (e is NumberExpression ne)
                {
                    return ne.Value;
                }

                if (e is LiteralExpression le)
                {
                    return (double)le.Value.Value;
                }

                e = ex.Evaluate(scope);
            }
        }

        public static object EvaluateToType(this Expression ex, LiteralTypeId literalTypeId, Scope scope)
        {
            switch (literalTypeId)
            {
                case LiteralTypeId.Number: return EvaluateToDouble(ex, scope);
                default: throw new NotImplementedException($"Expression conversion to {nameof(literalTypeId)} '{literalTypeId}' not implemented.");
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
