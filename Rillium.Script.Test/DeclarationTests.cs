using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Rillium.Script.Test
{
    [TestClass]
    public class DeclarationTests
    {
        [TestMethod]
        public void Tests()
        {
            Assert.AreEqual(expected: 100, Evaluator.Evaluate<int>("var x = 100; x;"));
            Assert.AreEqual(expected: -100, Evaluator.Evaluate<int>("var x = -100; x;"));

            Assert.AreEqual(expected: 100, Evaluator.Evaluate<int>("var x; x = 100; x;"));
            Assert.AreEqual(expected: -100, Evaluator.Evaluate<int>("var x; x = -100; x;"));
        }

        [TestMethod]
        public void TestsMultipleDeclarationTest()
        {
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("var x = 100; var y = 5; var z = y==x; z;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("var x = 100; var y = 100; var z = y==x; z;"));

            Assert.AreEqual(expected: 100, Evaluator.Evaluate<int>("var x = 100; var y = 5; x;"));
            Assert.AreEqual(expected: 5, Evaluator.Evaluate<int>("var x = 100; var y = 5; y;"));
        }

        [TestMethod]
        public void TestsDeclarationPassingTest()
        {
            const string source = @"
                var a = 1;
                var b = a;
                var c = b;
                var d = c;
                return d;";

            Assert.AreEqual(expected: 1, Evaluator.Evaluate<int>(source));

            const string source2 = @"
                var a = 1;
                var b = a + a;
                var c = b + a;
                var d = c + b;
                return d;";

            Assert.AreEqual(expected: 5, Evaluator.Evaluate<int>(source2));
        }

        [TestMethod]
        public void TestVariableIncrementsTest()
        {
            const string source = @"
                var a = 0;
                a = a + 1;
                a = a + 1;
                a = a + 1;
                return a;";

            Assert.AreEqual(expected: 3, Evaluator.Evaluate<int>(source));
        }

        [TestMethod]
        public void TestVariableIncrementsTest2()
        {
            const string source = @"
                var a = 0;
                1 + a;";

            Assert.AreEqual(expected: 1, Evaluator.Evaluate<int>(source));
        }
    }
}
