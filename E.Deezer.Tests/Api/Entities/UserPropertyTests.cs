using System;

using NUnit.Framework;

using E.Deezer.Api;

namespace E.Deezer.Tests.Api.Entities
{
    [TestFixture]
    public class UserPropertyTests : ObjectWithImageTestBase
    {
        private IUser user;

        [SetUp]
        public void SetUp()
        {
            user = new User()
            {
                Id = 0,
                Name = "Test User",
                Country = "Test",
                Link = "www.deezer.com",

                SmallPicture = Str.SMALL_PICTURE,
                MediumPicture = Str.MEDIUM_PICTURE,
                LargePicture = Str.LARGE_PICTURE,
            };
            base.Setup();
        }

        protected override void OnSetUp() { base.objectWithImage = user; }

        [TearDown]
        public void TearDown()
        {
            user = null;
        }

        [Test]
        public void Test_BasicProperties()
        {
            Assert.NotNull(user.Name);
            Assert.NotNull(user.Link);
            Assert.NotNull(user.Country);

            Assert.AreEqual(0, user.Id);
        }
    }
}
