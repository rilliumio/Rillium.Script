using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rillium.Script.Exceptions;

namespace Rillium.Script.Test
{
    [TestClass]
    public class EvaluatorTests
    {
        // --- Compile ---

        [TestMethod]
        public void CompileReturnsCompiledScript()
        {
            CompiledScript result = Evaluator.Compile("var x = 1; x;");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CompileEmptyScriptReturnsCompiledScript()
        {
            CompiledScript result = Evaluator.Compile("");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CompileSyntaxErrorThrows()
        {
            Assert.ThrowsException<InvalidOperationException>(
                () => Evaluator.Compile("var = ;"));
        }

        [TestMethod]
        public void CompileResultIsReusable()
        {
            CompiledScript script = Evaluator.Compile("args[0] * 2;");

            Assert.AreEqual(expected: 10, script.Evaluate<int>(5));
            Assert.AreEqual(expected: 20, script.Evaluate<int>(10));
        }

        // --- Run(string, params object[]?) ---

        [TestMethod]
        public void RunReturnsOutputAndConsole()
        {
            var (output, console) = Evaluator.Run("var x = 42; x;");

            Assert.AreEqual(expected: 42.0, output);
            Assert.IsNotNull(console);
        }

        [TestMethod]
        public void RunWithArgsReturnsCorrectOutput()
        {
            var (output, console) = Evaluator.Run("args[0] + args[1];", 3, 7);

            Assert.AreEqual(expected: 10.0, output);
        }

        [TestMethod]
        public void RunWithNullArgsReturnsOutput()
        {
            var (output, console) = Evaluator.Run("var x = 5; x;", (object[]?)null);

            Assert.AreEqual(expected: 5.0, output);
        }

        [TestMethod]
        public void RunEmptyScriptReturnsNull()
        {
            var (output, console) = Evaluator.Run("");

            Assert.IsNull(output);
            Assert.AreEqual(expected: "", console);
        }

        [TestMethod]
        public void RunWithReturnStatement()
        {
            var (output, console) = Evaluator.Run("return 99;");

            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void RunStringExpression()
        {
            var (output, console) = Evaluator.Run("'hello';");

            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void RunBoolExpression()
        {
            var (output, console) = Evaluator.Run("true;");

            Assert.AreEqual(expected: true, output);
        }

        // --- Run(string, StreamWriter, params object[]?) ---

        [TestMethod]
        public void RunWithStreamWriterReturnsOutput()
        {
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream);

            object? result = Evaluator.Run("var x = 10; x;", streamWriter);

            Assert.AreEqual(expected: 10.0, result);
        }

        [TestMethod]
        public void RunWithStreamWriterAndArgs()
        {
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream);

            object? result = Evaluator.Run("args[0] + 1;", streamWriter, 4);

            Assert.AreEqual(expected: 5.0, result);
        }

        [TestMethod]
        public void RunWithStreamWriterEmptyScript()
        {
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream);

            object? result = Evaluator.Run("", streamWriter);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void RunWithStreamWriterReturnStatement()
        {
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream);

            object? result = Evaluator.Run("return 7;", streamWriter);

            Assert.IsNotNull(result);
        }

        // --- Evaluate<T>(string, params object[]?) ---

        [TestMethod]
        public void EvaluateInt()
        {
            Assert.AreEqual(expected: 42, Evaluator.Evaluate<int>("42;"));
        }

        [TestMethod]
        public void EvaluateDouble()
        {
            Assert.AreEqual(expected: 3.14, Evaluator.Evaluate<double>("3.14;"));
        }

        [TestMethod]
        public void EvaluateLong()
        {
            Assert.AreEqual(expected: 100L, Evaluator.Evaluate<long>("100;"));
        }

        [TestMethod]
        public void EvaluateDecimal()
        {
            Assert.AreEqual(expected: 2.5m, Evaluator.Evaluate<decimal>("2.5;"));
        }

        [TestMethod]
        public void EvaluateByte()
        {
            Assert.AreEqual(expected: (byte)255, Evaluator.Evaluate<byte>("255;"));
        }

        [TestMethod]
        public void EvaluateShort()
        {
            Assert.AreEqual(expected: (short)100, Evaluator.Evaluate<short>("100;"));
        }

        [TestMethod]
        public void EvaluateBool()
        {
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("true;"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("false;"));
        }

        [TestMethod]
        public void EvaluateString()
        {
            Assert.AreEqual(expected: "hello", Evaluator.Evaluate<string>("'hello';"));
        }

        [TestMethod]
        public void EvaluateWithArgs()
        {
            Assert.AreEqual(expected: 15, Evaluator.Evaluate<int>("args[0] + args[1];", 5, 10));
        }

        [TestMethod]
        public void EvaluateWithReturnStatement()
        {
            Assert.AreEqual(expected: 7, Evaluator.Evaluate<int>("return 3 + 4;"));
        }

        [TestMethod]
        public void EvaluateArithmeticExpression()
        {
            Assert.AreEqual(expected: 20, Evaluator.Evaluate<int>("(2 + 3) * 4;"));
        }

        [TestMethod]
        public void EvaluateVariableDeclaration()
        {
            Assert.AreEqual(expected: 50, Evaluator.Evaluate<int>("var x = 25; var y = 25; x + y;"));
        }

        [TestMethod]
        public void EvaluateByteArray()
        {
            CollectionAssert.AreEquivalent(
                expected: new byte[] { 1, 2, 3 },
                Evaluator.Evaluate<byte[]>("var x = [1, 2, 3]; x;"));
        }

        [TestMethod]
        public void EvaluateShortArray()
        {
            CollectionAssert.AreEquivalent(
                expected: new short[] { 10, 20 },
                Evaluator.Evaluate<short[]>("var x = [10, 20]; x;"));
        }

        [TestMethod]
        public void EvaluateIntArray()
        {
            CollectionAssert.AreEquivalent(
                expected: new int[] { 100, 200 },
                Evaluator.Evaluate<int[]>("var x = [100, 200]; x;"));
        }

        [TestMethod]
        public void EvaluateDoubleArray()
        {
            CollectionAssert.AreEquivalent(
                expected: new double[] { 1.1, 2.2 },
                Evaluator.Evaluate<double[]>("var x = [1.1, 2.2]; x;"));
        }

        [TestMethod]
        public void EvaluateNullResultThrows()
        {
            Assert.ThrowsException<ArgumentException>(
                () => Evaluator.Evaluate<int>(""));
        }

        [TestMethod]
        public void EvaluateWithNullArgs()
        {
            Assert.AreEqual(expected: 5, Evaluator.Evaluate<int>("var x = 5; x;", null));
        }

        [TestMethod]
        public void EvaluateUndeclaredVariableThrows()
        {
            Assert.ThrowsException<BadNameException>(
                () => Evaluator.Evaluate<int>("y;"));
        }

        [TestMethod]
        public void EvaluateWithForLoop()
        {
            const string source = @"
                var sum = 0;
                for (var i = 1; i <= 5; i++) {
                    sum = sum + i;
                }
                return sum;";

            Assert.AreEqual(expected: 15, Evaluator.Evaluate<int>(source));
        }

        [TestMethod]
        public void EvaluateWithIfElse()
        {
            const string source = @"
                var x = args[0];
                if (x > 0) { return 1; } else { return -1; }";

            Assert.AreEqual(expected: 1, Evaluator.Evaluate<int>(source, 5));
            Assert.AreEqual(expected: -1, Evaluator.Evaluate<int>(source, -5));
        }
    }
}
