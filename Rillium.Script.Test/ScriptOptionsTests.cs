using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rillium.Script.Exceptions;

namespace Rillium.Script.Test
{
    [TestClass]
    public class ScriptOptionsTests
    {
        [TestMethod]
        public void CustomFunction_SingleArg_ReturnsResult()
        {
            var options = new ScriptOptions()
                .AddFunction("double", (double x) => x * 2);

            var result = Evaluator.Evaluate<int>("double(5);", options);

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void CustomFunction_TwoArgs_PassesArgumentsCorrectly()
        {
            var options = new ScriptOptions()
                .AddFunction("add", (double x, double y) => x + y);

            var result = Evaluator.Evaluate<int>("add(3, 7);", options);

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void CustomFunction_InArithmeticExpression()
        {
            var options = new ScriptOptions()
                .AddFunction("triple", (double x) => x * 3);

            var result = Evaluator.Evaluate<int>("triple(3) + 10;", options);

            Assert.AreEqual(19, result);
        }

        [TestMethod]
        public void CustomFunction_AsArgumentToBuiltIn()
        {
            var options = new ScriptOptions()
                .AddFunction("negate", (double x) => -x);

            var result = Evaluator.Evaluate<int>("Abs(negate(5));", options);

            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void Compile_WithoutOptions_StillWorks()
        {
            var result = Evaluator.Evaluate<int>("Abs(-1);");

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void ScriptOptions_ReusableAcrossMultipleCompiles()
        {
            var options = new ScriptOptions()
                .AddFunction("double", (double x) => x * 2);

            var result1 = Evaluator.Evaluate<int>("double(5);", options);
            var result2 = Evaluator.Evaluate<int>("double(10);", options);

            Assert.AreEqual(10, result1);
            Assert.AreEqual(20, result2);
        }

        [TestMethod]
        public void UnregisteredFunction_ThrowsBadNameException()
        {
            Assert.ThrowsException<BadNameException>(() =>
                Evaluator.Evaluate<int>("unknownFunc(1);"));
        }

        [TestMethod]
        public void CustomFunction_InVarDeclaration()
        {
            var options = new ScriptOptions()
                .AddFunction("square", (double x) => x * x);

            var result = Evaluator.Evaluate<int>("var x = square(4); x;", options);

            Assert.AreEqual(16, result);
        }

        [TestMethod]
        public void CustomFunction_InForLoop()
        {
            var options = new ScriptOptions()
                .AddFunction("double", (double x) => x * 2);

            var result = Evaluator.Evaluate<int>(
                "var sum = 0; for (var i = 0; i < 3; i++) { sum = sum + double(i); } sum;", options);

            // double(0) + double(1) + double(2) = 0 + 2 + 4 = 6
            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public void CustomFunction_InIfCondition()
        {
            var options = new ScriptOptions()
                .AddFunction("double", (double x) => x * 2);

            var result = Evaluator.Evaluate<int>(
                "var x = 0; if (double(5) > 8) { x = 1; } else { x = 2; } x;", options);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void CustomFunction_NestedCalls()
        {
            var options = new ScriptOptions()
                .AddFunction("double", (double x) => x * 2);

            var result = Evaluator.Evaluate<int>("double(double(3));", options);

            Assert.AreEqual(12, result);
        }

        [TestMethod]
        public void CustomFunction_WithReturn()
        {
            var options = new ScriptOptions()
                .AddFunction("double", (double x) => x * 2);

            var result = Evaluator.Evaluate<int>("return double(7);", options);

            Assert.AreEqual(14, result);
        }

        [TestMethod]
        public void CustomFunction_MultipleFunctionsRegistered()
        {
            var options = new ScriptOptions()
                .AddFunction("double", (double x) => x * 2)
                .AddFunction("add", (double x, double y) => x + y);

            var result = Evaluator.Evaluate<int>("add(double(3), double(4));", options);

            // double(3)=6, double(4)=8, add(6,8)=14
            Assert.AreEqual(14, result);
        }

        [TestMethod]
        public void AddAction_ZeroArgs_SideEffectExecuted()
        {
            bool executed = false;
            var options = new ScriptOptions()
                .AddAction("doSomething", () => { executed = true; });

            Evaluator.Evaluate<int>("doSomething(); 0;", options);

            Assert.IsTrue(executed);
        }

        [TestMethod]
        public void AddAction_SingleArg_SideEffectExecuted()
        {
            double captured = 0;
            var options = new ScriptOptions()
                .AddAction("capture", (double x) => { captured = x; });

            Evaluator.Evaluate<int>("capture(42); 0;", options);

            Assert.AreEqual(42.0, captured);
        }

        [TestMethod]
        public void AddAction_TwoArgs_SideEffectExecuted()
        {
            double sum = 0;
            var options = new ScriptOptions()
                .AddAction("addSideEffect", (double x, double y) => { sum = x + y; });

            Evaluator.Evaluate<int>("addSideEffect(3, 7); 0;", options);

            Assert.AreEqual(10.0, sum);
        }

        [TestMethod]
        public void AsyncFunction_CalledViaSyncEvaluate_ThrowsAsyncFunctionCalledSynchronouslyException()
        {
            var options = new ScriptOptions()
                .AddFunctionAsync("asyncFunc", async (double x) =>
                {
                    await Task.Delay(1);
                    return x * 2;
                });

            Assert.ThrowsException<AsyncFunctionCalledSynchronouslyException>(() =>
                Evaluator.Evaluate<int>("asyncFunc(5);", options));
        }

        [TestMethod]
        public void AsyncFunction_CalledViaSyncRun_ThrowsAsyncFunctionCalledSynchronouslyException()
        {
            var options = new ScriptOptions()
                .AddFunctionAsync("asyncFunc", async (double x) =>
                {
                    await Task.Delay(1);
                    return x * 2;
                });

            Assert.ThrowsException<AsyncFunctionCalledSynchronouslyException>(() =>
                Evaluator.Run("asyncFunc(5);", options));
        }

        [TestMethod]
        public void LiteralTypeIdResolver_UnsupportedType_ThrowsArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new ScriptOptions().AddFunction("bad", (object x) => x));
        }
    }
}
