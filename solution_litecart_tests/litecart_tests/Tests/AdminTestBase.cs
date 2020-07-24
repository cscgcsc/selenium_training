using NUnit.Framework;

namespace litecart_tests
{
    public class AdminTestBase : TestBase
    {
        [SetUp]
        protected void SetupAuthorizationTest()
        {
            app.NavigationHelper.GoToAdminPage();
        }
    }
}
