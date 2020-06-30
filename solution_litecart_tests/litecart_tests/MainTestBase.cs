using NUnit.Framework;
using OpenQA.Selenium;

namespace litecart_tests
{
    public class MainTestBase : TestBase
    {
        [SetUp]
        protected void SetupAuthorizationTest()
        {
            driver.Url = applicationManager.baseURL + "/litecart/";
        }
    }
}
