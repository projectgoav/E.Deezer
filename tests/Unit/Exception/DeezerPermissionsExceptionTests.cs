using NUnit.Framework;

namespace E.Deezer.Tests.Unit.Exception
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class DeezerPermissionsExceptionTests
    {
        [TestCase(DeezerPermissions.BasicAccess,        Permissions.BASIC_ACCESS)]
        [TestCase(DeezerPermissions.Email,              Permissions.EMAIL)]
        [TestCase(DeezerPermissions.OfflineAccess,      Permissions.OFFLINE_ACCESS)]
        [TestCase(DeezerPermissions.ManageLibrary,      Permissions.MANAGE_LIBRARY)]
        [TestCase(DeezerPermissions.ManageCommunity,    Permissions.MANAGE_COMMUNITY)]
        [TestCase(DeezerPermissions.DeleteLibrary,      Permissions.DELETE_LIBRARY)]
        [TestCase(DeezerPermissions.ListeningHistory,   Permissions.LISTENING_HISTORY)]
        public void ExceptionMessageForKnownPermission(DeezerPermissions permission, string permissionAsString)
        {
            var expectedMessage = string.Format(DeezerPermissionsException.EXCEPTION_MESSAGE_FORMAT, permissionAsString);

            var actual = new DeezerPermissionsException(permission);

            Assert.AreEqual(expectedMessage, actual.Message);
        }
    }
}
