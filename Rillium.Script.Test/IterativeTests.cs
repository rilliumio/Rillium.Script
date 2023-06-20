using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rillium.Script.Test
{

    [TestClass]
    public class IterativeTests
    {
        [TestMethod]
        public void PostIncrementCorrectly()
        {
            Assert.AreEqual(expected: 10, Evaluator.Evaluate<int>("var i = 10; i++;"));
            Assert.AreEqual(expected: 10, Evaluator.Evaluate<int>("var i = 10; (i++);"));
            Assert.AreEqual(expected: 10, Evaluator.Evaluate<int>("var i = 10; return (i++);"));

            Assert.AreEqual(expected: 11, Evaluator.Evaluate<int>("var i = 10; i++; i;"));
            Assert.AreEqual(expected: 12, Evaluator.Evaluate<int>("var i = 10; i++; i++; i;"));
        }

        [TestMethod]
        public void PostDecrementCorrectly()
        {
            Assert.AreEqual(expected: 10, Evaluator.Evaluate<int>("var i = 10; i--;"));
            Assert.AreEqual(expected: 10, Evaluator.Evaluate<int>("var i = 10; (i--);"));
            Assert.AreEqual(expected: 10, Evaluator.Evaluate<int>("var i = 10; return (i--);"));

            Assert.AreEqual(expected: 9, Evaluator.Evaluate<int>("var i = 10; i--; i;"));
            Assert.AreEqual(expected: 8, Evaluator.Evaluate<int>("var i = 10; i--; i--; i;"));
        }

        [TestMethod]
        public void PreIncrement()
        {
            Assert.AreEqual(expected: 11, Evaluator.Evaluate<int>("var i = 10; ++i; i;"));
            Assert.AreEqual(expected: 11, Evaluator.Evaluate<int>("var i = 10; ++i;"));
            Assert.AreEqual(expected: 11, Evaluator.Evaluate<int>("var i = 10; (++i);"));
            Assert.AreEqual(expected: 11, Evaluator.Evaluate<int>("var i = 10; return (++i);"));
            Assert.AreEqual(expected: 12, Evaluator.Evaluate<int>("var i = 10; ++i; ++i; i;"));
        }

        [TestMethod]
        public void PreDecrement()
        {
            Assert.AreEqual(expected: 9, Evaluator.Evaluate<int>("var i = 10; --i; i;"));
            Assert.AreEqual(expected: 9, Evaluator.Evaluate<int>("var i = 10; --i;"));
            Assert.AreEqual(expected: 9, Evaluator.Evaluate<int>("var i = 10; (--i);"));
            Assert.AreEqual(expected: 9, Evaluator.Evaluate<int>("var i = 10; return (--i);"));
            Assert.AreEqual(expected: 8, Evaluator.Evaluate<int>("var i = 10; --i; --i; i;"));
        }
    }
}
