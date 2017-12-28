using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using NUnit.Framework;
using Newtonsoft.Json;

using E.Deezer.Api;

namespace E.Deezer.Tests.Api.Internal
{
    [TestFixture]
    public class TestObjectWithImage
    {
        private const string kDefaultJsonFilename = "ObjectWithImageDefault.json";
        private const string kCoverOnlyFilename = "ObjectWithImageOnlyCovers.json";
        private const string kMissingPictureFilename = "ObjectWithImageMissingPicture.json";
        
        private string defaultJson;
        private string missingPictureJson;
        private string coverOnlyJson;


        [OneTimeSetUp]
        public void Init()
        {
            string baseDir = TestContext.CurrentContext.WorkDirectory;
            string fullDir = Path.Combine(baseDir, "Resources", "Api", "Internal");


            var defaultPath = Path.Combine(fullDir, kDefaultJsonFilename);
            defaultJson = string.Join("\n", File.ReadAllLines(defaultPath));

            var coverOnlyPath = Path.Combine(fullDir, kCoverOnlyFilename);
            coverOnlyJson = string.Join("\n", File.ReadAllLines(coverOnlyPath));

            var missingPicturePath = Path.Combine(fullDir, kMissingPictureFilename);
            missingPictureJson = string.Join("\n", File.ReadAllLines(missingPicturePath));

            Assert.That(string.IsNullOrEmpty(defaultJson) == false);
            Assert.That(string.IsNullOrEmpty(coverOnlyJson) == false);
            Assert.That(string.IsNullOrEmpty(missingPictureJson) == false);
        }


        [Test]
        public void TestHasImageAll()
        {
            IObjectWithImage imgObj = JsonConvert.DeserializeObject<ObjectWithImage>(defaultJson);

            Assert.That(imgObj != null);

            Assert.That(imgObj.HasPicture(PictureSize.Small));
            Assert.That(imgObj.HasPicture(PictureSize.Medium));
            Assert.That(imgObj.HasPicture(PictureSize.Large));
            Assert.That(imgObj.HasPicture(PictureSize.ExtraLarge));
        }

        [Test]
        public void TestHasImageCoverOnly()
        {
            IObjectWithImage imgObj = JsonConvert.DeserializeObject<ObjectWithImage>(coverOnlyJson);

            Assert.That(imgObj != null);

            Assert.That(imgObj.HasPicture(PictureSize.Small));
            Assert.That(imgObj.HasPicture(PictureSize.Medium));
            Assert.That(imgObj.HasPicture(PictureSize.Large));
            Assert.That(imgObj.HasPicture(PictureSize.ExtraLarge));
        }

        [Test]
        public void TestHasImageWithMissingValues()
        {
            IObjectWithImage imgObj = JsonConvert.DeserializeObject<ObjectWithImage>(missingPictureJson);

            Assert.That(imgObj != null);

            Assert.That(imgObj.HasPicture(PictureSize.Small));
            Assert.That(imgObj.HasPicture(PictureSize.ExtraLarge));

            Assert.That(imgObj.HasPicture(PictureSize.Medium) == false );
            Assert.That(imgObj.HasPicture(PictureSize.Large) == false);
        }

        [Test]
        public void TestGetImageNotNull()
        {
            IObjectWithImage imgObj = JsonConvert.DeserializeObject<ObjectWithImage>(defaultJson);

            Assert.That(imgObj != null);

            Assert.That(string.IsNullOrEmpty(imgObj.GetPicture(PictureSize.Small)) == false);
            Assert.That(string.IsNullOrEmpty(imgObj.GetPicture(PictureSize.Medium)) == false);
            Assert.That(string.IsNullOrEmpty(imgObj.GetPicture(PictureSize.Large)) == false);
            Assert.That(string.IsNullOrEmpty(imgObj.GetPicture(PictureSize.ExtraLarge)) == false);
        }
        
    }
}
