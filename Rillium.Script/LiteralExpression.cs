﻿namespace Rillium.Script
{
    public class LiteralExpression : Expression
    {
        public LiteralValue Value { get; }

        public LiteralExpression(LiteralValue value)
        {
            Value = value;
        }

        public override Expression Evaluate()
        {
            switch (Value.TypeId)
            {
                case LiteralTypeId.String:
                    return this;
                case LiteralTypeId.Number:
                    if (Value.Value is double d)
                    {
                        return new NumberExpression(d);
                    }

                    if (Value.Value is string s)
                    {
                        return new NumberExpression(double.Parse(s));
                    }
                    throw new InvalidOperationException();
                default:
                    throw new InvalidOperationException(
                        $"Could not evaluate literal expression with value type '{Value.TypeId}'.");
            }
        }

        public bool IsTrue()
        {
            if (Value?.Value == null) { return false; }
            var v = Value.Value;
            if (v is bool b) { return b; }
            if (v is string s) { return s == "true"; }
            if (v is double d) { return d == 1; }

            throw new ArgumentException($"Could not evaluate bool from '{Value.Value}'.");
        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.VisitLiteralExpression(this);
        }
    }
}
