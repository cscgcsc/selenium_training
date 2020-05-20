﻿using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace litecart_tests
{
    [TestFixture]
    public class ChromeBrowser
    {
        private IWebDriver driver;

        [SetUp]
        public void Start()
        {
            //ChromeOptions options = new ChromeOptions();
            //options.BinaryLocation = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
            //options.AddArgument("start-maximized");
            //driver = new ChromeDriver(options);

            driver = new ChromeDriver();
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
