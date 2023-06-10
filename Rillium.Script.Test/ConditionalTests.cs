using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Rillium.Script.Test
{
    [TestClass]
    public class ConditionalTestsExpressionTests
    {
        [TestMethod]
        public void IfElseCorrectly()
        {
            const string source = "if(true) { return 1; } else { return 2;}";

            Assert.AreEqual(
                expected: 1,
                Evaluator.Evaluate<int>(source));
        }


        [TestMethod]
        public void IfElseCorrectlyFastReturn()
        {
            const string source = "var x =0; if(1==10){ x=1; }else{ x=2;}; x;";

            Assert.AreEqual(
                expected: 2,
                Evaluator.Evaluate<int>(source));

            const string source2 = "var x =0; if(1==10){ x=1; }else{ x=(2*(8+1));}; x;";

            Assert.AreEqual(
                expected: 18,
                Evaluator.Evaluate<int>(source2));
        }


        [TestMethod]
        public void EqualityTest()
        {
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("1==1"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("-1==-1"));
        }
    }
}
