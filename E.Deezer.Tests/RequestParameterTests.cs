using NUnit.Framework;

namespace E.Deezer.Tests
{
    [TestFixture]
    class RequestParameterTests
    {
        [Test]
        public void GetAccessTokenParamter()
        {
            var actual = RequestParameter.GetAccessTokenParamter("245bmueho57");

            Assert.IsNotNull(actual);
            Assert.AreEqual("access_token", actual.Id, "Id");
            Assert.AreEqual("245bmueho57", actual.Value, "Value");
            Assert.AreEqual(ParameterType.QueryString, actual.Type, "Type");
        }

        [Test]
        public void GetNewQueryStringParameter()
        {
            var actual = RequestParameter.GetNewQueryStringParameter("address", new { number = 2 });

            Assert.IsNotNull(actual);
            Assert.AreEqual("address", actual.Id, "Id");
            Assert.AreEqual(new { number = 2 }, actual.Value, "Value");
            Assert.AreEqual(ParameterType.QueryString, actual.Type, "Type");
        }

        [Test]
        public void GetNewUrlSegmentParamter()
        {
            var actual = RequestParameter.GetNewUrlSegmentParamter("user", new { ID = 10 });

            Assert.IsNotNull(actual);
            Assert.AreEqual("user", actual.Id, "Id");
            Assert.AreEqual(new { ID = 10 }, actual.Value, "Value");
            Assert.AreEqual(ParameterType.UrlSegment, actual.Type, "Type");
        }

        [Test]
        public void EmptyListReturnsAnEmptyCollection()
        {
            var actual = RequestParameter.EmptyList;

            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }
    }
}
