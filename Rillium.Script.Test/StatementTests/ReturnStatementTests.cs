using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rillium.Script.Statements;

namespace Rillium.Script.Test.StatementTests
{
    [TestClass]
    public class ReturnStatementTests
    {
        [TestMethod]
        public void ReturnStatementExecuteShouldThrow()
        {
            var token = new Token(TokenId.String, null, 0);
            var ex = new ReturnStatement(token.BuildLiteralExpression());

            Assert.ThrowsException<NotImplementedException>(() => ex.Execute(null));
        }
    }
}
