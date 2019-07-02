using NUnit.Framework;

namespace E.Deezer.Tests.Exception
{
    [TestFixture]
    class DeezerPermissionsExceptionTests
    {
        [TestCase(DeezerPermissions.BasicAccess, "basic_access")]
        [TestCase(DeezerPermissions.Email, "email")]
        [TestCase(DeezerPermissions.OfflineAccess, "offline_access")]
        [TestCase(DeezerPermissions.ManageLibrary, "manage_library")]
        [TestCase(DeezerPermissions.ManageCommunity, "manage_community")]
        [TestCase(DeezerPermissions.DeleteLibrary, "delete_library")]
        [TestCase(DeezerPermissions.ListeningHistory, "listening_history")]
        public void ExceptionMessageForKnownPermission(DeezerPermissions permission, string permissionAsString)
        {
            var actual = new DeezerPermissionsException(permission);

            Assert.AreEqual(
                $"The provided access token doesn't provide '{permissionAsString}' rights for this user and so this operation can't be performed. Please ensure the token hasn't expired.",
                actual.Message,
                "This permission is not yet exists in the switch-case!");
        }
    }
}
