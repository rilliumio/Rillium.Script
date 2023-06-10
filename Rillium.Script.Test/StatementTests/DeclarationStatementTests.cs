using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rillium.Script.Exceptions;
using Rillium.Script.Statements;

namespace Rillium.Script.Test.StatementTests
{
    [TestClass]
    public class DeclarationStatementTests
    {
        [TestMethod]
        public void ReturnStatementExecuteShouldThrow()
        {
            var token = new Token(TokenId.String, null, 0);
            var ex = new ReturnStatement(token.BuildLiteralExpression());

            Assert.ThrowsException<ReturnStatementException>(() => ex.Execute(null));
        }
    }
}
