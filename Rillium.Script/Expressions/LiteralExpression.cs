namespace Rillium.Script.Expressions
{
    internal class LiteralExpression : Expression
    {
        public LiteralValue Value { get; }

        public LiteralExpression(Token token, LiteralValue value)
            : base(token)
        {
            this.Value = value;
        }

        public override Expression Evaluate(Scope scope)
        {
            switch (this.Value.TypeId)
            {
                case LiteralTypeId.Bool:
                case LiteralTypeId.String:
                case LiteralTypeId.UnAssigned:
                    return this;
                default:
                    throw new InvalidOperationException(
                        $"Line {this.Token.Line + 1}. Could not evaluate literal expression with value type '{this.Value.TypeId}'.");
            }
        }

        public void ShouldNotBeUnassigned()
        {
            if (this.Value.TypeId == LiteralTypeId.UnAssigned)
            {
                this.Token.ThrowScriptException<ScriptException>(string.Format(Constants.ExceptionMessages.UnassignedLocalVariable, this.Token.Value));
            }
        }
    }
}
