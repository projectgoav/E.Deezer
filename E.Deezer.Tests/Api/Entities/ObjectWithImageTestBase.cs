using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using E.Deezer.Api;

namespace E.Deezer.Tests.Api.Entities
{
    [TestFixture]
    public abstract class ObjectWithImageTestBase
    {



        protected IObjectWithImage objectWithImage;


        [SetUp]
        public void Setup() { OnSetUp(); }

        protected abstract void OnSetUp();

        [Test]
        public void Test_GetPictureSmall()
        {
            string pictureUrl = objectWithImage.GetPicture(PictureSize.Small);

            Assert.NotNull(pictureUrl);
            Assert.True(pictureUrl.Contains("small"));
            Assert.True(pictureUrl.Contains("picture"));
        }

        [Test]
        public void Test_GetPictureMedium()
        {
            string pictureUrl = objectWithImage.GetPicture(PictureSize.Medium);

            Assert.NotNull(pictureUrl);
            Assert.True(pictureUrl.Contains("medium"));
            Assert.True(pictureUrl.Contains("picture"));
        }

        [Test]
        public void Test_GetPictureLarge()
        {
            string pictureUrl = objectWithImage.GetPicture(PictureSize.Large);

            Assert.NotNull(pictureUrl);
            Assert.True(pictureUrl.Contains("large"));
            Assert.True(pictureUrl.Contains("picture"));
        }

        [Test]
        public void Test_HasPicture()
        {
            Assert.True(objectWithImage.HasPicture(PictureSize.Small));
            Assert.True(objectWithImage.HasPicture(PictureSize.Medium));
            Assert.True(objectWithImage.HasPicture(PictureSize.Large));
        }

    }
}
