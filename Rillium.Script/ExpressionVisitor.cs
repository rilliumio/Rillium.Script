using System.Linq.Expressions;

namespace Rillium.Script
{
    public interface IExpressionVisitor<T>
    {
        T VisitBinaryExpression(BinaryExpression expression);
        T VisitUnaryExpression(UnaryExpression expression);
        T VisitLiteralExpression(LiteralExpression expression);
        T VisitIdentifierExpression(IdentifierExpression expression);
        T VisitAssignmentExpression(AssignmentExpression expression);

        T VisitNumberExpression(NumberExpression expression);

        T VisitVariableExpression(VariableExpression expression);

        T VisitArrayDoubleExpression(ArrayExpression expression);
    }
}
