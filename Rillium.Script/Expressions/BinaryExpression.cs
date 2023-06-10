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

            if (left is NumberExpression len)
            {
                if (right is NumberExpression lern)
                {
                    return this.Eval(len, lern);
                }

                if (right is LiteralExpression lr)
                {
                    lr.ShouldNotBeUnassigned();

                    var v = len.Value + (lr.Value?.Value as string);
                    return this.Token.BuildLiteralExpression(LiteralTypeId.String, v);
                }
            }

            if (left is IdentifierExpression ie)
            {
                if (scope.TryGet(ie.Name, out var ee) && ee is LiteralExpression le)
                {
                    le.ShouldNotBeUnassigned();
                }

                throw new BadNameException(
                    $"Line {this.Token.Line + 1}. " +
                    $"{string.Format(Constants.ExceptionMessages.NameDoesNotExist, ie.Name)}");
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

                if (right is NumberExpression nr)
                {
                    var v = (ll.Value?.Value as string) + nr.Value;
                    return this.Token.BuildLiteralExpression(LiteralTypeId.String, v);
                }
            }

            throw new ScriptException($"Line {this.Token.Line + 1}. Invalid binary expression.");
        }

        private NumberExpression Eval(NumberExpression ll, NumberExpression lr) =>
            new(this.Token, this.Ev(ll.Value, lr.Value));

        private double Ev(double l, double r) => this.Token.Id switch
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
            _ => throw new ScriptException(
                                $"Line: {this.Token.Line + 1}. Invalid binary operator '{this.Token.Id}'."),
        };
    }
}
