using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Rillium.Script.Test
{
    [TestClass]
    public class BooleanExpressionTests
    {
        [TestMethod]
        public void EqualityTest()
        {
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return true;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("true"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("true;"));

            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("false"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("false;"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("return false;"));

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
        public void BangEqualityTest()
        {
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("1!=1"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("1!=1;"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("(1!=1)"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("(1!=1);"));

            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("1!=2"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("1!=2;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("(1!=2)"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("(1!=2);"));

            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("return 1!=1;"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("return (1!=1);"));

            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return 1!=2;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return (1!=2);"));

            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("-1!=-1"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("-1!=-1;"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("return -1!=-1;"));

            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("-1!=-2"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("-1!=-2;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return -1!=-2;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("return (-1!=-2);"));
        }

        [TestMethod]
        public void BoolEquality()
        {
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("true == true;"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("true == false;"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("false == true;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("false == false;"));

            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("true != true;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("true != false;"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("false != true;"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("false != false;"));

            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("(true == true);"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("(true == false);"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("(false == true);"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("(false == false);"));

            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("(true != true);"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("(true != false);"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("(false != true);"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("(false != false);"));
        }

        [TestMethod]
        public void EqualityComparingNegativeNumbersTest()
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
        public void GreaterEqualNegativesTest()
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

        [TestMethod]
        public void StringEqualityCorrectly()
        {
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("('a'=='a')"));

            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("'a'=='a'"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("'a'=='b'"));
            Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("'a'!='a'"));
            Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("'a'!='b'"));

            //Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("('a'=='b')"));
            //Assert.AreEqual(expected: false, Evaluator.Evaluate<bool>("('a'!='a')"));
            //Assert.AreEqual(expected: true, Evaluator.Evaluate<bool>("('a'!='b')"));
        }
    }
}
