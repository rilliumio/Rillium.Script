using Rillium.Script.Exceptions;

namespace Rillium.Script.Expressions
{
    internal class BinaryExpression : Expression
    {
        public Expression Left { get; }
        public Expression Right { get; }

        public BinaryExpression(Expression left, Token token, Expression right)
            : base(token)
        {
            left.ShouldNotBeNull();
            right.ShouldNotBeNull();

            this.Left = left;
            this.Right = right;
        }

        public override Expression Evaluate(Scope scope)
        {
            var left = this.Left.Evaluate(scope);
            var right = this.Right.Evaluate(scope);
            return this.EvaluateCore(left, right, scope);
        }

        public override async Task<Expression> EvaluateAsync(Scope scope)
        {
            var left = await this.Left.EvaluateAsync(scope);
            var right = await this.Right.EvaluateAsync(scope);
            return this.EvaluateCore(left, right, scope);
        }

        private Expression EvaluateCore(Expression left, Expression right, Scope scope)
        {
            if (left is NumberExpression leftNumberExpression)
            {
                if (right is NumberExpression rightNumberExpression)
                {
                    return this.Eval(leftNumberExpression, rightNumberExpression);
                }

                if (right is LiteralExpression rightLiteralExpression)
                {
                    rightLiteralExpression.ShouldNotBeUnassigned();

                    var v = leftNumberExpression.Value + (rightLiteralExpression.Value?.Value as string);
                    return this.Token.BuildLiteralExpression(LiteralTypeId.String, v);
                }
            }

            if (left is IdentifierExpression leftIdentifierExpression)
            {
                if (scope.TryGet(leftIdentifierExpression.Name, out var leftScopeExpression) && leftScopeExpression is LiteralExpression le)
                {
                    le.ShouldNotBeUnassigned();
                }

                throw new BadNameException(
                    $"Line {this.Token.Line + 1}. " +
                    $"{string.Format(Constants.ExceptionMessages.NameDoesNotExist, leftIdentifierExpression.Name)}");
            }

            if (left is LiteralExpression ll)
            {
                if (right is LiteralExpression lr)
                {
                    if (ll.Value.TypeId == LiteralTypeId.Bool && lr.Value.TypeId == LiteralTypeId.Bool)
                    {
                        if (this.Token.Id == TokenId.EqualEqual)
                        {
                            return this.Token.BuildLiteralExpression(
                                LiteralTypeId.Bool,
                                (bool)ll.Value.Value! == (bool)lr.Value.Value!);
                        }

                        if (this.Token.Id == TokenId.BangEqual)
                        {
                            return this.Token.BuildLiteralExpression(
                                LiteralTypeId.Bool,
                                (bool)ll.Value.Value! != (bool)lr.Value.Value!);
                        }
                    }

                    if (this.Token.Id == TokenId.EqualEqual)
                    {
                        return this.Token.BuildLiteralExpression(
                            LiteralTypeId.Bool,
                            (ll.Value?.Value as string) == (lr.Value?.Value as string));
                    }

                    if (this.Token.Id == TokenId.BangEqual)
                    {
                        return this.Token.BuildLiteralExpression(
                            LiteralTypeId.Bool,
                            (ll.Value?.Value as string) != (lr.Value?.Value as string));
                    }

                    var v = (ll.Value?.Value as string) + lr.Value?.Value;
                    return this.Token.BuildLiteralExpression(LiteralTypeId.String, v);
                }
            }

            throw new ScriptException($"Line {this.Token.Line + 1}. Invalid binary expression.");
        }

        private NumberExpression Eval(NumberExpression ll, NumberExpression lr) =>
            new(this.Token, this.Eval(ll.Value, lr.Value));

        private double Eval(double l, double r) => this.Token.Id switch
        {
            TokenId.Plus => l + r,
            TokenId.Minus => l - r,
            TokenId.Star => l * r,
            TokenId.Slash => l / r,
            TokenId.EqualEqual => l == r ? 1 : 0,
            TokenId.BangEqual => l != r ? 1 : 0,
            TokenId.Less => l < r ? 1 : 0,
            TokenId.LessEqual => l <= r ? 1 : 0,
            TokenId.Greater => l > r ? 1 : 0,
            TokenId.GreaterEqual => l >= r ? 1 : 0,
            TokenId.Percent => l % r,
            _ => throw new ScriptException(
                       $"Line: {this.Token.Line + 1}. Invalid binary operator " +
                       $"'{this.Token.Id}'."),
        };
    }
}
