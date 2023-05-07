﻿namespace Rillium.Script
{
    public class BlockStatement : Statement
    {
        public List<Statement> Statements { get; }

        public BlockStatement(List<Statement> statements)
        {
            Statements = statements;
        }

        public override T Accept<T>(IStatementVisitor<T> visitor)
        {
            return visitor.VisitBlockStatement(this);
        }
    }

}
