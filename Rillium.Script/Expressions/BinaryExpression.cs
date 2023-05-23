using Rillium.Script.Exceptions;

namespace Rillium.Script.Expressions
{
    public class BinaryExpression : Expression
    {
        public Expression Left { get; }
        public Expression Right { get; }

        public BinaryExpression(Expression left, Token token, Expression right)
            : base(token)
        {
            Left = left;
            Right = right;
        }

        public override Expression Evaluate(Scope scope)
        {
            var left = Left.Evaluate(scope);
            var right = Right.Evaluate(scope);

            if (left is LiteralExpression le && right is LiteralExpression ler)
            {
                return new LiteralExpression(token, Eval(le, ler));
            }

            if (left is NumberExpression len && right is NumberExpression lern)
            {
                return Eval(len, lern);
            }

            if (left is IdentifierExpression ie)
            {
                ThrowScriptException<BadNameException>(
                    string.Format(Constants.ExceptionMessages.NameDoesNotExist, ie.Name));
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

                switch (token.Id)
                {
                    case TokenId.Plus: lvd.Value = l + r; return lvd;
                    case TokenId.Minus: lvd.Value = l - r; return lvd;
                    case TokenId.Star: lvd.Value = l * r; return lvd;
                    case TokenId.Slash: lvd.Value = l / r; return lvd;
                    case TokenId.EqualEqual: lvd.Value = l == r; return lvd;
                    // Handle other operators
                    default:
                        throw new Exception($"Unsupported operator: {token.Id}. Line: {token.Line}");
                }
            }

            lvd.TypeId = LiteralTypeId.String;
            switch (token.Id)
            {
                case TokenId.Plus:
                    lvd.Value = ll.Value.ToString() + lr.Value.ToString();
                    return lvd;
                default:
                    throw new Exception($"Unsupported operator: . Line: {token.Line}");
            }
        }


        private NumberExpression Eval(NumberExpression ll, NumberExpression lr) =>
            new NumberExpression(token, Ev(ll.Value, lr.Value));

        private double Ev(double l, double r)
        {
            switch (token.Id)
            {
                case TokenId.Plus: return l + r;
                case TokenId.Minus: return l - r;
                case TokenId.Star: return l * r;
                case TokenId.Slash: return l / r;
                case TokenId.EqualEqual: return l == r ? 1 : 0;
                case TokenId.Less: return l < r ? 1 : 0;
                case TokenId.LessEqual: return l <= r ? 1 : 0;
                case TokenId.Greater: return l > r ? 1 : 0;
                case TokenId.GreaterEqual: return l >= r ? 1 : 0;
                default:
                    throw new Exception($"Line: {token.Line}. Invalid binary operator '{token.Id}'.");
            }
        }

        private double ToDouble(object value)
        {
            if (value is double d) { return d; }
            if (value is string s) return double.Parse(s);
            throw new Exception($"Could not evaluate {value.GetType()} to number. Line: {token.Line}");
        }
    }
}
