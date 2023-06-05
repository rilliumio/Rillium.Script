using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rillium.Script.Expressions;
using Rillium.Script.Statements;

namespace Rillium.Script.Test.StatementTests
{
    [TestClass]
    public class ExpressionStatementTests
    {
        [TestMethod]
        public void InvalidExpressionStatementShouldThrow()
        {
            var token = new Token(TokenId.String, null, 0);
            var ex = new ExpressionStatement(new FooExpression(token));

            var exception = Assert.ThrowsException<ScriptException>(() => ex.Execute(null));
            Assert.AreEqual($"Line 1. Expression type {nameof(FooExpression)} not handled.", exception.Message);
        }

        private class FooExpression : Expression
        {
            public FooExpression(Token token) : base(token)
            {
            }

            public override Expression Evaluate(Scope scope)
            {
                return this;
            }
        }
    }
}
