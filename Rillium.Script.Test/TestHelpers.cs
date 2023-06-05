using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rillium.Script.Expressions;

namespace Rillium.Script.Test
{
    internal static class TestHelpers
    {
        public static void ShouldEvaluateEqual<T>(this string source, T expected) =>
            Assert.AreEqual<T>(expected, Evaluator.Evaluate<T>(source));

        public static void ShouldThrowWithMessage<T>(string expectedExceptionMessage, Func<object?> func) where T : Exception =>
           Assert.AreEqual(
               expectedExceptionMessage,
               Assert.ThrowsException<T>(func).Message);

        public static void ShouldThrowWithMessage<T>(string source, string expectedExceptionMessage) where T : Exception =>
            Assert.AreEqual(
                expectedExceptionMessage,
                Assert.ThrowsException<T>(() => Evaluator.Evaluate<object>(source)).Message);

        public static void ShouldThrowWithMessage<T>(this Expression expression, Scope scope, string expectedExceptionMessage) where T : Exception =>
            Assert.AreEqual(
                 expectedExceptionMessage,
                 Assert.ThrowsException<T>(() => expression.Evaluate(scope)).Message);

        public static void ShouldThrowWithMessage<T>(this Expression expression, string expectedExceptionMessage) where T : Exception =>
            ShouldThrowWithMessage<T>(expression, scope: new Scope(new FunctionTable()), expectedExceptionMessage);
    }
}
