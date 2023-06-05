using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rillium.Script.Exceptions;

namespace Rillium.Script.Test
{
    [TestClass]
    public class FunctionTableTests
    {
        [TestMethod]
        public void Test()
        {
            var expected = "Unknown function name 'foo'.";
            var functionTable = new FunctionTable();

            TestHelpers.ShouldThrowWithMessage<BadNameException>(
                expected, () => functionTable.GetFunction("foo", 7));
        }

        [TestMethod]
        public void AddNullArguments()
        {
            var expected = "No overload of function 'Abs' that takes 7 arguments.";
            var functionTable = new FunctionTable();

            TestHelpers.ShouldThrowWithMessage<ScriptException>(
                expected, () => functionTable.GetFunction(nameof(Math.Abs), 7));
        }
    }
}
