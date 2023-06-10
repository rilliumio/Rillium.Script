using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rillium.Script.Test
{
    [TestClass]
    public class BlockTests
    {
        [TestMethod]
        public void ForLoopWithArrayAggregateInBody()
        {
            const string source = @"
               var k = 0;
               for (var i = 0; i < 10; i= i+1) 
               {
                   k = k + 1;
               }
               return k;";

            Assert.AreEqual(expected: 10, Evaluator.Evaluate<int>(source));
        }

        [TestMethod]
        public void NestedLoops()
        {
            const string source = @"
               var k = 0;
               for (var i = 0; i < 10; i= i+1) 
               {
                   for (var j = 0; j < 10; j= j+1) 
                   {
                      k = k + i + j;
                   }
               }
               return k;";

            Assert.AreEqual(expected: 900, Evaluator.Evaluate<int>(source));
        }
    }
}
