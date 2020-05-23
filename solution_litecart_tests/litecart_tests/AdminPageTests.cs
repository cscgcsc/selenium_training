﻿using NUnit.Framework;
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
            Login();  
            string[] args = new string[1];
            List<IWebElement> menuItems = driver.FindElements(By.XPath("//ul[@id='box-apps-menu']/li")).ToList();

            for (int i = 0; i < menuItems.Count(); i++)
            {
                args[0] = menuItems[i].Text;
                menuItems[i].FindElement(By.XPath("./a")).Click();
                menuItems = driver.FindElements(By.XPath("//ul[@id='box-apps-menu']/li")).ToList();

                Assert.IsTrue(IsElementPresent(By.XPath("//td[@id='content']/h1")), "Page '{0}' doesn't contain h1 header", args);
                Assert.IsTrue(menuItems[i].GetAttribute("class").Contains("selected"), "Page '{0}' is not selected", args);

                WaitLeftMenuSubitems(menuItems[i]);
                List<IWebElement> submenuItems = menuItems[i].FindElements(By.XPath("./ul/li")).ToList();
                
                for (int n = 0; n < submenuItems.Count(); n++)
                {
                    args[0] = submenuItems[n].Text;
                    submenuItems[n].FindElement(By.XPath("./a")).Click();
                    menuItems = driver.FindElements(By.XPath("//ul[@id='box-apps-menu']/li")).ToList();
                    submenuItems = menuItems[i].FindElements(By.XPath("./ul/li")).ToList();

                    Assert.IsTrue(IsElementPresent(By.XPath("//td[@id='content']/h1")), "Page '{0}' doesn't contain h1 header", args);
                    Assert.IsTrue(submenuItems[n].GetAttribute("class").Contains("selected"), "Page '{0}' is not selected", args);
                }               
            }
        }

        [Test]
        public void CountriesOrderingTest()
        {
            Login();           
            driver.FindElement(By.XPath("//ul[@id='box-apps-menu']//span[text()='Countries']//parent::a")).Click();

            List<string> countriesList = new List<string>();

            ICollection<IWebElement> rows = driver.FindElements(By.XPath("//table[contains(@class, 'dataTable')]//tr[contains(@class, 'row')]"));
            foreach (IWebElement row in rows)
            {
                List<IWebElement> cells = row.FindElements(By.XPath("./td")).ToList();
                countriesList.Add(cells[4].Text);
            }

            List<string> sortedCountriesList = new List<string>(countriesList);
            sortedCountriesList.Sort();
            Assert.AreEqual(countriesList, sortedCountriesList, "List of countries is not sorted");
        }

        [Test]
        public void GeozonesOrderingFromCountryPageTest()
        {
            Login();
            driver.FindElement(By.XPath("//ul[@id='box-apps-menu']//span[text()='Countries']/parent::a")).Click();

            List<IWebElement> rows = driver.FindElements(By.XPath("//table[contains(@class, 'dataTable')]//tr[contains(@class, 'row')]")).ToList();
            for (int i = 0; i < rows.Count(); i++)
            {
                List<IWebElement> cells = rows[i].FindElements(By.XPath("./td")).ToList();
                if (cells[5].Text == "0") continue;
                string[] args = {cells[4].Text};
                //перейдем по ссылке на страницу геозон
                cells[4].FindElement(By.XPath("./a")).Click();
                
                //получим список геозон
                List<string> geozonesList = new List<string>();
                ICollection<IWebElement> rowsZones = driver.FindElements(By.XPath("//table[@id='table-zones']//tr[not(contains(@class, 'header'))]"));
                foreach (IWebElement rowZones in rowsZones)
                {
                    List<IWebElement> cellsZones = rowZones.FindElements(By.XPath("./td")).ToList();
                    //если это строка добавления новой геозоны, то пропустим
                    if (cellsZones[3].FindElements(By.XPath(".//button[@name='add_zone']")).Count() != 0) continue;
                    geozonesList.Add(cellsZones[2].Text);
                }

                List<string> sortedGeozonesList = new List<string>(geozonesList);
                sortedGeozonesList.Sort();
                Assert.AreEqual(geozonesList, sortedGeozonesList, "List of {0} geozones is not sorted", args);
                
                //вернемся назад
                driver.Navigate().Back();
                rows = driver.FindElements(By.XPath("//table[contains(@class, 'dataTable')]//tr[contains(@class, 'row')]")).ToList();
            }
        }

        [Test]
        public void GeozonesOrderingTest()
        {
            Login();
            driver.FindElement(By.XPath("//ul[@id='box-apps-menu']//span[text()='Geo Zones']/parent::a")).Click();

            List<IWebElement> rows = driver.FindElements(By.XPath("//table[contains(@class, 'dataTable')]//tr[contains(@class, 'row')]")).ToList();
            for (int i = 0; i < rows.Count(); i++)
            {
                List<IWebElement> cells = rows[i].FindElements(By.XPath("./td")).ToList();               
                string[] args = {cells[2].Text};
                //перейдем по ссылке на страницу геозон
                cells[2].FindElement(By.XPath("./a")).Click();

                //получим список геозон
                List<string> geozonesList = new List<string>();
                ICollection<IWebElement> rowsZones = driver.FindElements(By.XPath("//table[@id='table-zones']//tr[not(contains(@class, 'header'))]"));
                foreach (IWebElement rowZones in rowsZones)
                {
                    List<IWebElement> cellsZones = rowZones.FindElements(By.XPath("./td")).ToList();
                    //если это строка добавления новой геозоны, то пропустим
                    if (cellsZones.Count() == 1) continue;
                    geozonesList.Add(cellsZones[2].FindElement(By.XPath(".//option[@selected]")).Text);
                }

                List<string> sortedGeozonesList = new List<string>(geozonesList);
                sortedGeozonesList.Sort();
                Assert.AreEqual(geozonesList, sortedGeozonesList, "List of {0} geozones is not sorted", args);

                //вернемся назад
                driver.Navigate().Back();
                rows = driver.FindElements(By.XPath("//table[contains(@class, 'dataTable')]//tr[contains(@class, 'row')]")).ToList();
            }
        }
       
        private void Login()
        {
            driver.Url = "http://localhost/litecart/admin/";
            if (!IsLoggedIn())
            {
                driver.FindElement(By.XPath("//input[@name='username']")).SendKeys("admin");
                driver.FindElement(By.XPath("//input[@name='password']")).SendKeys("secret");
                driver.FindElement(By.XPath("//button[@name='login']")).Click();
                WaitForElementPresent(By.XPath("//a[contains(@href ,'logout.php')]"));
            }
        }

        private void WaitLeftMenuSubitems(IWebElement element)
        {
            for (int attempt = 0; ; attempt++)
            {
                if (attempt >= 5) break;
                try
                {
                    element.FindElement(By.XPath("./ul/li"));
                    break;
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

        //private void ClickAllLeftMenuItems(string xPathToFind)
        //{
        //    OldWaitLeftMenuSubitems(By.XPath(xPathToFind));

        //    for (int i = 0; i < driver.FindElements(By.XPath(xPathToFind)).Count(); i++)
        //    {
        //        List<IWebElement> menuItems = driver.FindElements(By.XPath(xPathToFind)).ToList();
        //        object[] args = { menuItems[i].Text };
        //        menuItems[i].Click();
        //        Assert.IsTrue(IsElementPresent(By.XPath("//td[@id='content']/h1")), "Page '{0}' doesn't contain h1 header", args);
        //        Assert.AreEqual("selected", driver.FindElements(By.XPath(xPathToFind)).ToList()[i].GetAttribute("class"), "Page '{0}' is not selected", args);
        //        ClickAllLeftMenuItems(xPathToFind + "[" + (i + 1) + "]/ul/li");
        //    }
        //}

        //private void OldWaitLeftMenuSubitems(By element)
        //{
        //    for (int attempt = 0; ; attempt++)
        //    {
        //        if (attempt >= 3) break;
        //        try
        //        {
        //            if (IsElementPresent(element)) break;
        //        }
        //        catch (Exception)
        //        { }
        //        Thread.Sleep(100);
        //    }
        //}
    }
}
