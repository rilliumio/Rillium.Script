using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rillium.Script.Test
{
    [TestClass]
    public class FunctionTests
    {
        [TestMethod]
        public void MathFunctionTest()
        {
            ShouldEvaluateEqual("Abs(-1)", Math.Abs(-1.0));
            ShouldEvaluateEqual("Acos(0.002)", Math.Acos(0.002));
            ShouldEvaluateEqual("Acosh(45)", Math.Acosh(45));
            ShouldEvaluateEqual("Asin(45)", Math.Asin(45));
            ShouldEvaluateEqual("Asinh(45)", Math.Asinh(45));

            ShouldEvaluateEqual("Atan(45)", Math.Atan(45));
            ShouldEvaluateEqual("Atan2(45, 12.2)", Math.Atan2(45, 12.2));
            ShouldEvaluateEqual("Atanh(45)", Math.Atanh(45));
            ShouldEvaluateEqual("BitDecrement(45)", Math.BitDecrement(45));
            ShouldEvaluateEqual("BitIncrement(45)", Math.BitIncrement(45));

            ShouldEvaluateEqual("Cbrt(45)", Math.Cbrt(45));
            ShouldEvaluateEqual("Ceiling(45.0)", Math.Ceiling(45.0));
            ShouldEvaluateEqual("Clamp(1.1,2,3)", Math.Clamp(1.1, 2, 3));
            ShouldEvaluateEqual("CopySign(1,2)", Math.CopySign(1, 2));
            ShouldEvaluateEqual("Cos(25)", Math.Cos(25));

            ShouldEvaluateEqual("Cosh(25)", Math.Cosh(25));
            ShouldEvaluateEqual("E()", Math.E);
            ShouldEvaluateEqual("Exp(22.1)", Math.Exp(22.1));
            ShouldEvaluateEqual("Floor(22.1)", Math.Floor(22.1));
            ShouldEvaluateEqual("FusedMultiplyAdd(1.1, 2.2, 3.3)", Math.FusedMultiplyAdd(1.1, 2.2, 3.3));

            ShouldEvaluateEqual("IEEERemainder(2.2, 3.3)", Math.IEEERemainder(2.2, 3.3));
            ShouldEvaluateEqual("ILogB(25)", Math.ILogB(25));
            ShouldEvaluateEqual("Log(25.2)", Math.Log(25.2));
            ShouldEvaluateEqual("Log10(25.2)", Math.Log10(25.2));
            ShouldEvaluateEqual("Log2(25.2)", Math.Log2(25.2));

            ShouldEvaluateEqual("Max(100.1, -1.1)", Math.Max(100.1, -1.1));
            ShouldEvaluateEqual("MaxMagnitude(100.1, -1.1)", Math.MaxMagnitude(100.1, -1.1));
            ShouldEvaluateEqual("Min(100.1, -1.1)", Math.Min(100.1, -1.1));
            ShouldEvaluateEqual("MinMagnitude(100.1, -1.1)", Math.MinMagnitude(100.1, -1.1));
            ShouldEvaluateEqual("PI()", Math.PI);

            ShouldEvaluateEqual("Pow(1.2,1.3)", Math.Pow(1.2, 1.3));
            ShouldEvaluateEqual("ReciprocalEstimate(1.2)", Math.ReciprocalEstimate(1.2));
            ShouldEvaluateEqual("ReciprocalSqrtEstimate(1.2)", Math.ReciprocalSqrtEstimate(1.2));
            ShouldEvaluateEqual("Round(1.2)", Math.Round(1.2));
            ShouldEvaluateEqual("Round(1.123456, 4)", Math.Round(1.123456, 4));

            ShouldEvaluateEqual("ScaleB(1.123456, 4)", Math.ScaleB(1.123456, 4));
            ShouldEvaluateEqual("Sign(25.2)", Math.Sign(25.2));
            ShouldEvaluateEqual("Sin(25.2)", Math.Sin(25.2));
            ShouldEvaluateEqual("Sinh(25.2)", Math.Sinh(25.2));
            ShouldEvaluateEqual("Sqrt(25.2)", Math.Sqrt(25.2));

            ShouldEvaluateEqual("Tan(25.2)", Math.Tan(25.2));
            ShouldEvaluateEqual("Tanh(25.2)", Math.Tanh(25.2));
            ShouldEvaluateEqual("Truncate(25.2)", Math.Truncate(25.2));
            ShouldEvaluateEqual("Tau()", Math.Tau);
        }

        [TestMethod]
        public void MathFunctionsAssignmentTests()
        {
            Assert.AreEqual(expected: 1, Evaluator.Evaluate<int>("var x = Abs(-1); x;"));
            Assert.AreEqual(expected: 1, Evaluator.Evaluate<int>("var x = -1; var y = Abs(x); y;"));
            Assert.AreEqual(expected: Math.Abs(-1.1), Evaluator.Evaluate<double>("var x = Abs(-1.1); x;"));
            Assert.AreEqual(expected: Math.Abs(-1.1), Evaluator.Evaluate<double>("var x = -1.1; var y = Abs(x); y;"));

            Assert.AreEqual(expected: Math.Max(25, 2), Evaluator.Evaluate<int>("var x = Max(25, 2); x;"));
            Assert.AreEqual(expected: Math.Max(-25, 2), Evaluator.Evaluate<int>("var x = Max(-25, 2); x;"));
            Assert.AreEqual(expected: Math.Max(-25.1, 2), Evaluator.Evaluate<double>("var x = Max(-25.1, 2); x;"));
            Assert.AreEqual(expected: Math.Max(Math.Abs(-25), 2), Evaluator.Evaluate<int>("var x = Max(Abs(-25), 2); x;"));
            Assert.AreEqual(expected: Math.Max(-25, 2.1), Evaluator.Evaluate<double>("var x = Max(-25, 2.1); x;"));

            Assert.AreEqual(expected: Math.Min(25, 2), Evaluator.Evaluate<int>("var x = Min(25, 2); x;"));
            Assert.AreEqual(expected: Math.Min(-25, 2), Evaluator.Evaluate<int>("var x = Min(-25, 2); x;"));
            Assert.AreEqual(expected: Math.Min(-25.1, 2), Evaluator.Evaluate<double>("var x = Min(-25.1, 2); x;"));
            Assert.AreEqual(expected: Math.Min(Math.Abs(-25), 2), Evaluator.Evaluate<int>("var x = Min(Abs(-25), 2); x;"));
            Assert.AreEqual(expected: Math.Min(-25, 2.1), Evaluator.Evaluate<double>("var x = Min(-25, 2.1); x;"));
        }

        [TestMethod]
        public void MathFunctionExpressionTests()
        {
            Assert.AreEqual(expected: ((1) + (Math.Log(1.5)) * 3), Evaluator.Evaluate<double>("((1) + (Log(1.5)) * 3)"));
            Assert.AreEqual(expected: (1 + Math.Log(1.5) * 3), Evaluator.Evaluate<double>("(1 + Log(1.5) * 3)"));
            Assert.AreEqual(expected: ((1) + Math.Log(1.5) * 3), Evaluator.Evaluate<double>("((1) + Log(1.5) * 3)"));

            Assert.AreEqual(expected: 1 + Math.Log(1.5) + 1, Evaluator.Evaluate<double>("1 + Log(1.5) + 1"));
            Assert.AreEqual(expected: Math.Log(1.5) + 1, Evaluator.Evaluate<double>("Log(1.5) + 1"));
            Assert.AreEqual(expected: 1 + (Math.Log(1.5)) * 3, Evaluator.Evaluate<double>("1 + (Log(1.5)) * 3"));
            Assert.AreEqual(expected: (1 + Math.Log(1.5)) * 3, Evaluator.Evaluate<double>("(1 + Log(1.5)) * 3"));
            Assert.AreEqual(expected: 1 + (Math.Log(1.5) * 3), Evaluator.Evaluate<double>("1 + (Log(1.5) * 3)"));
            Assert.AreEqual(expected: (1 + ((Math.Log(1.5))) * 3), Evaluator.Evaluate<double>("(1 + ((Log(1.5))) * 3)"));
            Assert.AreEqual(expected: ((1.0) + ((Math.Log(1.5))) * 3.0), Evaluator.Evaluate<double>("((1.0) + ((Log(1.5))) * 3.0)"));
        }

        private static void ShouldEvaluateEqual<T>(string source, T Expected) =>
            TestHelpers.ShouldEvaluateEqual(source, Expected);
    }
}
