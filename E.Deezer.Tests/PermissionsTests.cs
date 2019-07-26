using NUnit.Framework;

namespace E.Deezer.Tests
{
    [TestFixture]
    class PermissionsTests
    {
        [TestCase(DeezerPermissions.BasicAccess, "basic_access")]
        [TestCase(DeezerPermissions.Email, "email")]
        [TestCase(DeezerPermissions.OfflineAccess, "offline_access")]
        [TestCase(DeezerPermissions.ManageLibrary, "manage_library")]
        [TestCase(DeezerPermissions.ManageCommunity, "manage_community")]
        [TestCase(DeezerPermissions.DeleteLibrary, "delete_library")]
        [TestCase(DeezerPermissions.ListeningHistory, "listening_history")]
        [TestCase(DeezerPermissions.Email | DeezerPermissions.BasicAccess, "basic_access, email")]
        public void PermissionToString(DeezerPermissions input, string expected)
        {
            Assert.AreEqual(expected, input.PermissionToString());
        }
    }
}
