using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using E.Deezer;

namespace E.Deezer.Portable.Tests
{
    [TestFixture]
    public class ExecutorServiceTests
    {
        private ExecutorService executor;

        [SetUp]
        public void SetUp()
        {
            executor = new ExecutorService();
            Assert.NotNull(executor);
        }

        [TearDown]
        public void TearDown()
        {
            executor.Dispose();
            executor = null;
        }


        [Test]
        public void TestParamsAddJsonOutput()
        {
            string url = "test.com";
            string trueurl = executor.BuildUrl(url, RequestParameter.EmptyList);

            Assert.NotNull(trueurl);
            Assert.AreEqual("test.com?output=json", trueurl);
        }

        [Test]
        public void TestParamsFilledUrl()
        {
            string url = "test.com/charts/{id}";
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewUrlSegmentParamter("id", "something"),
            };

            string trueurl = executor.BuildUrl(url, parms);

            Assert.NotNull(trueurl);
            Assert.AreEqual("test.com/charts/something?output=json", trueurl);
        }

        [Test]
        public void TestParamsFilledQueryString()
        {
            string url = "test.com/charts";
            List<IRequestParameter> parms = new List<IRequestParameter>()
            {
                RequestParameter.GetNewQueryStringParameter("id", "something"),
            };

            string trueurl = executor.BuildUrl(url, parms);

            Assert.NotNull(trueurl);
            Assert.AreEqual("test.com/charts?id=something&output=json", trueurl);
        }


    }
}
