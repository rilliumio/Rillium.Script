using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rillium.Script.Test
{
    [TestClass]
    public class AsyncEvaluationTests
    {
        [TestMethod]
        public async Task AsyncFunction_EvaluateAsync_ReturnsResult()
        {
            var options = new ScriptOptions()
                .AddFunctionAsync<double, double>("fetchValue", async (double x) =>
                {
                    await Task.Delay(1);
                    return x * 10;
                });

            var result = await Evaluator.EvaluateAsync<int>("fetchValue(5);", options);

            Assert.AreEqual(50, result);
        }

        [TestMethod]
        public async Task AsyncFunction_RunAsync_ReturnsOutput()
        {
            var options = new ScriptOptions()
                .AddFunctionAsync<double, double>("compute", async (double x) =>
                {
                    await Task.Delay(1);
                    return x * 3;
                });

            var (output, console) = await Evaluator.RunAsync("compute(4);", options);

            Assert.AreEqual(12.0, output);
        }

        [TestMethod]
        public async Task MixedSyncAndAsyncFunctions()
        {
            var options = new ScriptOptions()
                .AddFunction("double", (double x) => x * 2)
                .AddFunctionAsync<double, double>("fetchValue", async (double x) =>
                {
                    await Task.Delay(1);
                    return x + 100;
                });

            var result = await Evaluator.EvaluateAsync<int>("double(fetchValue(5));", options);

            // fetchValue(5) = 105, double(105) = 210
            Assert.AreEqual(210, result);
        }

        [TestMethod]
        public async Task AsyncFunction_InVarDeclaration()
        {
            var options = new ScriptOptions()
                .AddFunctionAsync<double, double>("fetchValue", async (double x) =>
                {
                    await Task.Delay(1);
                    return 42.0;
                });

            var result = await Evaluator.EvaluateAsync<int>("var x = fetchValue(0); x;", options);

            Assert.AreEqual(42, result);
        }

        [TestMethod]
        public async Task AsyncFunction_InIfBody()
        {
            var options = new ScriptOptions()
                .AddFunctionAsync<double, double>("fetchValue", async (double x) =>
                {
                    await Task.Delay(1);
                    return x * 2;
                });

            var result = await Evaluator.EvaluateAsync<int>(
                "var x = 0; if (1 > 0) { x = fetchValue(5); } x;", options);

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public async Task AsyncFunction_InForLoopBody()
        {
            var options = new ScriptOptions()
                .AddFunctionAsync<double, double>("fetchValue", async (double x) =>
                {
                    await Task.Delay(1);
                    return x * 2;
                });

            var result = await Evaluator.EvaluateAsync<int>(
                "var sum = 0; for (var i = 0; i < 3; i++) { sum = sum + fetchValue(i); } sum;", options);

            // fetchValue(0)=0, fetchValue(1)=2, fetchValue(2)=4 => sum=6
            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public async Task AsyncFunction_InReturnStatement()
        {
            var options = new ScriptOptions()
                .AddFunctionAsync<double, double>("fetchValue", async (double x) =>
                {
                    await Task.Delay(1);
                    return 99.0;
                });

            var result = await Evaluator.EvaluateAsync<int>("return fetchValue(0);", options);

            Assert.AreEqual(99, result);
        }

        [TestMethod]
        public async Task AsyncFunction_InReturnStatementInt()
        {
            var options = new ScriptOptions()
                .AddFunctionAsync("fetchValue", async (int i) =>
                {
                    await Task.Delay(1);
                    return i + 1;
                });

            var result = await Evaluator.EvaluateAsync<int>("return fetchValue(0);", options);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public async Task AsyncFunction_NestedAsyncCalls()
        {
            var options = new ScriptOptions()
                .AddFunctionAsync<double, double>("fetchValue", async (double x) =>
                {
                    await Task.Delay(1);
                    return x + 1;
                });

            var result = await Evaluator.EvaluateAsync<int>("fetchValue(fetchValue(1));", options);

            // fetchValue(1)=2, fetchValue(2)=3
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public async Task AsyncFunction_InBinaryExpression()
        {
            var options = new ScriptOptions()
                .AddFunctionAsync<double, double>("fetchValue", async (double x) =>
                {
                    await Task.Delay(1);
                    return x * 10;
                });

            var result = await Evaluator.EvaluateAsync<int>("fetchValue(1) + fetchValue(2);", options);

            // fetchValue(1)=10, fetchValue(2)=20 => 30
            Assert.AreEqual(30, result);
        }

        [TestMethod]
        public async Task AsyncFunction_ThatThrows_PropagatesException()
        {
            var options = new ScriptOptions()
                .AddFunctionAsync<double, double>("failingFunc", async (double x) =>
                {
                    await Task.Delay(1);
                    throw new InvalidOperationException("Test error");
#pragma warning disable CS0162
                    return 0.0;
#pragma warning restore CS0162
                });

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
                await Evaluator.EvaluateAsync<int>("failingFunc(1);", options));
        }

        [TestMethod]
        public async Task SyncOnlyScript_ViaRunAsync_WorksIdentically()
        {
            var (output, console) = await Evaluator.RunAsync("1 + 2;");

            Assert.AreEqual(3.0, output);
        }

        [TestMethod]
        public async Task SyncOnlyScript_ViaEvaluateAsync_WorksIdentically()
        {
            var result = await Evaluator.EvaluateAsync<int>("1 + 2;");

            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public async Task AsyncFunction_TwoArgs()
        {
            var options = new ScriptOptions()
                .AddFunctionAsync<double, double, double>("asyncAdd", async (double x, double y) =>
                {
                    await Task.Delay(1);
                    return x + y;
                });

            var result = await Evaluator.EvaluateAsync<int>("asyncAdd(3, 7);", options);

            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public async Task AsyncFunction_InTernaryExpression()
        {
            var options = new ScriptOptions()
                .AddFunctionAsync<double, double>("fetchValue", async (double x) =>
                {
                    await Task.Delay(1);
                    return x * 2;
                });

            // Use async function result in a ternary condition
            var result = await Evaluator.EvaluateAsync<int>(
                "var v = fetchValue(5); var x = v > 0 ? 1 : 2; x;", options);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public async Task AddActionAsync_ZeroArgs_SideEffectExecuted()
        {
            bool executed = false;
            var options = new ScriptOptions()
                .AddActionAsync("doSomethingAsync", async () =>
                {
                    await Task.Delay(1);
                    executed = true;
                });

            await Evaluator.EvaluateAsync<int>("doSomethingAsync(); 0;", options);

            Assert.IsTrue(executed);
        }

        [TestMethod]
        public async Task AddActionAsync_SingleArg_SideEffectExecuted()
        {
            double captured = 0;
            var options = new ScriptOptions()
                .AddActionAsync<double>("captureAsync", async (double x) =>
                {
                    await Task.Delay(1);
                    captured = x;
                });

            await Evaluator.EvaluateAsync<int>("captureAsync(99); 0;", options);

            Assert.AreEqual(99.0, captured);
        }

        [TestMethod]
        public async Task AddActionAsync_TwoArgs_SideEffectExecuted()
        {
            double sum = 0;
            var options = new ScriptOptions()
                .AddActionAsync<double, double>("addSideEffectAsync", async (double x, double y) =>
                {
                    await Task.Delay(1);
                    sum = x + y;
                });

            await Evaluator.EvaluateAsync<int>("addSideEffectAsync(4, 6); 0;", options);

            Assert.AreEqual(10.0, sum);
        }

        [TestMethod]
        public async Task AddFunctionAsync_ZeroArgs_ReturnsResult()
        {
            var options = new ScriptOptions()
                .AddFunctionAsync<double>("getValue", async () =>
                {
                    await Task.Delay(1);
                    return 42.0;
                });

            var result = await Evaluator.EvaluateAsync<int>("getValue();", options);

            Assert.AreEqual(42, result);
        }
    }
}
