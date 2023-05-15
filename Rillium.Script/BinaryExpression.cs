namespace Rillium.Script
{
    public class BinaryExpression : Expression
    {
        public Expression Left { get; }
        public TokenType Operator { get; }
        public Expression Right { get; }

        public BinaryExpression(Expression left, TokenType @operator, Expression right)
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.VisitBinaryExpression(this);
        }

        public override Expression Evaluate(Scope scope)
        {
            var left = Left.Evaluate(scope);
            var right = Right.Evaluate(scope);

            if (left is LiteralExpression le && right is LiteralExpression ler)
            {
                return new LiteralExpression(Eval(le, ler));
            }

            if (left is NumberExpression len && right is NumberExpression lern)
            {
                return Eval(len, lern);
            }

            throw new ArgumentException("Invalid binary expression.");
        }

        private LiteralValue Eval(LiteralExpression ll, LiteralExpression lr)
        {
            var lvd = new LiteralValue();
            if (ll.Value.TypeId == LiteralTypeId.Number && lr.Value.TypeId == LiteralTypeId.Number)
            {
                var l = ToDouble(ll.Value.Value);
                var r = ToDouble(lr.Value.Value);
                lvd.TypeId = LiteralTypeId.Number;

                switch (Operator)
                {
                    case TokenType.Plus: lvd.Value = l + r; return lvd;
                    case TokenType.Minus: lvd.Value = l - r; return lvd;
                    case TokenType.Star: lvd.Value = l * r; return lvd;
                    case TokenType.Slash: lvd.Value = l / r; return lvd;
                    case TokenType.EqualEqual: lvd.Value = l == r; return lvd;
                    // Handle other operators
                    default:
                        throw new Exception($"Unsupported operator: {Operator}");
                }
            }

            lvd.TypeId = LiteralTypeId.String;
            switch (Operator)
            {
                case TokenType.Plus:
                    lvd.Value = ll.Value.ToString() + lr.Value.ToString();
                    return lvd;
                default:
                    throw new Exception($"Unsupported operator: {Operator}");
            }
        }


        private NumberExpression Eval(NumberExpression ll, NumberExpression lr) =>
            new NumberExpression(Ev(ll.Value, lr.Value));

        private double Ev(double l, double r)
        {
            switch (Operator)
            {
                case TokenType.Plus: return l + r;
                case TokenType.Minus: return l - r;
                case TokenType.Star: return l * r;
                case TokenType.Slash: return l / r;
                case TokenType.EqualEqual: return (l == r) ? 1 : 0;
                case TokenType.Less: return (l < r) ? 1 : 0;
                case TokenType.LessEqual: return (l <= r) ? 1 : 0;
                case TokenType.Greater: return (l > r) ? 1 : 0;
                case TokenType.GreaterEqual: return (l >= r) ? 1 : 0;
                default:
                    throw new Exception($"Unsupported operator: {Operator}");
            }
        }

        private static double ToDouble(object value)
        {
            if (value is double d) { return d; }
            if (value is string s) return double.Parse(s);
            throw new Exception($"Could not evaluate {value.GetType()} to number.");
        }
    }
}
