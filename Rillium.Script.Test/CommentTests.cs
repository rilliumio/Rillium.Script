using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rillium.Script.Test
{
    [TestClass]
    public class CommentTests
    {
        [TestMethod]
        public void CommentsCorrectly()
        {
            var source = @"
            //
            //
            var d = 1; // end of line comment.
            // full line comment
            for (var i = 0; i < 10; i++)  // end of line comment.
            { d = d * 2; } // end of line comment.
            return d; // end of line comment.
            ";

            Assert.AreEqual(expected: 1024, Evaluator.Evaluate<int>(source));
        }

        [TestMethod]
        public void BlockCommentsCorrectly()
        {
            var source = @"
            /*
               multi-line
               block comment.
            */

            var d = 1; /* end of line block comment */
            /* single line block comment */
            for (var i = 0; i < 10; i++) /* end of line block comment */
            { d = d * 2; } /* end of line block comment */

            /*
               multi-line
               block comment.
            */

            return d; /* end of line block comment */
            ";

            Assert.AreEqual(expected: 1024, Evaluator.Evaluate<int>(source));
        }

        [TestMethod]
        public void BlockCommentsInLineCorrectly()
        {
            var source = @"
            var  /* in-line block comment */ d = 1;
            for (var i = 0; /* in-line block comment */ i < 10; i++)  // end of line comment.
            { d = d * /* in-line block comment */ 2; /* in-line block comment */ }
            return /* in-line block comment */ d;
            ";

            Assert.AreEqual(expected: 1024, Evaluator.Evaluate<int>(source));
        }

        [TestMethod]
        public void NonTerminatedBlockCommentsCorrectly()
        {
            var source = @"
            /* unterminated block comment
            var d = 1;
            for (var i = 0; i < 10; i++)
            { d = d * 2; }
            return d; 
            ";

            TestHelpers.ShouldThrowWithMessage<ScriptException>(source, Constants.ExceptionMessages.UnterminatedBlockComment);
        }
    }
}
