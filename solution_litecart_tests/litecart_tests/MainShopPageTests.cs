using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Threading;

namespace litecart_tests
{
    class MainShopPageTests
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
        public void ProductsStickerTest()
        {
            driver.Url = "http://localhost/litecart/";

            ICollection<IWebElement> productsList = driver.FindElements(By.XPath("//ul[contains(@class, 'products')]/li"));

            foreach(IWebElement product in productsList)
            {
                object[] args = { product.FindElement(By.XPath(".//div[contains(@class, 'name')]")).Text };
                Assert.AreEqual(1, product.FindElements(By.XPath(".//div[contains(@class, 'sticker')]")).Count, "The number of stickers for '{0}' is not equal 1.", args);
            }
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
