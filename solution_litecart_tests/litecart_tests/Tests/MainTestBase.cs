using NUnit.Framework;

namespace litecart_tests
{
    public class MainTestBase : TestBase
    {
        [SetUp]
        protected void SetupAuthorizationTest()
        {
            app.NavigationHelper.GoToMainPage();
        }
    }
}
