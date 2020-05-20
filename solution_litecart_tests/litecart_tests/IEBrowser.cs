using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using System;

namespace litecart_tests
{
    [TestFixture]
    public class IEBrowser
    {
        private IWebDriver driver;

        [SetUp]
        public void Start()
        {        
            //InternetExplorerDriverService IEDriverService = InternetExplorerDriverService.CreateDefaultService();
            //IEDriverService.LoggingLevel = InternetExplorerDriverLogLevel.Trace;
            //IEDriverService.LogFile = @"D:\q\1.log"; 
            //driver = new InternetExplorerDriver(IEDriverService);
            
            driver = new InternetExplorerDriver();
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
            Console.Out.WriteLine("типа закрыл");
            driver = null;
        }
    }
}
