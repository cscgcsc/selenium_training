using NUnit.Framework;

namespace litecart_tests
{
    public class MainAuthorizationTestBase : TestBase
    {
        [SetUp]
        protected void SetupAuthorizationTest()
        {
            app.LoginHelper.LoginInMainPage();
        }
    }
}
