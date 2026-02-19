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

        [TestMethod]
        public void EvaluateDoubleResult()
        {
            CompiledScript script = Evaluator.Compile("var x = 3.14; x;");

            Assert.AreEqual(expected: 3.14, script.Evaluate<double>());
        }

        [TestMethod]
        public void EvaluateDoubleToIntConversion()
        {
            CompiledScript script = Evaluator.Compile("var x = 7.9; x;");

            Assert.AreEqual(expected: 8, script.Evaluate<int>());
        }

        [TestMethod]
        public void EvaluateDoubleToLongConversion()
        {
            CompiledScript script = Evaluator.Compile("var x = 100; x;");

            Assert.AreEqual(expected: 100L, script.Evaluate<long>());
        }

        [TestMethod]
        public void EvaluateDoubleToDecimalConversion()
        {
            CompiledScript script = Evaluator.Compile("var x = 5.5; x;");

            Assert.AreEqual(expected: 5.5m, script.Evaluate<decimal>());
        }

        [TestMethod]
        public void EvaluateBoolResult()
        {
            CompiledScript script = Evaluator.Compile("true;");

            Assert.AreEqual(expected: true, script.Evaluate<bool>());
        }

        [TestMethod]
        public void EvaluateStringResult()
        {
            CompiledScript script = Evaluator.Compile("'hello';");

            Assert.AreEqual(expected: "hello", script.Evaluate<string>());
        }

        [TestMethod]
        public void EvaluateByteArrayResult()
        {
            CompiledScript script = Evaluator.Compile("var x = [1, 2, 3]; x;");

            CollectionAssert.AreEquivalent(
                expected: new byte[] { 1, 2, 3 },
                script.Evaluate<byte[]>());
        }

        [TestMethod]
        public void EvaluateShortArrayResult()
        {
            CompiledScript script = Evaluator.Compile("var x = [10, 20, 30]; x;");

            CollectionAssert.AreEquivalent(
                expected: new short[] { 10, 20, 30 },
                script.Evaluate<short[]>());
        }

        [TestMethod]
        public void EvaluateIntArrayResult()
        {
            CompiledScript script = Evaluator.Compile("var x = [100, 200, 300]; x;");

            CollectionAssert.AreEquivalent(
                expected: new int[] { 100, 200, 300 },
                script.Evaluate<int[]>());
        }

        [TestMethod]
        public void EvaluateDoubleArrayResult()
        {
            CompiledScript script = Evaluator.Compile("var x = [1.1, 2.2, 3.3]; x;");

            CollectionAssert.AreEquivalent(
                expected: new double[] { 1.1, 2.2, 3.3 },
                script.Evaluate<double[]>());
        }

        [TestMethod]
        public void EvaluateNullResultThrows()
        {
            CompiledScript script = Evaluator.Compile("");

            Assert.ThrowsException<ArgumentException>(
                () => script.Evaluate<int>());
        }

        [TestMethod]
        public void RunWithStreamWriter()
        {
            CompiledScript script = Evaluator.Compile("var x = 42; x;");

            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream);

            object? result = script.Run(streamWriter);

            Assert.AreEqual(expected: 42.0, result);
        }

        [TestMethod]
        public void RunWithStreamWriterAndArgs()
        {
            CompiledScript script = Evaluator.Compile("args[0];");

            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream);

            object? result = script.Run(streamWriter, 99);

            Assert.AreEqual(expected: 99.0, result);
        }

        [TestMethod]
        public void RunReturnsNullForEmptyScript()
        {
            CompiledScript script = Evaluator.Compile("");

            var (output, console) = script.Run();

            Assert.IsNull(output);
            Assert.AreEqual(expected: "", console);
        }

        [TestMethod]
        public void ReturnFromInsideIfBlock()
        {
            CompiledScript script = Evaluator.Compile(@"
                var x = args[0];
                if (x > 10) { return x * 2; }
                return x;
            ");

            Assert.AreEqual(expected: 40, script.Evaluate<int>(20));
            Assert.AreEqual(expected: 5, script.Evaluate<int>(5));
        }

        [TestMethod]
        public void ReturnFromInsideForLoop()
        {
            CompiledScript script = Evaluator.Compile(@"
                for (var i = 0; i < args[0]; i++) {
                    if (i == 3) { return i; }
                }
                return -1;
            ");

            Assert.AreEqual(expected: 3, script.Evaluate<int>(10));
            Assert.AreEqual(expected: -1, script.Evaluate<int>(2));
        }

        [TestMethod]
        public void EvaluateWithNullArgs()
        {
            CompiledScript script = Evaluator.Compile("var x = 5; x;");

            Assert.AreEqual(expected: 5, script.Evaluate<int>(null));
        }

        [TestMethod]
        public void RunWithNullArgs()
        {
            CompiledScript script = Evaluator.Compile("var x = 10; x;");

            var (output, console) = script.Run((object[]?)null);

            Assert.AreEqual(expected: 10.0, output);
        }
    }
}
