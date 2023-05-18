using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rillium.Script.Test
{
    [TestClass]
    public class ArrayTests
    {
        [TestMethod]
        public void DeclareByteArrayCorrectly()
        {
            CollectionAssert.AreEquivalent(
                expected: new byte[] { 2, 4, 5 },
                Evaluator.Evaluate<byte[]>("var x = [2, 4.1, 5.2]; x;"));

            CollectionAssert.AreEquivalent(
                expected: new byte[] { 2, 4, 5 },
                Evaluator.Evaluate<byte[]>("var x = [2,4,5]; x;"));
        }

        [TestMethod]
        public void DeclareShortArrayCorrectly()
        {
            CollectionAssert.AreEquivalent(
                expected: new short[] { 2, 4, 5 },
                Evaluator.Evaluate<short[]>("var x = [2, 4, 5.2]; x;"));

            CollectionAssert.AreEquivalent(
                expected: new short[] { 2, 4, 5 },
                Evaluator.Evaluate<short[]>("var x = [2,4,5]; x;"));
        }

        [TestMethod]
        public void DeclareIntArrayCorrectly()
        {
            CollectionAssert.AreEquivalent(
                expected: new int[] { 2, 4, 5 },
                Evaluator.Evaluate<int[]>("var x = [2, 4.1, 5.2]; x;"));

            CollectionAssert.AreEquivalent(
                expected: new int[] { 2, 4, 5 },
                Evaluator.Evaluate<int[]>("var x = [2,4,5]; x;"));
        }

        [TestMethod]
        public void DeclareDoubleArrayCorrectly()
        {
            CollectionAssert.AreEquivalent(
                expected: new double[] { 2, 4.1, 5.2 },
                Evaluator.Evaluate<double[]>("var x = [2, 4.1, 5.2]; x;"));

            CollectionAssert.AreEquivalent(
                expected: new double[] { 2, 4, 5 },
                Evaluator.Evaluate<double[]>("var x = [2,4,5]; x;"));
        }

        [TestMethod]
        public void ArrayValueByIndexCorrectly()
        {
            Assert.AreEqual(expected: 2, Evaluator.Evaluate<int>("var x = [2]; x[0];"));
            Assert.AreEqual(expected: 4, Evaluator.Evaluate<int>("var x = [2,4,8]; x[1];"));
            Assert.AreEqual(expected: 8, Evaluator.Evaluate<int>("var x = [2,4,8]; x[2];"));
        }

        [TestMethod]
        public void ArrayAggregateTests()
        {
            Assert.AreEqual(expected: 3, Evaluator.Evaluate<int>("var x = [2,4,8]; x.Length;"));
            Assert.AreEqual(expected: 14, Evaluator.Evaluate<int>("var x = [2,4,8]; x.Sum();"));
            Assert.AreEqual(expected: 2, Evaluator.Evaluate<int>("var x = [2,4,8]; x.Min();"));
            Assert.AreEqual(expected: 8, Evaluator.Evaluate<int>("var x = [2,4,8]; x.Max();"));
            Assert.AreEqual(expected: 5, Evaluator.Evaluate<int>("var x = [2,4,8]; x.Average();"));
        }

        [TestMethod]
        public void ArrayAggregateExpressionTests1()
        {
            Assert.AreEqual(expected: 4, Evaluator.Evaluate<int>("var x = [2,4,8]; x.Length + 1;"));
            Assert.AreEqual(expected: 15, Evaluator.Evaluate<int>("var x = [2,4,8]; x.Sum() + 1;"));
            Assert.AreEqual(expected: 3, Evaluator.Evaluate<int>("var x = [2,4,8]; x.Min() + 1;"));
            Assert.AreEqual(expected: 9, Evaluator.Evaluate<int>("var x = [2,4,8]; x.Max() + 1;"));
            Assert.AreEqual(expected: 6, Evaluator.Evaluate<int>("var x = [2,4,8]; x.Average() + 1;"));
        }

        [TestMethod]
        public void ArrayAggregateExpressionTests2()
        {
            Assert.AreEqual(expected: 4, Evaluator.Evaluate<int>("var x = [2,4,8]; 1 + x.Length;"));
            Assert.AreEqual(expected: 15, Evaluator.Evaluate<int>("var x = [2,4,8]; 1 + x.Sum();"));
            Assert.AreEqual(expected: 3, Evaluator.Evaluate<int>("var x = [2,4,8]; 1 + x.Min();"));
            Assert.AreEqual(expected: 9, Evaluator.Evaluate<int>("var x = [2,4,8]; 1 + x.Max();"));
            Assert.AreEqual(expected: 6, Evaluator.Evaluate<int>("var x = [2,4,8]; 1 + x.Average();"));
        }
    }
}
