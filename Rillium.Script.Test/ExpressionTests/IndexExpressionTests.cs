using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rillium.Script.Expressions;

namespace Rillium.Script.Test.ExpressionTests
{
    [TestClass]
    public class IndexExpressionTests
    {
        [TestMethod]
        public void BadLiteralTypeIdShouldThrow()
        {
            var expectedMessage = $"Line 1. Invalid array index value.";

            var arrayVariableToken = new Token(TokenId.Identifier, "x", 0);
            var literalValueToken = new Token(TokenId.Number, "1", 0);
            var unexpectedIndexingToken = new Token(TokenId.String, "foo", 0);
            var scope = new Scope(new FunctionTable());

            var ex = new ArrayExpression(
                  arrayVariableToken,
                  new List<Expression>() { literalValueToken.BuildLiteralExpression() });

            scope.Set(arrayVariableToken.Value!, ex);

            var testedExpression = new IndexExpression(
                new Token(TokenId.If, null, 0),
                new VariableExpression(arrayVariableToken),
                unexpectedIndexingToken.BuildLiteralExpression());

            testedExpression.ShouldThrowWithMessage<ScriptException>(scope, expectedMessage);
        }
    }
}
