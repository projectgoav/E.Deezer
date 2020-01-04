using E.Deezer.Api;
using NUnit.Framework;

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

            Assert.AreEqual(doesHaveEmail, perms.HasPermission(DeezerPermissions.Email));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void HasPermissionReturnsTrueWhenItHasOnlyOfflineAccess(bool doesHaveOfflineAccess)
        {
            var perms =  OAuthPermissions.FromValues(hasOfflineAccess: doesHaveOfflineAccess);

            Assert.AreEqual(doesHaveOfflineAccess, perms.HasPermission(DeezerPermissions.OfflineAccess));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void HasPermissionReturnsTrueWhenItHasOnlyManageLibrary(bool doesHaveManageLibrary)
        {
            var perms =  OAuthPermissions.FromValues(hasManageLibrary: doesHaveManageLibrary);

            Assert.AreEqual(doesHaveManageLibrary, perms.HasPermission(DeezerPermissions.ManageLibrary));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void HasPermissionReturnsTrueWhenItHasOnlyManageCommunity(bool doesHaveManageCommunity)
        {
            var perms =  OAuthPermissions.FromValues(hasManageCommunity: doesHaveManageCommunity);

            Assert.AreEqual(doesHaveManageCommunity, perms.HasPermission(DeezerPermissions.ManageCommunity));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void HasPermissionReturnsTrueWhenItHasOnlyDeleteLibrary(bool doesHaveDeleteLibrary)
        {
            var perms =  OAuthPermissions.FromValues(hasDeleteLibrary: doesHaveDeleteLibrary);

            Assert.AreEqual(doesHaveDeleteLibrary, perms.HasPermission(DeezerPermissions.DeleteLibrary));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void HasPermissionReturnsTrueWhenItHasOnlyListeningHistory(bool doesHaveListeningHistory)
        {
            var perms =  OAuthPermissions.FromValues(hasListeningHistory: doesHaveListeningHistory);

            Assert.AreEqual(doesHaveListeningHistory, perms.HasPermission(DeezerPermissions.ListeningHistory));
        }


        [TestCase(false, false)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(true, true)]
        public void HasPermissionReturnsTrueWhenMultipleAreAvailable(bool hasEmail, bool hasBasicAccess)
        {
            var perms = OAuthPermissions.FromValues(hasEmail: hasEmail,
                                                    hasBasicAccess: hasBasicAccess);

            Assert.AreEqual(hasEmail, perms.HasPermission(DeezerPermissions.Email));
            Assert.AreEqual(hasBasicAccess, perms.HasPermission(DeezerPermissions.BasicAccess));
        }
    }
}
