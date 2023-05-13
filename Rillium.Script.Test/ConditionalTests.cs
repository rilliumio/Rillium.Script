namespace Rillium.Script.Test
{
    [TestClass]
    public class BooleanExpressionTests
    {
        [TestMethod]
        public void EqualityTest()
        {
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("1==1"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("-1==-1"));
        }
    }
}
