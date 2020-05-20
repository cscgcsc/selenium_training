using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace litecart_tests
{
    [TestFixture]
    public class AdminPageTests
    {
        private IWebDriver driver;

        [SetUp]
        public void Start()
        {
            ChromeOptions options = new ChromeOptions();         
            options.AddArgument("start-maximized");

            driver = new ChromeDriver(options);
        }

        [Test]
        public void LeftMenuVerificationTest()
        {
            driver.Url = "http://localhost/litecart/admin/";

            if (!IsLoggedIn())
            {
                driver.FindElement(By.XPath("//input[@name='username']")).SendKeys("admin");
                driver.FindElement(By.XPath("//input[@name='password']")).SendKeys("secret");
                driver.FindElement(By.XPath("//button[@name='login']")).Click();
                WaitForElementPresent(By.XPath("//a[contains(@href ,'logout.php')]"));
            }          

            ClickAllLeftMenuItems("//ul[@id='box-apps-menu']/li");
        }

        private void ClickAllLeftMenuItems(string xPathToFind)
        {
            WaitLeftMenuSubitems(By.XPath(xPathToFind));

            for (int i = 0; i < driver.FindElements(By.XPath(xPathToFind)).Count(); i++)
            {
                List<IWebElement> menuItems = driver.FindElements(By.XPath(xPathToFind)).ToList();
                object[] args = { menuItems[i].Text };
                menuItems[i].Click();
                Assert.IsTrue(IsElementPresent(By.XPath("//td[@id='content']/h1")), "Page '{0}' doesn't contain h1 header", args);
                Assert.AreEqual("selected", driver.FindElements(By.XPath(xPathToFind)).ToList()[i].GetAttribute("class"), "Page '{0}' is not selected", args);
                ClickAllLeftMenuItems(xPathToFind + "[" + (i + 1) + "]/ul/li");
            }
        }

        private void WaitLeftMenuSubitems(By element)
        {
            for (int attempt = 0; ; attempt++)
            {
                if (attempt >= 3) break;
                try
                {
                    if (IsElementPresent(element)) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(100);
            }
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

        private void WaitForElementPresent(By element)
        {
            for (int second = 0; ; second++)
            {
                if (second >= 10) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(element)) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
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
