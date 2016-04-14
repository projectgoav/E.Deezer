using System;

using NUnit.Framework;

using E.Deezer.Api;

namespace E.Deezer.Tests.Api.Entities
{
    [TestFixture]
    public class ArtistPropertyTests : ObjectWithImageTestBase
    {
        private IArtist artist;

        [SetUp]
        public void SetUp()
        {
            artist = new Artist()
            {
                Id = 0,
                Name = "Test Artist",
                Link = "www.deezer.com",

                SmallPicture = Str.SMALL_PICTURE,
                MediumPicture = Str.MEDIUM_PICTURE,
                LargePicture = Str.LARGE_PICTURE,
            };
            base.Setup();
        }

        protected override void OnSetUp() { base.objectWithImage = artist; }

        [TearDown]
        public void TearDown()
        {
            artist = null;
        }

        [Test]
        public void Test_BasicProperties()
        {
            Assert.NotNull(artist.Name);
            Assert.NotNull(artist.Link);

            Assert.AreEqual(0, artist.Id);
        }
    }
}
