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

        public LiteralExpression TryReduce()
        {
            if (!(Left is LiteralExpression ll && Right is LiteralExpression lr)) { return null; }

            return new LiteralExpression(Eval(ll, lr));
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

        private static double ToDouble(object value)
        {
            if (value is double d) { return d; }
            if (value is string s) return double.Parse(s);
            throw new Exception($"Could not evaluate {value.GetType()} to number.");
        }
    }
}
