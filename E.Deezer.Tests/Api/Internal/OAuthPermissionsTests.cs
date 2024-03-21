using E.Deezer.Api;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace E.Deezer.Tests.Api.Internal
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class OAuthPermissionsTests
    {

        [TestCase(true)]
        [TestCase(false)]
        public void HasPermissionReturnsTrueWhenItHasOnlyEmail(bool doesHaveEmail)
        {
            var perms =  OAuthPermissions.FromValues(hasEmail: doesHaveEmail);

            ClassicAssert.AreEqual(doesHaveEmail, perms.HasPermission(DeezerPermissions.Email));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void HasPermissionReturnsTrueWhenItHasOnlyOfflineAccess(bool doesHaveOfflineAccess)
        {
            var perms =  OAuthPermissions.FromValues(hasOfflineAccess: doesHaveOfflineAccess);

            ClassicAssert.AreEqual(doesHaveOfflineAccess, perms.HasPermission(DeezerPermissions.OfflineAccess));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void HasPermissionReturnsTrueWhenItHasOnlyManageLibrary(bool doesHaveManageLibrary)
        {
            var perms =  OAuthPermissions.FromValues(hasManageLibrary: doesHaveManageLibrary);

            ClassicAssert.AreEqual(doesHaveManageLibrary, perms.HasPermission(DeezerPermissions.ManageLibrary));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void HasPermissionReturnsTrueWhenItHasOnlyManageCommunity(bool doesHaveManageCommunity)
        {
            var perms =  OAuthPermissions.FromValues(hasManageCommunity: doesHaveManageCommunity);

            ClassicAssert.AreEqual(doesHaveManageCommunity, perms.HasPermission(DeezerPermissions.ManageCommunity));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void HasPermissionReturnsTrueWhenItHasOnlyDeleteLibrary(bool doesHaveDeleteLibrary)
        {
            var perms =  OAuthPermissions.FromValues(hasDeleteLibrary: doesHaveDeleteLibrary);

            ClassicAssert.AreEqual(doesHaveDeleteLibrary, perms.HasPermission(DeezerPermissions.DeleteLibrary));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void HasPermissionReturnsTrueWhenItHasOnlyListeningHistory(bool doesHaveListeningHistory)
        {
            var perms =  OAuthPermissions.FromValues(hasListeningHistory: doesHaveListeningHistory);

            ClassicAssert.AreEqual(doesHaveListeningHistory, perms.HasPermission(DeezerPermissions.ListeningHistory));
        }


        [TestCase(false, false)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(true, true)]
        public void HasPermissionReturnsTrueWhenMultipleAreAvailable(bool hasEmail, bool hasBasicAccess)
        {
            var perms = OAuthPermissions.FromValues(hasEmail: hasEmail,
                                                    hasBasicAccess: hasBasicAccess);

            ClassicAssert.AreEqual(hasEmail, perms.HasPermission(DeezerPermissions.Email));
            ClassicAssert.AreEqual(hasBasicAccess, perms.HasPermission(DeezerPermissions.BasicAccess));
        }
    }
}
