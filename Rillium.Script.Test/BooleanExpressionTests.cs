using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Rillium.Script.Test
{
    [TestClass]
    public class BooleanExpressionTests
    {
        [TestMethod]
        public void EqualityTest()
        {
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("1"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("1;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return 1;"));

            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("1==1"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("1==1;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("(1==1)"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("(1==1);"));

            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return 1==1;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return (1==1);"));

            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("-1==-1"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("-1==-1;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return -1==-1;"));
        }

        [TestMethod]
        public void EqualityCompairingNegativeNumbersTest()
        {
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("0"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("-1"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("-1;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return -1;"));

            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("-1==-1"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("-1==-1;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("(-1==-1)"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("(-1==-1);"));

            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return -1==-1;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return (-1==-1);"));
        }


        [TestMethod]
        public void GreaterThanTest()
        {
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("1>0"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("1>0;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("(1>0)"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("(1>0);"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return 1>0;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return (1>0);"));

            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("0>1"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("0>1;"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("(0>1)"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("(0>1);"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("return 0>1;"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("return (0>1);"));
        }

        [TestMethod]
        public void GreaterThanNegativesTest()
        {
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("-2>-1"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("-2>-1;"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("(-2>-1)"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("(-2>-1);"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("return -2>-1;"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("return (-2>-1);"));

            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("-1>-2"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("-1>-2;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("(-1>-2)"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("(-1>-2);"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return -1>-2;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return (-1>-2);"));
        }

        [TestMethod]
        public void GreaterEqualTest()
        {
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("1>=0"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("1>=0;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("(1>=0)"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("(1>=0);"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return 1>=0;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return (1>=0);"));

            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("0>=1"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("0>=1;"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("(0>=1)"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("(0>=1);"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("return 0>=1;"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("return (0>=1);"));
        }

        [TestMethod]
        public void GreaterEquaNegativeslTest()
        {
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("-1>=-2"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("-1>=-2;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("(-1>=-2)"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("(-1>=-2);"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return -1>=-2;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return (-1>=-2);"));

            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("-2>=-1"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("-2>=-1;"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("(-2>=-1)"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("(-2>=-1);"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("return -2>=-1;"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("return (-2>=-1);"));
        }

        [TestMethod]
        public void LessTest()
        {
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("0<1"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("0<1;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("(0<1)"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("(0<1);"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return 0<1;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return (0<1);"));

            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("1<0"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("1<0;"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("(1<0)"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("(1<0);"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("return 1<0;"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("return (1<0);"));
        }

        [TestMethod]
        public void LessNegativesTest()
        {
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("-2<1"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("-2<1;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("(-2<1)"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("(-2<1);"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return -2<1;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return (-2<1);"));

            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("-1<-2"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("-1<-2;"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("(-1<-2)"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("(-1<-2);"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("return -1<-2;"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("return (-1<-2);"));
        }

        [TestMethod]
        public void LessEqualTest()
        {
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("0<=0"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("1<=1"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("0<=1"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("0<=1;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("(0<=1)"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("(0<=1);"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return 0<=1;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return (0<=1);"));

            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("1<=0"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("1<=0;"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("(1<=0)"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("(1<=0);"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("return 1<=0;"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("return (1<=0);"));
        }

        [TestMethod]
        public void LessEqualNegativesTest()
        {
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("-2<=-2"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("-1<=-1"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("-2<=-1"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("-2<=-1;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("(-2<=-1)"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("(-2<=-1);"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return -2<=-1;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return (-2<=-1);"));

            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("-1<=-2"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("-1<=-2;"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("(-1<=-2)"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("(-1<=-2);"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("return -1<=-2;"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("return (-1<=-2);"));
        }
    }
}
