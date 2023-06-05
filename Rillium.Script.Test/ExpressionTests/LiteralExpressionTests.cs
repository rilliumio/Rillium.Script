using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rillium.Script.Expressions;

namespace Rillium.Script.Test.ExpressionTests
{
    [TestClass]
    public class LiteralExpressionTests
    {
        [TestMethod]
        public void InvalalidStateShouldThrow()
        {
            var ex = new LiteralExpression(
                new Token(TokenId.If, null, 0),
                new LiteralValue() { TypeId = (LiteralTypeId)99, Value = "" });

            Assert.ThrowsException<InvalidOperationException>(() => ex.Evaluate(null));
        }
    }
}
