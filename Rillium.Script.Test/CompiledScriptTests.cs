using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rillium.Script.Test
{
    [TestClass]
    public class CompiledScriptTests
    {
        [TestMethod]
        public void CompileOnceEvaluateMultipleTimesWithDifferentArgs()
        {
            CompiledScript script = Evaluator.Compile("var x = args[0] * 2; x;");

            Assert.AreEqual(expected: 10, script.Evaluate<int>(5));
            Assert.AreEqual(expected: 20, script.Evaluate<int>(10));
            Assert.AreEqual(expected: -6, script.Evaluate<int>(-3));
        }

        [TestMethod]
        public void CompileOnceEvaluateMultipleTimesNoArgs()
        {
            CompiledScript script = Evaluator.Compile("var x = 1 + 2; x;");

            Assert.AreEqual(expected: 3, script.Evaluate<int>());
            Assert.AreEqual(expected: 3, script.Evaluate<int>());
        }

        [TestMethod]
        public void CompiledScriptRunReturnsOutput()
        {
            CompiledScript script = Evaluator.Compile("var x = args[0] * 3; x;");

            var (output1, console1) = script.Run(5);
            Assert.AreEqual(expected: 15.0, output1);

            var (output2, console2) = script.Run(10);
            Assert.AreEqual(expected: 30.0, output2);
        }

        [TestMethod]
        public void CompiledScriptRepeatedCallsAreIndependent()
        {
            CompiledScript script = Evaluator.Compile("var x = args[0]; var y = x + 10; y;");

            Assert.AreEqual(expected: 15, script.Evaluate<int>(5));
            Assert.AreEqual(expected: 110, script.Evaluate<int>(100));
        }

        [TestMethod]
        public void CompiledScriptWithReturnStatement()
        {
            CompiledScript script = Evaluator.Compile("return args[0] + args[1];");

            Assert.AreEqual(expected: 11, script.Evaluate<int>(5, 6));
            Assert.AreEqual(expected: 3, script.Evaluate<int>(1, 2));
        }

        [TestMethod]
        public void CompiledScriptWithForLoop()
        {
            CompiledScript script = Evaluator.Compile(@"
                var sum = 0;
                for (var i = 0; i < args[0]; i++) {
                    sum = sum + i;
                }
                return sum;
            ");

            Assert.AreEqual(expected: 10, script.Evaluate<int>(5));
            Assert.AreEqual(expected: 0, script.Evaluate<int>(1));
        }

        [TestMethod]
        public void CompiledScriptWithStringArgs()
        {
            CompiledScript script = Evaluator.Compile("args[0];");

            Assert.AreEqual(expected: "foo", script.Evaluate<string>("foo"));
            Assert.AreEqual(expected: "bar", script.Evaluate<string>("bar"));
        }
    }
}
