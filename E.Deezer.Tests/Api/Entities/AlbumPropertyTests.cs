using System;

using NUnit.Framework;

using E.Deezer.Api;

namespace E.Deezer.Tests.Api.Entities
{
    public class AlbumPropertyTests : ObjectWithImageTestBase
    {
        private IAlbum album;

        [SetUp]
        public void SetUp()
        {
            album = new Album()
            {
                Id = 0,
                Title = "Test Album",
                Rating = 0,
                Link = "www.deezer.com",
                ArtistInternal = new Artist(),

                SmallCover = Str.SMALL_COVER,
                MediumCover = Str.MEDIUM_COVER,
                LargeCover = Str.LARGE_COVER,
            };
            base.Setup();
        }

        protected override void OnSetUp() {  base.objectWithImage = album; }

        [TearDown]
        public void TearDown()
        {
            album = null;
        }


        [Test]
        public void TestNotNull()
        {
            Assert.NotNull(album);
        }


        [Test]
        public void Test_BasicProperties()
        {
            Assert.NotNull(album.Title);
            Assert.NotNull(album.Link);
            Assert.NotNull(album.Artist);

            Assert.AreEqual(0, album.Id);
            Assert.AreEqual(0, album.Rating);
        }
    }
}
