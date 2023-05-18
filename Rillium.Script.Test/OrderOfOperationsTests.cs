using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rillium.Script.Test
{
    [TestClass]
    public class OrderOfOperationsTests
    {
        [TestMethod]
        public void OrderOfOperationsPositivesCorrectly()
        {
            foreach (var source in OrderOfOpertationsDeps.PositiveCases.Keys)
            {
                source.ShouldEvaluateEqual(
                    OrderOfOpertationsDeps.PositiveCases[source]);
            }
        }

        [TestMethod]
        public void OrderOfOperationsNegativesCorrectly()
        {
            foreach (var source in OrderOfOpertationsDeps.NegativeCases.Keys)
            {
                source.ShouldEvaluateEqual(
                    OrderOfOpertationsDeps.NegativeCases[source]);
            }
        }
    }
}
