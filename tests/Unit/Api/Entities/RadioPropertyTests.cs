using E.Deezer.Api;
using NUnit.Framework;

namespace E.Deezer.Tests.Unit.Api.Entities
{
    [TestFixture]
    public class RadioPropertyTests : ObjectWithImageTestBase
    {
        private IRadio radio;

        [SetUp]
        public void SetUp()
        {
            radio = new Radio()
            {
                Id = 0,
                Title = "Test Radio",
                Description = "Description",
                ShareLink = "www.deezer.com",

                SmallPicture = Str.SMALL_PICTURE,
                MediumPicture = Str.MEDIUM_PICTURE,
                LargePicture = Str.LARGE_PICTURE,
            };
            base.Setup();
        }

        protected override void OnSetUp() { base.objectWithImage = radio; }

        [TearDown]
        public void TearDown()
        {
            radio = null;
        }

        [Test]
        public void Test_BasicProperties()
        {
            Assert.NotNull(radio.Title);
            Assert.NotNull(radio.ShareLink);
            Assert.NotNull(radio.Description);

            Assert.AreEqual(0, radio.Id);
        }
    }
}
