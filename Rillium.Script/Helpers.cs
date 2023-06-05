using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Rillium.Script.Expressions;

namespace Rillium.Script
{
    internal static class Helpers
    {
        public static LiteralExpression BuildLiteralExpression(this Token token)
        {
            var typeId = Convert(token.Id);
            return new LiteralExpression(
                token,
                new LiteralValue()
                {
                    TypeId = typeId,
                    Value = typeId == LiteralTypeId.Bool ? token.Id == TokenId.True : token.Value,
                });
        }

        public static LiteralExpression BuildLiteralExpression(this Token token, LiteralTypeId id, object? v)
        {
            return new LiteralExpression(token, new LiteralValue() { TypeId = id, Value = v });
        }

        private static LiteralTypeId Convert(TokenId tokenId)
        {
            switch (tokenId)
            {
                case TokenId.True:
                case TokenId.False:
                    return LiteralTypeId.Bool;

                case TokenId.Number:
                    return LiteralTypeId.Number;

                case TokenId.String:
                    return LiteralTypeId.String;

                default:
                    throw new ArgumentException(
                    $"Invalid token {tokenId}. Cannot " +
                    $"convert to {nameof(LiteralTypeId)}.");
            }
        }

        public static bool EvaluateToBool(this Expression ex, Scope scope)
        {
            var e = ex;
            while (true)
            {
                if (e is NumberExpression ne) { return IsTrue(ne.Value); }

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

        public static void ShouldNotBeNull(
            [NotNull] this object? value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value == null)
            {
                throw new NullReferenceException($"'{paramName}' should not be null.");
            }
        }

        private static bool IsTrue(object? value)
        {
            if (value == null) { return false; }
            if (value is bool b) { return b; }
            if (value is double d) { return d != 0; }

            throw new ScriptException($"Could not evaluate bool from '{value}'.");
        }
    }
}
