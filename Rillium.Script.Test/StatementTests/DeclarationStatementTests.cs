using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rillium.Script.Exceptions;
using Rillium.Script.Expressions;
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

        [TestMethod]
        public void ExecuteSetsVariableInScope()
        {
            var scope = new Scope(new FunctionTable());
            var identifier = new Token(TokenId.Identifier, "myVar", 0);
            var initializer = new NumberExpression(identifier, 42.0);
            var statement = new DeclarationStatement(identifier, initializer);

            statement.Execute(scope);

            Assert.IsTrue(scope.HasVariable("myVar"));
            Assert.IsTrue(scope.TryGet("myVar", out object? value));
            Assert.IsInstanceOfType(value, typeof(NumberExpression));
            Assert.AreEqual(expected: 42.0, ((NumberExpression)value).Value);
        }

        [TestMethod]
        public void ExecuteWithNullInitializerSetsNull()
        {
            var scope = new Scope(new FunctionTable());
            var identifier = new Token(TokenId.Identifier, "x", 0);
            var statement = new DeclarationStatement(identifier, null);

            statement.Execute(scope);

            Assert.IsTrue(scope.HasVariable("x"));
            Assert.IsTrue(scope.TryGet("x", out object? value));
            Assert.IsNull(value);
        }

        [TestMethod]
        public void ExecuteWithLiteralInitializer()
        {
            var scope = new Scope(new FunctionTable());
            var identifier = new Token(TokenId.Identifier, "name", 0);
            var literalToken = new Token(TokenId.String, "hello", 0);
            var initializer = literalToken.BuildLiteralExpression();
            var statement = new DeclarationStatement(identifier, initializer);

            statement.Execute(scope);

            Assert.IsTrue(scope.HasVariable("name"));
            Assert.IsTrue(scope.TryGet("name", out object? value));
            Assert.IsInstanceOfType(value, typeof(LiteralExpression));
        }

        [TestMethod]
        public void ExecuteOverwritesExistingVariable()
        {
            var scope = new Scope(new FunctionTable());
            var identifier = new Token(TokenId.Identifier, "x", 0);

            var first = new DeclarationStatement(identifier, new NumberExpression(identifier, 1.0));
            first.Execute(scope);

            var second = new DeclarationStatement(identifier, new NumberExpression(identifier, 99.0));
            second.Execute(scope);

            Assert.IsTrue(scope.TryGet("x", out object? value));
            Assert.IsInstanceOfType(value, typeof(NumberExpression));
            Assert.AreEqual(expected: 99.0, ((NumberExpression)value).Value);
        }

        [TestMethod]
        public void ConstructorSetsProperties()
        {
            var identifier = new Token(TokenId.Identifier, "z", 0);
            var initializer = new NumberExpression(identifier, 7.0);
            var statement = new DeclarationStatement(identifier, initializer);

            Assert.AreEqual(identifier, statement.Identifier);
            Assert.AreEqual(initializer, statement.Initializer);
        }
    }
}
