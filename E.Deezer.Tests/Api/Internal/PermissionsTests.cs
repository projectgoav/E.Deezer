using E.Deezer.Api;
using NUnit.Framework;

namespace E.Deezer.Tests.Api.Internal
{
    [TestFixture]
    class PermissionsTests
    {
        private OAuthPermissions _oAuth;

        [SetUp]
        public void SetUp()
        {
            _oAuth = new OAuthPermissions();
        }

        [Test]
        public void HasPermissionReturnsTrueWhenItHasOnlyEmail()
        {
            _oAuth.HasEmail = true;

            Assert.IsTrue(_oAuth.HasPermission(DeezerPermissions.Email));
        }

        [Test]
        public void HasPermissionReturnsTrueWhenItHasOnlyOfflineAccess()
        {
            _oAuth.HasOfflineAccess = true;

            Assert.IsTrue(_oAuth.HasPermission(DeezerPermissions.OfflineAccess));
        }

        [Test]
        public void HasPermissionReturnsTrueWhenItHasOnlyManageLibrary()
        {
            _oAuth.HasManageLibrary = true;

            Assert.IsTrue(_oAuth.HasPermission(DeezerPermissions.ManageLibrary));
        }

        [Test]
        public void HasPermissionReturnsTrueWhenItHasOnlyManageCommunity()
        {
            _oAuth.HasManageCommunity = true;

            Assert.IsTrue(_oAuth.HasPermission(DeezerPermissions.ManageCommunity));
        }

        [Test]
        public void HasPermissionReturnsTrueWhenItHasOnlyDeleteLibrary()
        {
            _oAuth.HasDeleteLibrary = true;

            Assert.IsTrue(_oAuth.HasPermission(DeezerPermissions.DeleteLibrary));
        }

        [Test]
        public void HasPermissionReturnsTrueWhenItHasOnlyListeningHistory()
        {
            _oAuth.HasListeningHistory = true;

            Assert.IsTrue(_oAuth.HasPermission(DeezerPermissions.ListeningHistory));
        }

        [Test]
        public void HasPermissionsReturnsFalseWhenHasMultiplePermissionsButNotBasicAccess()
        {
            GrantAllPermissionFor(_oAuth);
            _oAuth.HasBasicAccess = false;

            Assert.IsFalse(_oAuth.HasPermission(DeezerPermissions.BasicAccess));
        }

        [Test]
        public void HasPermissionsReturnsFalseWhenHasMultiplePermissionsButNotEmail()
        {
            GrantAllPermissionFor(_oAuth);
            _oAuth.HasEmail = false;

            Assert.IsFalse(_oAuth.HasPermission(DeezerPermissions.Email));
        }

        [Test]
        public void HasPermissionsReturnsFalseWhenHasMultiplePermissionsButNotOfflineAccess()
        {
            GrantAllPermissionFor(_oAuth);
            _oAuth.HasOfflineAccess = false;

            Assert.IsFalse(_oAuth.HasPermission(DeezerPermissions.OfflineAccess));
        }

        [Test]
        public void HasPermissionsReturnsFalseWhenHasMultiplePermissionsButNotManageLibrary()
        {
            GrantAllPermissionFor(_oAuth);
            _oAuth.HasManageLibrary = false;

            Assert.IsFalse(_oAuth.HasPermission(DeezerPermissions.ManageLibrary));
        }

        [Test]
        public void HasPermissionsReturnsFalseWhenHasMultiplePermissionsButNotManageCommunity()
        {
            GrantAllPermissionFor(_oAuth);
            _oAuth.HasManageCommunity = false;

            Assert.IsFalse(_oAuth.HasPermission(DeezerPermissions.ManageCommunity));
        }

        [Test]
        public void HasPermissionsReturnsFalseWhenHasMultiplePermissionsButNotDeleteLibrary()
        {
            GrantAllPermissionFor(_oAuth);
            _oAuth.HasDeleteLibrary = false;

            Assert.IsFalse(_oAuth.HasPermission(DeezerPermissions.DeleteLibrary));
        }

        [Test]
        public void HasPermissionsReturnsFalseWhenHasMultiplePermissionsButNotListeningHistory()
        {
            GrantAllPermissionFor(_oAuth);
            _oAuth.HasListeningHistory = false;

            Assert.IsFalse(_oAuth.HasPermission(DeezerPermissions.ListeningHistory));
        }

        [Test]
        public void HasPermissionsReturnsFalseWhenMultipleRequiredPermissionsMissing()
        {
            GrantAllPermissionFor(_oAuth);
            _oAuth.HasBasicAccess = false;
            _oAuth.HasEmail = false;

            Assert.IsFalse(_oAuth.HasPermission(DeezerPermissions.BasicAccess), "BasicAccess");
            Assert.IsFalse(_oAuth.HasPermission(DeezerPermissions.Email), "Email");
        }

        [Test]
        public void HasPermissionsReturnsTrueWhenMultipleNonRequiredPermissionsMissing()
        {
            GrantAllPermissionFor(_oAuth);
            _oAuth.HasBasicAccess = false;
            _oAuth.HasEmail = false;

            Assert.IsTrue(_oAuth.HasPermission(DeezerPermissions.DeleteLibrary), "DeleteLibrary");
            Assert.IsTrue(_oAuth.HasPermission(DeezerPermissions.ManageCommunity), "ManageCommunity");
        }

        private void GrantAllPermissionFor(OAuthPermissions oauth)
        {
            oauth.HasBasicAccess = true;
            oauth.HasDeleteLibrary = true;
            oauth.HasEmail = true;
            oauth.HasListeningHistory = true;
            oauth.HasManageCommunity = true;
            oauth.HasManageLibrary = true;
            oauth.HasOfflineAccess = true;
        }
    }
}
