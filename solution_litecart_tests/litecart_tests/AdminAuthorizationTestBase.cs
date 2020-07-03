using NUnit.Framework;
using OpenQA.Selenium;

namespace litecart_tests
{
    public class AdminAuthorizationTestBase : TestBase
    {
        [SetUp]
        protected void SetupAuthorizationTest()
        {
            driver.Url = applicationManager.baseURL + "/litecart/admin/";
            if (!IsLoggedInAdminPage())
            {
                driver.FindElement(By.XPath("//input[@name='username']")).SendKeys("admin");
                driver.FindElement(By.XPath("//input[@name='password']")).SendKeys("secret");
                driver.FindElement(By.XPath("//button[@name='login']")).Click();
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//a[contains(@href ,'logout.php')]")));
            }
        }
    }
}
