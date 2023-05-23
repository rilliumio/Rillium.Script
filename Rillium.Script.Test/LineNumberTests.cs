using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rillium.Script.Exceptions;

namespace Rillium.Script.Test
{
    [TestClass]
    public class LineNumberTests
    {
        [TestMethod]
        public void ErrorOnLineNumberCorrectly()
        {
            const string expected = "Line 5. The name 'b' does not exist in the current context.";

            const string source = @"
                var n = 0;
                for (var i = 0; i < 10; i = i + 1)
                {
                    b = 2;
                }
                ";

            TestHelpers.ShouldThrowWithMessage<BadNameException>(source, expected);
        }

        [TestMethod]
        public void ErrorOnLineNumber2Correctly()
        {
            const string expected = "Line 3. The name 'b' does not exist in the current context.";

            const string source = @"
                var n = 0; for (var i = 0; i < 10; i = i + 1) {
                    b = 2;
                }
                ";

            TestHelpers.ShouldThrowWithMessage<BadNameException>(source, expected);
        }
    }
}
