using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rillium.Script.Expressions;

namespace Rillium.Script.Test
{
    [TestClass]
    public class HelpersTests
    {
        [TestMethod]
        public void NullGuardCorrectly()
        {
            ArrayExpression? arrayExpression = null;
            var exception = Assert.ThrowsException<NullReferenceException>(
                () => arrayExpression.ShouldNotBeNull());

            Assert.AreEqual(exception.Message, $"'{nameof(arrayExpression)}' should not be null.");
        }
    }
}
