using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace litecart_tests
{
    [TestFixture]
    public class FirefoxBrowser
    {
        private IWebDriver driver;

        [SetUp]
        public void Start()
        {
            FirefoxOptions options = new FirefoxOptions();         
            options.BrowserExecutableLocation = @"C:\Program Files\Mozilla Firefox\firefox.exe";
            //options.LogLevel = FirefoxDriverLogLevel.Trace;
            //options.BrowserExecutableLocation = @"C:\Program Files\Firefox Nightly\firefox.exe";       
            //options.BrowserExecutableLocation = @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe";
            //options.UseLegacyImplementation = true;

            driver = new FirefoxDriver(options);
        }

        [Test]
        public void FirstTest()
        {
            driver.Url = "http://yandex.ru/";
            driver.FindElement(By.XPath("//input[@id='text']")).SendKeys("test");
            driver.FindElement(By.XPath("//button[@type='submit']")).Click();
        }

        [TearDown]
        public void Stop()
        {
            driver.Quit();
            driver = null;
        }
    }
}
