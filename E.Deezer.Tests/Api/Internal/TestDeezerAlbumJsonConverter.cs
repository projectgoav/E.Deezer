using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using NUnit.Framework;

using E.Deezer.Api;

namespace E.Deezer.Tests.Api.Internal
{
    [TestFixture]
    public class TestDeezerAlbumJsonConverter
    {

        [Test]
        public void TestWithoutError()
        {
            var json = @"{ 'id' : 123 }"; //Minimal properties
            var subject = new DeezerObjectResponseJsonDeserializer();

            var response = JsonConvert.DeserializeObject<AlbumObjectResponse>(json, subject);

            Assert.NotNull(response);

            Assert.IsNull(response.Error);

            Assert.AreEqual(123ul, response.Object.Id);
        }

        [Test]
        public void TestWithError()
        {
            var json = @"{ 'error' : { 'type' : 'NotFound', 'code' : 404 } }";
            var subject = new DeezerObjectResponseJsonDeserializer();

            var response = JsonConvert.DeserializeObject<AlbumObjectResponse>(json, subject);

            Assert.NotNull(response);

            Assert.NotNull(response.Error);

            Assert.AreEqual("NotFound", response.Error.Type);
            Assert.AreEqual(404, response.Error.Code);
        }
    }
}
