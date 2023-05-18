using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rillium.Script.Test
{
    internal static class TestHelpers
    {
        public static void ShouldEvaluateEqual<T>(this string source, T expected) =>
            Assert.AreEqual<T>(expected, Evaluator.Evaluate<T>(source));
    }
}
