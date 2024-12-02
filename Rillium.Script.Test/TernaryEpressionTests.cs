using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rillium.Script.Test
{
    [TestClass]
    public class TernaryTests
    {
        [TestMethod]
        public void TernaryCorrectly()
        {
            Assert.AreEqual(expected: 2, Evaluator.Evaluate<int>("return true ? 2 : 4;"));
            Assert.AreEqual(expected: 4, Evaluator.Evaluate<int>("return false ? 2 : 4;"));

            Assert.AreEqual(expected: 2, Evaluator.Evaluate<int>("return (true ? 2 : 4);"));
            Assert.AreEqual(expected: 4, Evaluator.Evaluate<int>("return (false ? 2 : 4);"));

            Assert.AreEqual(expected: 2, Evaluator.Evaluate<int>("var x = true ? 2 : 4; return x;"));
            Assert.AreEqual(expected: 4, Evaluator.Evaluate<int>("var x = false ? 2 : 4; return x;"));

            Assert.AreEqual(expected: 2, Evaluator.Evaluate<int>("var x = (true ? 2 : 4); return x;"));
            Assert.AreEqual(expected: 4, Evaluator.Evaluate<int>("var x = (false ? 2 : 4); return x;"));
        }

        [TestMethod]
        public void TernaryCorrectly2()
        {
            Assert.AreEqual(expected: 2, Evaluator.Evaluate<int>("return 1==1 ? 2 : 4;"));
            Assert.AreEqual(expected: 4, Evaluator.Evaluate<int>("return 1==2 ? 2 : 4;"));

            Assert.AreEqual(expected: 2, Evaluator.Evaluate<int>("return 1!=2 ? 2 : 4;"));
            Assert.AreEqual(expected: 4, Evaluator.Evaluate<int>("return 1!=1 ? 2 : 4;"));

            Assert.AreEqual(expected: 2, Evaluator.Evaluate<int>("return 1<2 ? 2 : 4;"));
            Assert.AreEqual(expected: 4, Evaluator.Evaluate<int>("return 1>2 ? 2 : 4;"));

            Assert.AreEqual(expected: 2, Evaluator.Evaluate<int>("var x = 1==1 ? 2 : 4;x;"));
            Assert.AreEqual(expected: 4, Evaluator.Evaluate<int>("var x = 1==2 ? 2 : 4;x;"));

            Assert.AreEqual(expected: 2, Evaluator.Evaluate<int>("var x = 1!=2 ? 2 : 4;x;"));
            Assert.AreEqual(expected: 4, Evaluator.Evaluate<int>("var x = 1!=1 ? 2 : 4;x;"));

            Assert.AreEqual(expected: 2, Evaluator.Evaluate<int>("var x = 1<2 ? 2 : 4;x;"));
            Assert.AreEqual(expected: 4, Evaluator.Evaluate<int>("var x = 1>2 ? 2 : 4;x;"));
        }

        [TestMethod]
        public void TernaryNestedCorrectly()
        {
            Assert.AreEqual(expected: 4, Evaluator.Evaluate<int>("return (1 == 1 ? true : false) ? 4 : 5;"));
            Assert.AreEqual(expected: 5, Evaluator.Evaluate<int>("return (1 != 1 ? true : false) ? 4 : 5;"));

            Assert.AreEqual(expected: 2, Evaluator.Evaluate<int>("return (1 == 1 ? (2 == 2 ? 2 : 4) : 10);"));
            Assert.AreEqual(expected: 4, Evaluator.Evaluate<int>("return (1 == 1 ? (2 != 2 ? 2 : 4) : 10);"));

            Assert.AreEqual(expected: 10, Evaluator.Evaluate<int>("return (1 != 1 ? (2 == 2 ? 2 : 4) : 10);"));
            Assert.AreEqual(expected: 10, Evaluator.Evaluate<int>("return (1 != 1 ? (2 != 2 ? 2 : 4) : 10);"));

            Assert.AreEqual(expected: 8, Evaluator.Evaluate<int>("return 5 + (true ? 3 : 1);"));
            Assert.AreEqual(expected: 6, Evaluator.Evaluate<int>("return 5 + (false ? 3 : 1);"));

            Assert.AreEqual(expected: 4, Evaluator.Evaluate<int>("var x = (1 == 1 ? true : false) ? 4 : 5;x;"));
            Assert.AreEqual(expected: 5, Evaluator.Evaluate<int>("var x = (1 != 1 ? true : false) ? 4 : 5;x;"));

            Assert.AreEqual(expected: 2, Evaluator.Evaluate<int>("var x = (1 == 1 ? (2 == 2 ? 2 : 4) : 10);x;"));
            Assert.AreEqual(expected: 4, Evaluator.Evaluate<int>("var x = (1 == 1 ? (2 != 2 ? 2 : 4) : 10);x;"));

            Assert.AreEqual(expected: 10, Evaluator.Evaluate<int>("var x = (1 != 1 ? (2 == 2 ? 2 : 4) : 10);x;"));
            Assert.AreEqual(expected: 10, Evaluator.Evaluate<int>("var x = (1 != 1 ? (2 != 2 ? 2 : 4) : 10);x;"));

            Assert.AreEqual(expected: 8, Evaluator.Evaluate<int>("var x = 5 + (true ? 3 : 1); x;"));
            Assert.AreEqual(expected: 6, Evaluator.Evaluate<int>("var x = 5 + (false ? 3 : 1); x;"));
        }
    }
}