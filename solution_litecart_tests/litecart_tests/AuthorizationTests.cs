using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace litecart_tests
{
    class AuthorizationTests : TestBase
    {
        [Test]
        public void AuthWithValidCredentials()
        {
            driver.Url = applicationManager.baseURL + "/litecart/admin/";

            if (IsLoggedInAdminPage())
            {
                driver.FindElement(By.XPath("//a[contains(@href ,'logout.php')]")).Click();
            }

            driver.FindElement(By.XPath("//input[@name='username']")).SendKeys("admin");
            driver.FindElement(By.XPath("//input[@name='password']")).SendKeys("secret");
            driver.FindElement(By.XPath("//button[@name='login']")).Click();
        }   
    }
}
