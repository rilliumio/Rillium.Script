namespace Rillium.Script.Statements
{
    internal class BlockStatement : Statement
    {
        public List<Statement> Statements { get; }

        public BlockStatement(List<Statement> statements)
        {
            Statements = statements;
        }

        public override void Execute(Scope scope)
        {
            foreach (var statement in Statements)
            {
                statement.Execute(scope);
            }
        }

        public override async Task ExecuteAsync(Scope scope)
        {
            foreach (var statement in Statements)
            {
                await statement.ExecuteAsync(scope);
            }
        }
    }
}
