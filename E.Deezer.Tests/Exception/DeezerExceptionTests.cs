using E.Deezer.Api;
using NUnit.Framework;

namespace E.Deezer.Tests.Exception
{
    [TestFixture]
    class DeezerExceptionTests
    {
        [TestCase("QuotaException", 4u)]
        [TestCase("ItemsLimitExceededException", 100u)]
        [TestCase("PermissionException", 200u)]
        [TestCase("InvalidTokenException", 300u)]
        [TestCase("ParameterException", 500u)]
        [TestCase("MissingParameterException", 501u)]
        [TestCase("InvalidQueryException", 600u)]
        [TestCase("ServiceBusyException", 700u)]
        [TestCase("DataNotFoundException", 800u)]
        public void TestExceptionMessageFor(string justToMakeItMoreReadable, uint errorCode)
        {
            var error = new Error() { Code = errorCode };

            var ex = new DeezerException(error);

            Assert.AreNotEqual("An unknown exception has occured...", ex.Message, "This exception is not yet exists in the switch-case!");
        }
    }
}
