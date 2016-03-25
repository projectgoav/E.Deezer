using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using E.Deezer.Api;

namespace E.Deezer.Tests.Api.Entities
{
    public abstract class ObjectWithImageTestBase
    {
        protected IObjectWithImage objectWithImage;


        [SetUp]
        public void Setup() { OnSetUp(); }

        protected abstract void OnSetUp();

        [Test]
        public void TestGetPictureSmall()
        {
            string pictureUrl = objectWithImage.GetPicture(PictureSize.SMALL);

            Assert.NotNull(pictureUrl);
            Assert.True(pictureUrl.Contains("small"));
            Assert.True(pictureUrl.Contains("picture"));
        }

        [Test]
        public void TestGetPictureMedium()
        {
            string pictureUrl = objectWithImage.GetPicture(PictureSize.MEDIUM);

            Assert.NotNull(pictureUrl);
            Assert.True(pictureUrl.Contains("medium"));
            Assert.True(pictureUrl.Contains("picture"));
        }

        [Test]
        public void TestGetPictureLarge()
        {
            string pictureUrl = objectWithImage.GetPicture(PictureSize.LARGE);

            Assert.NotNull(pictureUrl);
            Assert.True(pictureUrl.Contains("large"));
            Assert.True(pictureUrl.Contains("picture"));
        }

    }
}
