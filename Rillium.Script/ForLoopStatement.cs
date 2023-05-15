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

        public override void Execute(Scope scope)
        {
            // Evaluate the initial expression
            Initialization.Execute(scope);

            // Loop while the condition is true
            while (Condition.EvaluateToBool(scope))
            {
                // Execute the loop body
                Body.Execute(scope);

                // Evaluate the increment expression
                Iteration.Execute(scope);
            }
        }

        public override T Accept<T>(IStatementVisitor<T> visitor)
        {
            return visitor.VisitForLoopStatement(this);
        }
    }

}
