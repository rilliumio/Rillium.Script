using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rillium.Script.Test
{
    [TestClass]
    public class TokenTests
    {
        [TestMethod]
        public void ToStringCorrectly()
        {
            const string name = "x";
            Assert.AreEqual(
                $"{TokenId.Identifier}: {name}",
                new Token(TokenId.Identifier, name, 0).ToString());
        }
    }
}
