namespace Rillium.Script
{
    public interface IStatementVisitor<T>
    {
        T VisitBlockStatement(BlockStatement statement);
        T VisitExpression(Expression expression);
        T VisitExpressionStatement(ExpressionStatement statement);
        T VisitForLoopStatement(ForLoopStatement statement);
        T VisitIfStatement(IfStatement statement);
        T VisitDeclarationStatement(DeclarationStatement statement);
        T VisitVariableExpression(VariableExpression variableExpression);

        // Add additional methods for each concrete subclass of Statement
    }

    public class StatementVisitor<T> : IStatementVisitor<T>, IExpressionVisitor<T>
    {
        public T VisitBlockStatement(BlockStatement statement)
        {
            // Default behavior for BlockStatement
            return default(T);
        }

        public T VisitExpression(Expression expression)
        {
            // Default behavior for expression statements
            return default(T);
        }

        public T VisitExpressionStatement(ExpressionStatement statement)
        {
            // Default behavior for ForLoopStatement
            return default(T);
        }

        public T VisitForLoopStatement(ForLoopStatement statement)
        {
            // Default behavior for ForLoopStatement
            return default(T);
        }

        public T VisitIfStatement(IfStatement statement)
        {
            statement.Condition.Accept(this); // Visit the condition expression
            statement.ThenStatement.Accept(this); // Visit the then branch statement
            if (statement.ElseStatement != null)
            {
                statement.ElseStatement.Accept(this); // Visit the else branch statement, if it exists
            }
            return default(T);
        }

        public T VisitDeclarationStatement(DeclarationStatement statement)
        {
            // Default behavior for IfStatement
            return default(T);
        }

        public T VisitBinaryExpression(BinaryExpression expression)
        {
            throw new NotImplementedException();
        }

        public T VisitUnaryExpression(System.Linq.Expressions.UnaryExpression expression)
        {
            throw new NotImplementedException();
        }

        public T VisitLiteralExpression(LiteralExpression expression)
        {
            throw new NotImplementedException();
        }

        public T VisitIdentifierExpression(IdentifierExpression expression)
        {
            throw new NotImplementedException();
        }

        public T VisitAssignmentExpression(AssignmentExpression expression)
        {
            throw new NotImplementedException();
        }

        public T VisitNumberExpression(NumberExpression expression)
        {
            throw new NotImplementedException();
        }

        public T VisitVariableExpression(VariableExpression variableExpression)
        {
            return default(T);
        }

        // Add additional methods for each concrete subclass of Statement
    }
}
