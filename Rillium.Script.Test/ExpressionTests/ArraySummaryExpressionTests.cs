using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rillium.Script.Expressions;

namespace Rillium.Script.Test.ExpressionTests
{
    [TestClass]
    public class ArraySummaryExpressionTests
    {
        [TestMethod]
        public void InvalidExpressionShouldThrow()
        {
            var token = new Token(TokenId.String, null, 0);

            var ex = new ArraySummaryExpression(
               token,
               token.BuildLiteralExpression(),
               ArraySummaryId.Length);

            Assert.ThrowsException<ScriptException>(() => ex.Evaluate(null));
        }

        [TestMethod]
        public void InvalidSummaryIdShouldThrow()
        {
            const ArraySummaryId bad = (ArraySummaryId)int.MaxValue;

            var expectedMessage = $"Line 1. Invalid aggregate identifier '{bad}'.";

            var token = new Token(TokenId.String, null, 0);

            var ex = new ArraySummaryExpression(
               token,
               new ArrayExpression(
                   token,
                   new List<Expression>() { token.BuildLiteralExpression() }),
                   bad);

            ex.ShouldThrowWithMessage<ScriptException>(expectedMessage);
        }


        [TestMethod]
        public void NumericSummaryOnNonNumericArrayShouldThrow()
        {
            const string expectedMessage = "Line 1. Could not perform aggregate on non numeric array.";

            var token = new Token(TokenId.String, null, 0);

            var ex = new ArraySummaryExpression(
               token,
               new ArrayExpression(
                   token,
                   new List<Expression>() { token.BuildLiteralExpression() }),
                   ArraySummaryId.Sum);

            ex.ShouldThrowWithMessage<ScriptException>(expectedMessage);
        }
    }
}
