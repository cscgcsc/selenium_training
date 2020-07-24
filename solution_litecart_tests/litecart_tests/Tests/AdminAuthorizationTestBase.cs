using NUnit.Framework;

namespace litecart_tests
{
    public class AdminAuthorizationTestBase : TestBase
    {
        [SetUp]
        protected void SetupAuthorizationTest()
        {
            app.LoginHelper.LoginInAdminPage();
        }
    }
}
