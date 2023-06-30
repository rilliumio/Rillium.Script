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
        public void OrderOfOperationsPositivesInAssignmentCorrectly()
        {
            foreach (var source in OrderOfOperationsDeps.PositiveCases.Keys)
            {
                var assignmentSource = $"var x = {OrderOfOperationsDeps.PositiveCases[source]}; x;";
                assignmentSource.ShouldEvaluateEqual(OrderOfOperationsDeps.PositiveCases[source]);
            }
        }

        [TestMethod]
        public void OrderOfOperationsNegativesAssignmentCorrectly()
        {
            foreach (var source in OrderOfOperationsDeps.NegativeCases.Keys)
            {
                var assignmentSource = $"var x = {OrderOfOperationsDeps.NegativeCases[source]}; x;";
                assignmentSource.ShouldEvaluateEqual(OrderOfOperationsDeps.NegativeCases[source]);
            }
        }
    }
}
