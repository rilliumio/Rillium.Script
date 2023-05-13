namespace Rillium.Script.Test
{
    [TestClass]
    public class PrimitiveExpressionsTests
    {
        [TestMethod]
        public void SingleValue()
        {

            Assert.AreEqual(expected: (byte)1, Evaluator.Evaluate<byte>("1"));
            Assert.AreEqual(expected: (byte)1, Evaluator.Evaluate<byte>("1;"));
            Assert.AreEqual(expected: (byte)1, Evaluator.Evaluate<byte>("return 1;"));

            Assert.AreEqual(expected: (short)1, Evaluator.Evaluate<short>("1"));
            Assert.AreEqual(expected: (short)1, Evaluator.Evaluate<short>("1;"));
            Assert.AreEqual(expected: (short)1, Evaluator.Evaluate<short>("return 1;"));

            Assert.AreEqual(expected: 1, Evaluator.Evaluate<int>("1"));
            Assert.AreEqual(expected: 1, Evaluator.Evaluate<int>("1;"));
            Assert.AreEqual(expected: 1, Evaluator.Evaluate<int>("return 1;"));

            Assert.AreEqual(expected: 1d, Evaluator.Evaluate<double>("1"));
            Assert.AreEqual(expected: 1d, Evaluator.Evaluate<double>("1;"));
            Assert.AreEqual(expected: 1d, Evaluator.Evaluate<double>("return 1;"));

            Assert.AreEqual(expected: 1f, Evaluator.Evaluate<float>("1"));
            Assert.AreEqual(expected: 1f, Evaluator.Evaluate<float>("1;"));
            Assert.AreEqual(expected: 1f, Evaluator.Evaluate<float>("return 1;"));

            Assert.AreEqual(expected: -1, Evaluator.Evaluate<int>("-1"));
            Assert.AreEqual(expected: -1, Evaluator.Evaluate<int>("-1;"));
            Assert.AreEqual(expected: -1, Evaluator.Evaluate<int>("return -1;"));
        }

        [TestMethod]
        public void NegativeValue()
        {
            Assert.AreEqual(expected: -1, Evaluator.Evaluate<int>("return -(2-1);"));
            Assert.AreEqual(expected: -1, Evaluator.Evaluate<int>("-(2-1)"));
            Assert.AreEqual(expected: -1, Evaluator.Evaluate<int>("-(2-1);"));

            Assert.AreEqual(expected: -1, Evaluator.Evaluate<int>("return -1;"));
            Assert.AreEqual(expected: -1, Evaluator.Evaluate<int>("-1"));
            Assert.AreEqual(expected: -1, Evaluator.Evaluate<int>("-1;"));
            Assert.AreEqual(expected: 0, Evaluator.Evaluate<int>("1-1"));
            Assert.AreEqual(expected: 0, Evaluator.Evaluate<int>("-(1-1)"));
        }

        [TestMethod]
        public void TestAddition()
        {
            Assert.AreEqual(
                expected: 2,
                Evaluator.Evaluate<int>("1+1"));

            Assert.AreEqual(
               expected: 2,
               Evaluator.Evaluate<int>("(1+1)"));

            Assert.AreEqual(
              expected: 3,
              Evaluator.Evaluate<int>("(1+2)"));
        }

        [TestMethod]
        public void TestMultiplication()
        {
            Assert.AreEqual(
                expected: 6,
                Evaluator.Evaluate<int>("2*(1+2)"));
        }

        [TestMethod]
        public void TestDivision()
        {
            Assert.AreEqual(
               expected: 2.0 / 3.0,
               Evaluator.Evaluate<double>("2/(1+2)"));
        }

        [TestMethod]
        public void TestAssignment()
        {
            Assert.AreEqual(
              expected: 7,
              Evaluator.Evaluate<int>("var x=7;x;"));
        }

        [TestMethod]
        public void EvaluateStatementEof()
        {
            Assert.AreEqual(
            expected: 1,
            Evaluator.Evaluate<int>("1;"));
        }

        [TestMethod]
        public void IfThenCorrectly()
        {
            const string source = "var x =0; if(1==1){ x=1; }else{ x=2;}; x;";

            Assert.AreEqual(
                expected: 1,
                Evaluator.Evaluate<int>(source));
        }

        [TestMethod]
        public void IfElseCorrectly()
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
    }
}