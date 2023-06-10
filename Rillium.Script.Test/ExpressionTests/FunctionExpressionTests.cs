using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rillium.Script.Expressions;

namespace Rillium.Script.Test.ExpressionTests
{
    [TestClass]
    public class FunctionExpressionTests
    {
        [TestMethod]
        public void BadLiteralTypeIdShouldThrow()
        {
            var bad = (LiteralTypeId)99;
            var expectedMessage = $"Line 1. Could not evaluate literal expression with value type '{bad}'.";

            var ex = new LiteralExpression(
                new Token(TokenId.If, null, 0),
                new LiteralValue() { TypeId = bad, Value = "" });

            ex.ShouldThrowWithMessage<InvalidOperationException>(expectedMessage);
        }

        [TestMethod]
        public void InvalidArgumentConversionTypeStringToDouble()
        {
            var expectedMessage = $"Line 1. Cannot convert type String to double.";

            var functionToken = new Token(TokenId.Function, "Abs", 0);
            var literalValueToken = new Token(TokenId.String, "1", 0);

            var unexpectedExpressionType = new FunctionExpression(
                functionToken,
                functionToken.Value!,
                new List<Expression>() { literalValueToken.BuildLiteralExpression() });

            unexpectedExpressionType.ShouldThrowWithMessage<ScriptException>(expectedMessage);
        }
    }
}
