using NUnit.Framework;

namespace E.Deezer.Tests
{
    [TestFixture]
    class DeezerSessionTests
    {
        [Test]
        public void TestCreateNew()
        {
            var actual = DeezerSession.CreateNew();

            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsAuthenticated);
        }

        [Test]
        public void EndPointConstantVariableDidnotChanged()
        {
            Assert.AreEqual("https://api.deezer.com/", DeezerSession.ENDPOINT);
            Assert.Warn("This field no longer in use.");
        }
    }
}
