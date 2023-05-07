namespace Rillium.Script.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var m = new ScriptEvaluator("1+(2+2)");
            var d = m.EvaluateExpression();
            Assert.AreEqual(5, d);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var m = new ScriptEvaluator("2*(1+2)");
            var d = m.EvaluateExpression();
            Assert.AreEqual(6, d);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var m = new ScriptEvaluator("2/(1+2)");
            var d = m.EvaluateExpression();
            Assert.AreEqual(2.0 / 3.0, d);
        }

        [TestMethod]
        public void TestMethod4()
        {
            var m = new ScriptEvaluator("var x=0;x;");
            var d = m.EvaluateExpression();
            Assert.AreEqual(0, d);
        }

        [TestMethod]
        public void DeclareTest()
        {
            var m = new ScriptEvaluator("" +
                "var x =0; if(1==1){ x=1; }else{ x=2;}; x;");
            m.EvaluateStatement();
        }
    }
}