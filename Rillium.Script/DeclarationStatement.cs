namespace Rillium.Script
{
    public class DeclarationStatement : Statement
    {
        public Token Identifier { get; }
        public Expression Initializer { get; }

        public DeclarationStatement(Token identifier, Expression initializer)
        {
            Identifier = identifier;
            Initializer = initializer;
        }

        public override T Accept<T>(IStatementVisitor<T> visitor)
        {
            return visitor.VisitDeclarationStatement(this);
        }
    }
}
