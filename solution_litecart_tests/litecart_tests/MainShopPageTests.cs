using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
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

            //FirefoxOptions options = new FirefoxOptions();
            //options.BrowserExecutableLocation = @"C:\Program Files\Mozilla Firefox\firefox.exe";
            //driver = new FirefoxDriver(options);

            //driver = new InternetExplorerDriver();
        }

        [Test]
        public void ProductsStickerTest()
        {
            driver.Url = "http://localhost/litecart/";

            ICollection<IWebElement> productsList = driver.FindElements(By.XPath("//ul[contains(@class, 'products')]/li"));

            foreach(IWebElement product in productsList)
            {
                object[] args = {product.FindElement(By.XPath(".//div[contains(@class, 'name')]")).Text};
                Assert.AreEqual(1, product.FindElements(By.XPath(".//div[contains(@class, 'sticker')]")).Count, "The number of stickers for '{0}' is not equal 1.", args);
            }
        }

        [Test]
        public void ComparisonProductsInformationTest()
        {
            driver.Url = "http://localhost/litecart/";

            //Список товаров
            List<IWebElement> productsList = driver.FindElements(By.XPath("//div[@id='box-campaigns']//li")).ToList();

            string[] oldArgs = {productsList[0].FindElement(By.XPath(".//div[contains(@class, 'name')]")).Text};
            Assert.IsTrue(IsElementPresent(By.XPath(".//s[contains(@class, 'regular-price')]"), productsList[0]), "Regular price for '{0}' is not strikethrough.", oldArgs);
            Assert.IsTrue(IsElementPresent(By.XPath(".//strong[contains(@class, 'campaign-price')]"), productsList[0]), "Sale price for '{0}' is not bold.", oldArgs);
            
            IWebElement oldRegularPrice = productsList[0].FindElement(By.XPath(".//s[contains(@class, 'regular-price')]"));
            IWebElement oldSalePrice = productsList[0].FindElement(By.XPath(".//strong[contains(@class, 'campaign-price')]"));
            string oldRegularPriceText = oldRegularPrice.Text;
            string oldSalePriceText = oldSalePrice.Text;
            Assert.IsTrue(IsGreyColor(oldRegularPrice.GetCssValue("color")), "Regular price for '{0}' is not grey color.", oldArgs);
            Assert.IsTrue(IsRedColor(oldSalePrice.GetCssValue("color")), "Sale price for '{0}' is not red color.", oldArgs);
            Assert.Greater(GetNumber(oldSalePrice.GetCssValue("font-size")), GetNumber(oldRegularPrice.GetCssValue("font-size")), "Regular price for '{0}' is equal or greater than sale price.", oldArgs);       

            //Страница товара
            productsList[0].FindElement(By.XPath("./a")).Click();            
            IWebElement productInfo = driver.FindElement(By.XPath("//div[@id='box-product']"));

            Assert.IsTrue(IsElementPresent(By.XPath(".//s[contains(@class, 'regular-price')]"), productInfo), "Regular price for '{0}' is not strikethrough.", oldArgs);
            Assert.IsTrue(IsElementPresent(By.XPath(".//strong[contains(@class, 'campaign-price')]"), productInfo), "Sale price for '{0}' is not bold.", oldArgs);

            IWebElement newRegularPrice = productInfo.FindElement(By.XPath(".//s[contains(@class, 'regular-price')]"));
            IWebElement newSalePrice = productInfo.FindElement(By.XPath(".//strong[contains(@class, 'campaign-price')]"));
            Assert.IsTrue(IsGreyColor(newRegularPrice.GetCssValue("color")), "Regular price for '{0}' is not grey color.", oldArgs);
            Assert.IsTrue(IsRedColor(newSalePrice.GetCssValue("color")), "Sale price for '{0}' is not red color.", oldArgs);
            Assert.Greater(GetNumber(newSalePrice.GetCssValue("font-size")), GetNumber(newRegularPrice.GetCssValue("font-size")), "Regular price for '{0}' is equal or greater than sale price.", oldArgs);

            //Общие проверки
            Assert.AreEqual(newRegularPrice.Text, oldRegularPriceText, "Regular price for '{0}' on main page and product's page do not coincide.", oldArgs);
            Assert.AreEqual(newSalePrice.Text, oldSalePriceText, "Sale price for '{0}' on main page and product's page do not coincide.", oldArgs);
            Assert.AreEqual(productInfo.FindElement(By.XPath(".//h1")).Text, oldArgs[0], "Product name for '{0}' on main page and product's page do not coincide.", oldArgs);
        }

        private double GetNumber(string cssSize)
        {
            cssSize = Regex.Replace(cssSize, @"[^\d\.,]", "").Replace(".", ",");           
            return double.Parse(cssSize);
        }

        private string[] GetColor(string cssColor)
        {       
            return Regex.Replace(cssColor, @"[^\d,]", "").Split(new char[] {','});
        }

        private bool IsGreyColor(string cssColor)
        {
            string[] rgb = GetColor(cssColor);

            return rgb[0] == rgb[1] 
                && rgb[0] == rgb[2];
        }

        private bool IsRedColor(string cssColor)
        {
            string[] rgb = GetColor(cssColor);

            return rgb[1] == "0" 
                && rgb[2] == "0";
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

        private bool IsElementPresent(By element, IWebElement webElement)
        {
            try
            {
                webElement.FindElement(element);
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
