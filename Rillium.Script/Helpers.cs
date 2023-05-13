﻿namespace Rillium.Script
{
    internal static class Helpers
    {
        public static LiteralExpression EvaluateToLiteral(this Expression ex)
        {
            var e = ex;
            while (true)
            {
                if (e is LiteralExpression le) { return le; }
                e = ex.Evaluate();
            }
        }

        public static NumberExpression EvaluateToNumber(this Expression ex)
        {
            var e = ex;
            while (true)
            {
                // TODO: A literal can be a number so an expression conversion might be needed.
                if (e is NumberExpression ne) { return ne; }
                e = ex.Evaluate();
            }
        }

        public static bool EvaluateToBool(this Expression ex)
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

                e = ex.Evaluate();
            }
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
