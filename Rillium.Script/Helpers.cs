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
            return tokenId switch
            {
                TokenId.True or TokenId.False => LiteralTypeId.Bool,
                TokenId.Number => LiteralTypeId.Number,
                TokenId.String => LiteralTypeId.String,
                _ => throw new ArgumentException(
                                    $"Invalid token {tokenId}. Cannot " +
                                    $"convert to {nameof(LiteralTypeId)}."),
            };
        }

        public static bool EvaluateToBool(this Expression ex, Scope scope)
        {
            var e = ex;
            while (true)
            {
                if (e is LiteralExpression le && le.Value.TypeId == LiteralTypeId.Bool)
                {
                    return le.Value.Value is true;
                }

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

                if (e is LiteralExpression literalExpression)
                {
                    if (literalExpression.Value.TypeId == LiteralTypeId.String)
                    {
                        throw new ScriptException($"Line {ex.Token.Line + 1}. Cannot convert type {literalExpression.Value.TypeId} to double.");
                    }
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

        public static void InitializeScopeArguments(this Scope scope, object[]? args)
        {
            if (args == null) { return; }

            var token = new Token(TokenId.Identifier, Constants.EntryArgumentParameterName, 0);
            scope.Set(
                Constants.EntryArgumentParameterName,
                new ArrayExpression(token, ToArrayExpression(args)));
        }

        private static List<Expression> ToArrayExpression(object[] args)
        {
            args.ShouldNotBeNull();
            return args.Select(x => LiteralArgToExpression(x)).ToList();
        }

        private static Expression LiteralArgToExpression(object arg)
        {
            if (arg is bool b) { return BuildLiteralExpression(new Token(b ? TokenId.True : TokenId.False, null, 0), LiteralTypeId.Bool, b); }

            if (arg is byte bt) { return new NumberExpression(new Token(TokenId.Number, null, 0), bt); }

            if (arg is int i) { return new NumberExpression(new Token(TokenId.Number, null, 0), i); }
            if (arg is short s) { return new NumberExpression(new Token(TokenId.Number, null, 0), s); }
            if (arg is double d) { return new NumberExpression(new Token(TokenId.Number, null, 0), d); }
            if (arg is long l) { return new NumberExpression(new Token(TokenId.Number, null, 0), l); }
            if (arg is decimal dec) { return new NumberExpression(new Token(TokenId.Number, null, 0), (double)dec); }

            if (arg is string str) { return BuildLiteralExpression(new Token(TokenId.String, null, 0), LiteralTypeId.String, str); }

            throw new ArgumentException($"Argument type of '{arg.GetType().Name}' not supported.");
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
