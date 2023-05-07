namespace Rillium.Script
{
    public class ForLoopStatement : Statement
    {
        public Statement Initialization { get; }
        public Expression Condition { get; }
        public Statement Iteration { get; }
        public BlockStatement Body { get; }

        public ForLoopStatement(
            Statement initialization,
            Expression condition,
            Statement iteration,
            BlockStatement body)
        {
            Initialization = initialization;
            Condition = condition;
            Iteration = iteration;
            Body = body;
        }

        public override T Accept<T>(IStatementVisitor<T> visitor)
        {
            return visitor.VisitForLoopStatement(this);
        }
    }

}
