using System;

using NUnit.Framework;

using E.Deezer.Api;

namespace E.Deezer.Tests.Api.Entities
{
    [TestFixture]
    public class GenrePropertyTests : ObjectWithImageTestBase
    {
        private IGenre genre;

        [SetUp]
        public void SetUp()
        {
            genre = new Genre()
            {
                Id = 0,   
                Name = "Test Genre",

                SmallPicture = Str.SMALL_PICTURE,
                MediumPicture = Str.MEDIUM_PICTURE,
                LargePicture = Str.LARGE_PICTURE,
            };
            base.Setup();
        }

        protected override void OnSetUp() { base.objectWithImage = genre; }

        [TearDown]
        public void TearDown()
        {
            genre = null;
        }

        [Test]
        public void Test_BasicProperties()
        {
            Assert.NotNull(genre.Name);

            Assert.AreEqual(0, genre.Id);
        }
    }
}
