using Abacus;
using NUnit.Framework;

namespace AbacusTest
{
    [TestFixture()]
    public class ParserTest
    {
        [Test()]
        public void TestParser()
        {
            Parser p = new Parser("1");
            var result = p.parse();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Start);
            Assert.AreEqual(1, result.End);
        }

        [Test()]
        public void TestParser_Restartable()
        {
            Parser p = new Parser(" 1");
            var result = p.parse();

            Assert.IsNotNull(result.Expr);
            Assert.AreEqual(1, result.Start);
            Assert.AreEqual(2, result.End);
        }

        [Test()]
        public void TestParser_Dot()
        {
            Parser p = new Parser(".");
            var result = p.parse();

            Assert.IsNull(result.Expr);
            Assert.AreEqual(1, result.Start);
            Assert.AreEqual(1, result.End);
        }

        [Test()]
        public void TestParser_Dot_Ws_Front()
        {
            Parser p = new Parser(" .");
            var result = p.parse();

            Assert.IsNull(result.Expr);
            Assert.AreEqual(2, result.Start);
            Assert.AreEqual(2, result.End);
        }

        [Test()]
        public void TestParser_Dot_Ws_Back()
        {
            Parser p = new Parser(". ");
            var result = p.parse();

            Assert.IsNull(result.Expr);
            Assert.AreEqual(2, result.Start);
            Assert.AreEqual(2, result.End);
        }

        [Test()]
        public void TestParser_DoubleDiv()
        {
            Parser p = new Parser("1//1");
            var result = p.parse();
            Assert.IsInstanceOf(typeof(OperatorExpression), result.Expr);
        }

        [Test()]
        public void TestParser_Mod()
        {
            Parser p = new Parser("54 % 12");
            var result = p.parse();
            Assert.IsInstanceOf(typeof(OperatorExpression), result.Expr);
            OperatorExpression expr = (OperatorExpression)result.Expr;
            Assert.AreEqual("54", expr.Left.ToString());
            Assert.AreEqual("12", expr.Right.ToString());
        }
    }
}
