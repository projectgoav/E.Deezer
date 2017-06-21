using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using E.Deezer.Api;

namespace E.Deezer.Tests.Api.Entities
{
    public class AlbumTests : ObjectWithImageTestBase
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
                ArtistInternal = new Artist()
                {
                    Id = 1,
                    Link = "www.deezer.com",
                    Name = "Test Artist",

                    SmallPicture = "www.deezer.com/small/picture.png",
                    MediumPicture = "www.deezer.com/medium/picture.png",
                    LargePicture = "www.deezer.com/large/picture.png",
                },

                SmallCover = "www.deezer.com/small/cover/picture.png",
                MediumCover = "www.deezer.com/medium/cover/picture.png",
                LargeCover = "www.deezer.com/large/cover/picture.png",
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


        //[Test]
        //public void TestBasicProperties()
        //{
        //    Assert.NotNull(album.Title);
        //    Assert.NotNull(album.Link);
        //    Assert.NotNull(album.Artist);

        //    Assert.AreEqual(0, album.Id);
        //    Assert.AreEqual(0, album.Rating);
        //}

        //[Test]
        //public void TestGetPictureSmall()
        //{
        //    string pictureUrl = album.GetPicture(PictureSize.SMALL);

        //    Assert.NotNull(pictureUrl);
        //    Assert.True(pictureUrl.Contains("small"));
        //    Assert.True(pictureUrl.Contains("picture"));
        //}

        //[Test]
        //public void TestGetPictureMedium()
        //{
        //    string pictureUrl = album.GetPicture(PictureSize.MEDIUM);

        //    Assert.NotNull(pictureUrl);
        //    Assert.True(pictureUrl.Contains("medium"));
        //    Assert.True(pictureUrl.Contains("picture"));
        //}

        //[Test]
        //public void TestGetPictureLarge()
        //{
        //    string pictureUrl = album.GetPicture(PictureSize.LARGE);

        //    Assert.NotNull(pictureUrl);
        //    Assert.True(pictureUrl.Contains("large"));
        //    Assert.True(pictureUrl.Contains("picture"));
        //}


    }
}
