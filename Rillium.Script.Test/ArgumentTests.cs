using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rillium.Script.Exceptions;

namespace Rillium.Script.Test
{

    [TestClass]
    public class ArgumentTests
    {
        [TestMethod]
        public void EntryArgumentsTypesCorrectly()
        {
            const string source = "args[0];";

            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>(source, true));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>(source, false));
            Assert.AreEqual(expected: byte.MaxValue, Evaluator.Evaluate<byte>(source, byte.MaxValue));
            Assert.AreEqual(expected: short.MaxValue, Evaluator.Evaluate<short>(source, short.MaxValue));
            Assert.AreEqual(expected: 5.5d, Evaluator.Evaluate<double>(source, 5.5d));
            Assert.AreEqual(expected: 1, Evaluator.Evaluate<long>(source, (long)1));
            Assert.AreEqual(expected: decimal.Zero, Evaluator.Evaluate<decimal>(source, decimal.Zero));
            Assert.AreEqual(expected: 5, Evaluator.Evaluate<int>(source, 5));
            Assert.AreEqual(expected: "foo", Evaluator.Evaluate<string>(source, "foo"));
        }

        [TestMethod]
        public void EntryArgumentsCorrectly()
        {
            const string source = @"
               var i = args[0] + args[1] + args[2];
               return i;";

            Assert.AreEqual(expected: (5 + 6 + 7), Evaluator.Evaluate<int>(source, 5, 6, 7));
        }

        [TestMethod]
        public void EntryArgumentOfBoundShouldThrow() =>
            TestHelpers.ShouldThrowWithMessage<ScriptException>(
                "args[2]",
                "Line 1. Index was outside the bounds of the array.");

        [TestMethod]
        public void EntryArgumentReturnOutOfBoundShouldThrow() =>
           TestHelpers.ShouldThrowWithMessage<ScriptException>(
               "return args[2]",
               "Line 1. Index was outside the bounds of the array.");

        [TestMethod]
        public void EntryArgumentsNegativeShouldThrow() =>
          TestHelpers.ShouldThrowWithMessage<ScriptException>(
              "return args[-1]",
              "Line 1. Invalid indexing an array with a negative number. Array indices should not be less than zero.");

        [TestMethod]
        public void EntryArgumentsReturnEmptyIndexShouldThrow() =>
            TestHelpers.ShouldThrowWithMessage<SyntaxException>(
                "return args[]", "Line 1. Value expected.");

        [TestMethod]
        public void EntryArgumentsEmptyIndexShouldThrow() =>
            TestHelpers.ShouldThrowWithMessage<SyntaxException>(
                "args[]", "Line 1. Value expected.");


        [TestMethod]
        public void EntryArgumentsCorrectly1()
        {
            const string source = @"
               if(args[1]) { return args[0];} else { return args[7];}";

            Assert.AreEqual(expected: 5, Evaluator.Evaluate<int>(source, 5, true, 7));
        }

        [TestMethod]
        public void ArgumentEqualityCorrectly()
        {
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("'x' == args[0];", "x"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("args[0] == 'x';", "x"));

            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("false == args[0];", true));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("args[0] == false;", true));

            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("args[0] == true;", true));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("true == args[0];", true));

            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("args[0] == 1;", 1));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("1 == args[0];", 1));

            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("args[0] > 0;", 1));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("0 > args[0];", 1));
        }

        [TestMethod]
        public void ArgumentBangEqualityCorrectly()
        {
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("'x' != args[0];", "x"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("args[0] != 'x';", "x"));

            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("false != args[0];", true));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("args[0] != false;", true));

            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("args[0] != true;", true));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("true != args[0];", true));

            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("args[0] != 1;", 1));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("1 != args[0];", 1));
        }
    }
}
