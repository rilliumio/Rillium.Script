using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rillium.Script.Test
{
    internal static class TestHelpers
    {
        public static void ShouldEvaluateEqual<T>(this string source, T expected) =>
            Assert.AreEqual<T>(expected, Evaluator.Evaluate<T>(source));

        public static void ShouldThrowWithMessage<T>(string source, string expectedExceptionMessage) where T : Exception =>
            Assert.AreEqual(
                expectedExceptionMessage,
                Assert.ThrowsException<T>(() => Evaluator.Evaluate<object>(source)).Message);
    }
}
