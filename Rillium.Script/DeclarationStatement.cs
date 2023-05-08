namespace Rillium.Script
{
    public class DeclarationStatement : Statement
    {
        public Scope scope { get; }
        public Token Identifier { get; }
        public Expression Initializer { get; }

        public DeclarationStatement(
            Scope scope,
            Token identifier,
            Expression initializer)
        {
            this.scope = scope;
            Identifier = identifier;
            Initializer = initializer;
        }

        public override void Execute()
        {
            // Evaluate the initializer expression, if present
            var value = Initializer != null ? Initializer.EvaluateToLiteral() : null;

            // Add the variable to the current scope
            scope.Set(Identifier.Value, value);
        }

        public override T Accept<T>(IStatementVisitor<T> visitor)
        {
            return visitor.VisitDeclarationStatement(this);
        }
    }
}
