using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rillium.Script.Test
{
    [TestClass]
    public class StringTests
    {
        [TestMethod]
        public void StringsLiteralCorrectly()
        {
            Assert.AreEqual(expected: "foo", Evaluator.Evaluate<string>("\"foo\""));
            Assert.AreEqual(expected: "foo", Evaluator.Evaluate<string>("'foo'"));
        }

        [TestMethod]
        public void LiteralStringInCorrectly()
        {
            TestHelpers.ShouldThrowWithMessage<ScriptException>("\"foo", "Line 1. \" expected.");
            TestHelpers.ShouldThrowWithMessage<ScriptException>("'foo", "Line 1. ' expected.");
        }

        [TestMethod]
        public void DeclareStringsCorrectly()
        {
            Assert.AreEqual(expected: "foo", Evaluator.Evaluate<string>("var x = \"foo\"; x;"));
            Assert.AreEqual(expected: "foo", Evaluator.Evaluate<string>("var x = 'foo'; x;"));
        }

        [TestMethod]
        public void DeclareAndReturnStringsCorrectly()
        {
            Assert.AreEqual(expected: "foo", Evaluator.Evaluate<string>("var x = \"foo\"; return x;"));
            Assert.AreEqual(expected: "foo", Evaluator.Evaluate<string>("var x = 'foo'; return x;"));
        }


        [TestMethod]
        public void ConcatenateCorrectly()
        {
            Assert.AreEqual(expected: "ab", Evaluator.Evaluate<string>("var x = \"a\" + \"b\"; return x;"));
            Assert.AreEqual(expected: "ab", Evaluator.Evaluate<string>("var x = 'a' + 'b'; return x;"));
        }
    }
}
