﻿namespace Rillium.Script.Expressions
{
    internal class ArrayExpression : Expression
    {
        public List<Expression> Value { get; }

        public ArrayExpression(Token token, List<Expression> array)
            : base(token)
        {
            Value = array;
        }

        public override Expression Evaluate(Scope scope) => this;
    }
}
