namespace Rillium.Script.Expressions
{
    internal class TernaryExpression : Expression
    {
        public Expression Condition { get; }
        public Expression Left { get; set; }
        public Expression Right { get; set; }

        public TernaryExpression(Token token, Expression condition, Expression left, Expression right)
            : base(token)
        {
            condition.ShouldNotBeNull();
            left.ShouldNotBeNull();
            right.ShouldNotBeNull();

            this.Condition = condition;
            this.Left = left;
            this.Right = right;
        }

        public override Expression Evaluate(Scope scope)
        {
            var con = this.Condition.Evaluate(scope);
            return (con.EvaluateToBool(scope)) ? this.Left.Evaluate(scope) : this.Right.Evaluate(scope);
        }

        public override async Task<Expression> EvaluateAsync(Scope scope)
        {
            var con = await this.Condition.EvaluateAsync(scope);
            return (con.EvaluateToBool(scope))
                ? await this.Left.EvaluateAsync(scope)
                : await this.Right.EvaluateAsync(scope);
        }
    }
}
