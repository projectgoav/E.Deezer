using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace E.Deezer.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class DeezerPermissionsTests
    {
        private const string JOINT_A = Permissions.BASIC_ACCESS + ", " + Permissions.EMAIL;
        private const string JOINT_B = Permissions.BASIC_ACCESS + ", " + Permissions.MANAGE_COMMUNITY + ", " + Permissions.MANAGE_LIBRARY;


        [TestCase(DeezerPermissions.BasicAccess,        Permissions.BASIC_ACCESS)]
        [TestCase(DeezerPermissions.Email,              Permissions.EMAIL)]
        [TestCase(DeezerPermissions.OfflineAccess,      Permissions.OFFLINE_ACCESS)]
        [TestCase(DeezerPermissions.ManageLibrary,      Permissions.MANAGE_LIBRARY)]
        [TestCase(DeezerPermissions.ManageCommunity,    Permissions.MANAGE_COMMUNITY)]
        [TestCase(DeezerPermissions.DeleteLibrary,      Permissions.DELETE_LIBRARY)]
        [TestCase(DeezerPermissions.ListeningHistory,   Permissions.LISTENING_HISTORY)]
        [TestCase(DeezerPermissions.Email | DeezerPermissions.BasicAccess,                                              JOINT_A)]
        [TestCase(DeezerPermissions.ManageCommunity | DeezerPermissions.ManageLibrary | DeezerPermissions.BasicAccess,  JOINT_B)]
        public void PermissionToString(DeezerPermissions input, string expected)
        {
            ClassicAssert.AreEqual(expected, input.PermissionToString());
        }



        [TestCase(DeezerPermissions.BasicAccess, DeezerPermissions.Email, false)]
        [TestCase(DeezerPermissions.BasicAccess, DeezerPermissions.BasicAccess, true)]
        [TestCase(DeezerPermissions.Email | DeezerPermissions.OfflineAccess, DeezerPermissions.ManageCommunity, false)]
        [TestCase(DeezerPermissions.Email | DeezerPermissions.OfflineAccess, DeezerPermissions.OfflineAccess, true)]
        public void TestHasPermissions(DeezerPermissions permissions, DeezerPermissions requestedPermission, bool shouldBeAvailable)
        {
            ClassicAssert.AreEqual(shouldBeAvailable, permissions.HasPermission(requestedPermission));
        }
    }
}
