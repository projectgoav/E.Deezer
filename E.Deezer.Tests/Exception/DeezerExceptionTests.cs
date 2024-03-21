using E.Deezer.Api;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace E.Deezer.Tests.Exception
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class DeezerExceptionTests
    {
        [TestCase("QuotaException",              4u,   DeezerException.QUOTA_MESSAGE)]
        [TestCase("ItemsLimitExceededException", 100u, DeezerException.ITEM_LIMIT_MESSAGE)]
        [TestCase("PermissionException",         200u, DeezerException.INVALID_PERMISSION_MESSAGE)]
        [TestCase("InvalidTokenException",       300u, DeezerException.INVALID_TOKEN_MESSAGE)]
        [TestCase("ParameterException",          500u, DeezerException.INVALID_PARAMETER_MESSAGE)]
        [TestCase("MissingParameterException",   501u, DeezerException.MISSING_PARAMETER_MESSAGE)]
        [TestCase("InvalidQueryException",       600u, DeezerException.INVALID_QUERY_MESSAGE)]
        [TestCase("ServiceBusyException",        700u, DeezerException.SERVICE_BUSY_MESSAGE)]
        [TestCase("DataNotFoundException",       800u, DeezerException.NOT_FOUND_MESSAGE)]
        public void TestExceptionMessageFor(string _, uint errorCode, string expectedMessage)
        {
            var error = Error.FromValues(errorCode, string.Empty, string.Empty);

            var ex = new DeezerException(error);

            ClassicAssert.AreEqual(expectedMessage, ex.Message);
        }
    }
}
