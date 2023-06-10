using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rillium.Script.Exceptions;

namespace Rillium.Script.Test
{
    [TestClass]
    public class DeclarationTests
    {
        private const string Line1BadName = "Line 1. The name 'b' does not exist in the current context.";

        [TestMethod]
        public void UnassignedVariableTests()
        {
            const string message = "Line 1. Use of unassigned local variable 'x'.";
            TestHelpers.ShouldThrowWithMessage<ScriptException>("var x; x;", message);
            TestHelpers.ShouldThrowWithMessage<ScriptException>("var x; return x;", message);
            TestHelpers.ShouldThrowWithMessage<ScriptException>("var x; var y = 1 + x; y;", message);
            TestHelpers.ShouldThrowWithMessage<ScriptException>("var x; var y = x + 1; y;", message);
        }

        [TestMethod]
        public void Tests()
        {
            Assert.AreEqual(expected: 100, Evaluator.Evaluate<int>("var x; x = 100; x;"));

            Assert.AreEqual(expected: 100, Evaluator.Evaluate<int>("var x = 100; x;"));
            Assert.AreEqual(expected: -100, Evaluator.Evaluate<int>("var x = -100; x;"));

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


        [TestMethod]
        public void TestVariableIncrementsTest3()
        {
            const string source = @"var a = 1; var b = 2; a + b;";

            Assert.AreEqual(expected: 3, Evaluator.Evaluate<int>(source));
        }

        [TestMethod]
        public void TestVariableIncrementsTest4()
        {
            const string source = @"
                var a = 1;
                var b = 2;
                var c = 3;
                return a + b + c;";

            Assert.AreEqual(expected: 6, Evaluator.Evaluate<int>(source));
        }

        public void UndeclaredVariableTests() =>
            TestHelpers.ShouldThrowWithMessage<BadNameException>("var a = 1; a = b +1; a;", Line1BadName);

        [TestMethod]
        public void UndeclaredVariableTests2() =>
            TestHelpers.ShouldThrowWithMessage<BadNameException>("b + 1;", Line1BadName);

        [TestMethod]
        public void UndeclaredVariableTests3() =>
            TestHelpers.ShouldThrowWithMessage<BadNameException>("1 + b;", Line1BadName);

        [TestMethod]
        public void UndeclaredVariableTests4() =>
            TestHelpers.ShouldThrowWithMessage<BadNameException>("return b + 1;", Line1BadName);

        [TestMethod]
        public void UndeclaredVariableTests5() =>
            TestHelpers.ShouldThrowWithMessage<BadNameException>("b = 1;", Line1BadName);

        [TestMethod]
        public void UndeclaredVariableTests6() =>
           TestHelpers.ShouldThrowWithMessage<BadNameException>("b n = 1;", Line1BadName);
    }
}
