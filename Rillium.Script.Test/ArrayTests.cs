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
        }
    }
}
