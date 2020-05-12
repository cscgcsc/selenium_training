using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace litecart_tests
{
    class AuthorizationTests
    {
        private IWebDriver driver;

        [SetUp]
        public void Start()
        {
            driver = new ChromeDriver();
        }

        [Test]
        public void AuthWithValidCredentials()
        {
            driver.Url = "http://localhost/litecart/admin/";
            
            if(IsLoggedIn())
            {
                driver.FindElement(By.XPath("//a[contains(@href ,'logout.php')]")).Click();
            }

            driver.FindElement(By.XPath("//input[@name='username']")).SendKeys("admin");
            driver.FindElement(By.XPath("//input[@name='password']")).SendKeys("secret");
            driver.FindElement(By.XPath("//button[@name='login']")).Click();
        }

        private bool IsLoggedIn()
        {
            return IsElementPresent(By.XPath("//a[contains(@href ,'logout.php')]"));
        }

        private bool IsElementPresent(By element)
        {
            try
            {
                driver.FindElement(element);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        [TearDown]
        public void Stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
