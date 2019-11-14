using E.Deezer.Api;
using Newtonsoft.Json;
using NUnit.Framework;

namespace E.Deezer.Tests.Unit.Api.Internal
{
    [TestFixture]
    public class TestDeezerObjectDeserializer
    {
        [Test]
        public void TestAlbum()
        {
            var json = @"{ 'id' : 123 }"; //Minimal properties

            var response = GetWithoutErrorFromJson<AlbumObjectResponse>(json);

            Assert.AreEqual(123ul, response.Object.Id);
        }

        [Test]
        public void TestAlbumWithError()
        {
            var json = @"{ 'error' : { 'type' : 'NotFound', 'code' : 404 } }"; // Minimal properties

            GetWithErrorFromJson<AlbumObjectResponse>(json, "NotFound", 404);
        }

        [Test]
        public void TestTrack()
        {
            var json = @"{ 'id' : 789 }"; //Minimal properties

            var response = GetWithoutErrorFromJson<TrackObjectResponse>(json);

            Assert.AreEqual(789ul, response.Object.Id);
        }

        [Test]
        public void TestTrackWithError()
        {
            var json = @"{ 'error' : { 'type' : 'QuotaExceeded', 'code' : 9000 } }"; // Minimal properties

            GetWithErrorFromJson<TrackObjectResponse>(json, "QuotaExceeded", 9000);
        }

        [Test]
        public void TestPlaylist()
        {
            var json = @"{ 'id' : 13034431 }"; //Minimal properties

            var response = GetWithoutErrorFromJson<PlaylistObjectResponse>(json);

            Assert.AreEqual(13034431ul, response.Object.Id);
        }

        [Test]
        public void TestPlaylistWithError()
        {
            var json = @"{ 'error' : { 'type' : 'BadQuery', 'code' : 300 } }"; // Minimal properties

            GetWithErrorFromJson<PlaylistObjectResponse>(json, "BadQuery", 300);
        }

        [Test]
        public void TestRadio()
        {
            var json = @"{ 'id' : 1024 }"; //Minimal properties

            var response = GetWithoutErrorFromJson<RadioObjectResponse>(json);

            Assert.AreEqual(1024ul, response.Object.Id);
        }

        [Test]
        public void TestRadioWithError()
        {
            var json = @"{ 'error' : { 'type' : 'UnknownError', 'code' : 0 } }"; // Minimal properties

            GetWithErrorFromJson<RadioObjectResponse>(json, "UnknownError", 0);
        }

        private TResponse GetWithoutErrorFromJson<TResponse>(string json) where TResponse : IDeezerObjectResponse
        {
            var subject = new DeezerObjectResponseJsonDeserializer();

            var response = JsonConvert.DeserializeObject<TResponse>(json, subject);

            Assert.NotNull(response);

            Assert.IsNull(response.Error);

            return response;
        }

        private TResponse GetWithErrorFromJson<TResponse>(string json, string expectedError, int expectedErrorCode) where TResponse : IDeezerObjectResponse
        {
            var subject = new DeezerObjectResponseJsonDeserializer();

            var response = JsonConvert.DeserializeObject<TResponse>(json, subject);

            Assert.NotNull(response);

            Assert.NotNull(response.Error);

            Assert.AreEqual(expectedError, response.Error.Type);
            Assert.AreEqual(expectedErrorCode, response.Error.Code);

            return response;
        }
    }
}
