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
            Left = left;
            Right = right;
        }

        public override Expression Evaluate(Scope scope)
        {
            var left = Left.Evaluate(scope);
            var right = Right.Evaluate(scope);

            if (left is NumberExpression len && right is NumberExpression lern)
            {
                return Eval(len, lern);
            }

            if (left is IdentifierExpression ie)
            {
                ThrowScriptException<BadNameException>(
                    string.Format(Constants.ExceptionMessages.NameDoesNotExist, ie.Name));
            }

            throw new ScriptException("Invalid binary expression.");
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
                    throw new ScriptException(
                    $"Line: {token.Line}. Invalid binary operator '{token.Id}'.");
            }
        }
    }
}
