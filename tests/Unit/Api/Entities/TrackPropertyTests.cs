using E.Deezer.Api;
using NUnit.Framework;
using System;

namespace E.Deezer.Tests.Unit.Api.Entities
{
    [TestFixture]
    public class TrackPropertyTests : ObjectWithImageTestBase
    {
        private ITrack track;

        [SetUp]
        public void SetUp()
        {
            track = new Track()
            {
                Id = 0,
                Title = "Test Track",
                Link = "www.deezer.com",

                Duration = 0,
                IsExplicit = true,
                ReleaseDate = DateTime.Now,
                Preview = "www.deezer.com",

                AlbumInternal = new Album(),
                ArtistInternal = new Artist(),

                SmallPicture = Str.SMALL_PICTURE,
                MediumPicture = Str.MEDIUM_PICTURE,
                LargePicture = Str.LARGE_PICTURE,
            };
            base.Setup();
        }

        protected override void OnSetUp() { base.objectWithImage = track; }

        [TearDown]
        public void TearDown()
        {
            track = null;
        }

        [Test]
        public void Test_BasicProperties()
        {
            Assert.NotNull(track.Title);
            Assert.NotNull(track.Link);
            Assert.NotNull(track.Preview);
            Assert.NotNull(track.ReleaseDate);
            Assert.NotNull(track.Album);
            Assert.NotNull(track.Artist);

            Assert.AreEqual(0, track.Id);
            Assert.AreEqual(0, track.Duration);

            Assert.True(track.IsExplicit);
        }
    }
}
