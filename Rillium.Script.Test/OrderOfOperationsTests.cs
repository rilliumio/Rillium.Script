using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rillium.Script.Test
{
    [TestClass]
    public class OrderOfOperationsTests
    {
        [TestMethod]
        public void OrderOfOperationsPositivesCorrectly()
        {
            foreach (var source in OrderOfOperationsDeps.PositiveCases.Keys)
            {
                source.ShouldEvaluateEqual(
                    OrderOfOperationsDeps.PositiveCases[source]);
            }
        }

        [TestMethod]
        public void OrderOfOperationsNegativesCorrectly()
        {
            foreach (var source in OrderOfOperationsDeps.NegativeCases.Keys)
            {
                source.ShouldEvaluateEqual(
                    OrderOfOperationsDeps.NegativeCases[source]);
            }
        }
    }
}
