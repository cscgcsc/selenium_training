using NUnit.Framework;

namespace litecart_tests
{
    class AdminAuthorizationTests : AdminTestBase
    {
        [Test, TestCaseSource(typeof(DataProviders), "ValidUser")]
        public void AuthWithValidCredentials(User user)
        {
            if (app.LoginHelper.IsLoggedInAdminPage())
                app.LoginHelper.LogoutInAdminPage();

            app.LoginHelper.LoginInAdminPage(user);
        }   
    }
}
