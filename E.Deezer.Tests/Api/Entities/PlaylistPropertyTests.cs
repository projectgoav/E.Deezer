using E.Deezer.Api;
using NUnit.Framework;

namespace E.Deezer.Tests.Api.Entities
{
    [TestFixture]
    public class PlaylisyPropertyTests : ObjectWithImageTestBase
    {
        private IPlaylist playlist;

        [SetUp]
        public void SetUp()
        {
            playlist = new Playlist()
            {
                Id = 0,
                Title = "Test Playlist",
                Link = "www.deezer.com",
                Rating = 0,
                TrackCount = 5,
                //Public = true,
                IsLovedTrack = true,

                CreatorInternal = new UserProfile() { Username = "Test User" },
                //UserInternal = new User() { Name = "Test User" },

                SmallPicture = Str.SMALL_PICTURE,
                MediumPicture = Str.MEDIUM_PICTURE,
                LargePicture = Str.LARGE_PICTURE,
            };
            base.Setup();
        }

        protected override void OnSetUp() { base.objectWithImage = playlist; }

        [TearDown]
        public void TearDown()
        {
            playlist = null;
        }

        [Test]
        public void Test_BasicProperties()
        {
            Assert.NotNull(playlist.Title);
            Assert.NotNull(playlist.Link);
            Assert.NotNull(playlist.Link);

            Assert.NotNull(playlist.CreatorName);

            Assert.AreEqual(0, playlist.Id);
            Assert.AreEqual(0, playlist.Rating);
            Assert.AreEqual(5, playlist.TrackCount);

            Assert.True(playlist.IsLovedTrack);
            //Assert.True(playlist.Public);
        }
    }
}
