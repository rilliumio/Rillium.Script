﻿namespace Rillium.Script
{
    public class DeclarationStatement : Statement
    {
        public Token Identifier { get; }
        public Expression Initializer { get; }

        public DeclarationStatement(
            Token identifier,
            Expression initializer)
        {
            Identifier = identifier;
            Initializer = initializer;
        }

        public override void Execute(Scope scope)
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
