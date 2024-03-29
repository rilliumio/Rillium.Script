﻿namespace Rillium.Script.Expressions
{
    internal class IdentifierExpression : Expression
    {
        public string Name => this.Token.Value;
        private readonly TokenId nextTokenId;

        public IdentifierExpression(Token token, TokenId nextTokenId)
            : base(token)
        {
            this.nextTokenId = nextTokenId;
        }

        public override Expression Evaluate(Scope scope)
        {
            if (scope.TryGet(this.Token.Value, out var scopeExpression) && scopeExpression != null)
            {
                if (scopeExpression is NumberExpression numberExpression)
                {
                    if (this.Token.PreToken == TokenId.PlusPlus)
                    {
                        numberExpression.Increment();
                        return numberExpression;
                    }

                    if (this.Token.PreToken == TokenId.MinusMinus)
                    {
                        numberExpression.Decrement();
                        return numberExpression;
                    }


                    if (this.nextTokenId == TokenId.PlusPlus)
                    {
                        var copy = new NumberExpression(numberExpression.Token, numberExpression.Value);
                        numberExpression.Increment();
                        return copy;
                    }

                    if (this.nextTokenId == TokenId.MinusMinus)
                    {
                        var copy = new NumberExpression(numberExpression.Token, numberExpression.Value);
                        numberExpression.Decrement();
                        return copy;
                    }

                    return numberExpression;
                }

                if (scopeExpression is LiteralExpression literalExpression
                    && literalExpression.Value.TypeId != LiteralTypeId.UnAssigned)
                {
                    return literalExpression;
                }
            };

            return this;
        }
    }
}
