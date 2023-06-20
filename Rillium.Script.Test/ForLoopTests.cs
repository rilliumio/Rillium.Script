using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rillium.Script.Test
{
    [TestClass]
    public class ForLoopTests
    {

        [TestMethod]
        public void ForLoopCorrectlyFastReturn()
        {
            const string source = @"
               var i = 0; var k = 0;
               for(i = 0; i< 3; i= i+1) { k = k + 1; }
               k + i;";

            Assert.AreEqual(expected: 6, Evaluator.Evaluate<int>(source));
        }

        [TestMethod]
        public void ForLoopCorrectlyFastReturn2()
        {
            const string source = @"
               var k = 0;
               for(var i = 0; i< 3; i= i+1) { k = k + 1; }
               k + i;";

            Assert.AreEqual(expected: 6, Evaluator.Evaluate<int>(source));
        }


        [TestMethod]
        public void ForLoopCorrectlyReturn()
        {
            const string source = @"
               var i = 0; var k = 0;
               for(i = 0; i< 3; i= i+1) { k = k + 1; }
               return k + i;";

            Assert.AreEqual(expected: 6, Evaluator.Evaluate<int>(source));
        }

        [TestMethod]
        public void ForLoopCorrectlyReturn2()
        {
            const string source = @"
               var k = 0;
               for(var i = 0; i < 100; i= i+1) { k = k + 1; }
               return k;";

            Assert.AreEqual(expected: 100, Evaluator.Evaluate<int>(source));
        }

        [TestMethod]
        public void ForLoopWithArrayAggregateInCondition()
        {
            const string source = @"
               var a = [1,2,3]; var k = 0;
               for(var i = 0; i < a.Length; i= i+1) { k = k + 1; }
               return k;";

            Assert.AreEqual(expected: 3, Evaluator.Evaluate<int>(source));
        }

        [TestMethod]
        public void ForLoopWithArrayAggregateInBody()
        {
            const string source = @"
               var a = [1,2,3]; var k = 0;
               for(var i = 0; i < a.Length; i= i+1) { k = k + a[i]; }
               return k;";

            Assert.AreEqual(expected: 6, Evaluator.Evaluate<int>(source));
        }
        [TestMethod]
        public void ForLoopIncrementCorrectlyReturn()
        {
            const string source = @"
               var k = 0;
               for(var i = 0; i < 100; i++) { k++; }
               return k;";

            Assert.AreEqual(expected: 100, Evaluator.Evaluate<int>(source));
        }
    }
}
