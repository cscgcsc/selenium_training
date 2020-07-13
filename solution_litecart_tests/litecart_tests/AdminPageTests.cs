using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace litecart_tests
{
    [TestFixture]
    public class AdminPageTests : AdminAuthorizationTestBase
    {
        [Test]
        public void LeftMenuVerificationTest()
        {
            string[] args = new string[1];
            List<IWebElement> menuItems = driver.FindElements(By.XPath("//ul[@id='box-apps-menu']/li")).ToList();
            IWebElement previousHeader = driver.FindElement(By.XPath("//td[@id='content']"));

            for (int i = 0; i < menuItems.Count(); i++)
            {             
                args[0] = menuItems[i].Text;
                menuItems[i].FindElement(By.XPath("./a")).Click();
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.StalenessOf(previousHeader));
                menuItems = driver.FindElements(By.XPath("//ul[@id='box-apps-menu']/li")).ToList();

                Assert.IsTrue(IsElementPresent(By.XPath("//td[@id='content']/h1"), out previousHeader), "Page '{0}' doesn't contain h1 header", args);
                Assert.IsTrue(menuItems[i].GetAttribute("class").Contains("selected"), "Page '{0}' is not selected", args);

                WaitLeftMenuSubitems(menuItems[i]);
                List<IWebElement> submenuItems = menuItems[i].FindElements(By.XPath("./ul/li")).ToList();              
                for (int n = 0; n < submenuItems.Count(); n++)
                {                    
                    args[0] = submenuItems[n].Text;
                    Thread.Sleep(1000);
                    submenuItems[n].FindElement(By.XPath("./a")).Click();
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.StalenessOf(previousHeader));                    
                    menuItems = driver.FindElements(By.XPath("//ul[@id='box-apps-menu']/li")).ToList();
                    submenuItems = menuItems[i].FindElements(By.XPath("./ul/li")).ToList();
        
                    Assert.IsTrue(IsElementPresent(By.XPath("//td[@id='content']/h1"), out previousHeader), "Page '{0}' doesn't contain h1 header", args);
                    Assert.IsTrue(submenuItems[n].GetAttribute("class").Contains("selected"), "Page '{0}' is not selected", args);
                }               
            }
        }

        [Test]
        public void CountriesOrderingTest()
        {         
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

        //[Test]
        public void GeozonesOrderingFromCountryPageTest()
        {
            driver.FindElement(By.XPath("//ul[@id='box-apps-menu']//span[text()='Countries']/parent::a")).Click();

            List<IWebElement> rows = driver.FindElements(By.XPath("//table[contains(@class, 'dataTable')]//tr[contains(@class, 'row')]")).ToList();
            for (int i = 0; i < rows.Count(); i++)
            {
                List<IWebElement> cells = rows[i].FindElements(By.XPath("./td")).ToList();
                if (cells[5].Text == "0") continue;
                string[] args = {cells[4].Text};
                //Перейдем по ссылке на страницу геозон
                cells[4].FindElement(By.XPath("./a")).Click();
                
                //Получим список геозон
                List<string> geozonesList = new List<string>();
                ICollection<IWebElement> rowsZones = driver.FindElements(By.XPath("//table[@id='table-zones']//tr[not(contains(@class, 'header'))]"));
                foreach (IWebElement rowZones in rowsZones)
                {
                    List<IWebElement> cellsZones = rowZones.FindElements(By.XPath("./td")).ToList();
                    //Если это строка добавления новой геозоны, то пропустим
                    if (cellsZones[3].FindElements(By.XPath(".//button[@name='add_zone']")).Count() != 0) continue;
                    geozonesList.Add(cellsZones[2].Text);
                }

                List<string> sortedGeozonesList = new List<string>(geozonesList);
                sortedGeozonesList.Sort();
                Assert.AreEqual(geozonesList, sortedGeozonesList, "List of {0} geozones is not sorted", args);
                
                //Вернемся назад
                driver.Navigate().Back();
                rows = driver.FindElements(By.XPath("//table[contains(@class, 'dataTable')]//tr[contains(@class, 'row')]")).ToList();
            }
        }

        [Test]
        public void GeozonesOrderingTest()
        {
            driver.FindElement(By.XPath("//ul[@id='box-apps-menu']//span[text()='Geo Zones']/parent::a")).Click();

            List<IWebElement> rows = driver.FindElements(By.XPath("//table[contains(@class, 'dataTable')]//tr[contains(@class, 'row')]")).ToList();
            for (int i = 0; i < rows.Count(); i++)
            {
                List<IWebElement> cells = rows[i].FindElements(By.XPath("./td")).ToList();               
                string[] args = {cells[2].Text};
                //Перейдем по ссылке на страницу геозон
                cells[2].FindElement(By.XPath("./a")).Click();

                //Получим список геозон
                List<string> geozonesList = new List<string>();
                ICollection<IWebElement> rowsZones = driver.FindElements(By.XPath("//table[@id='table-zones']//tr[not(contains(@class, 'header'))]"));
                foreach (IWebElement rowZones in rowsZones)
                {
                    List<IWebElement> cellsZones = rowZones.FindElements(By.XPath("./td")).ToList();
                    //Если это строка добавления новой геозоны, то пропустим
                    if (cellsZones.Count() == 1) continue;
                    geozonesList.Add(cellsZones[2].FindElement(By.XPath(".//option[@selected]")).Text);
                }

                List<string> sortedGeozonesList = new List<string>(geozonesList);
                sortedGeozonesList.Sort();
                Assert.AreEqual(geozonesList, sortedGeozonesList, "List of {0} geozones is not sorted", args);

                //Вернемся назад
                driver.Navigate().Back();
                rows = driver.FindElements(By.XPath("//table[contains(@class, 'dataTable')]//tr[contains(@class, 'row')]")).ToList();
            }
        }

        [Test]
        public void AddingNewProductTest()
        {                
            driver.FindElement(By.XPath("//ul[@id='box-apps-menu']//a[contains(@href, 'doc=catalog')]")).Click();

            List<String> oldProductsList = new List<String>();
            ICollection<IWebElement> rows = driver.FindElements(By.XPath("//table[contains(@class, 'dataTable')]//tr[contains(@class, 'row')]"));
            foreach (IWebElement row in rows)
            {
                List<IWebElement> cells = row.FindElements(By.XPath("./td")).ToList();
                oldProductsList.Add(cells[2].Text);
            }
            driver.FindElement(By.XPath("//td[@id='content']//a[contains(@href, 'doc=edit_product')]")).Click();

            //Вкладка General
            driver.FindElement(By.XPath("//div[contains(@class, 'tab')]//a[@href='#tab-general']")).Click();
            string name = GenerateRandomString(10);
            TrySendKeys(driver.FindElement(By.XPath("//input[@name='name[en]']")), name);
            driver.FindElement(By.XPath("//input[@name='code']")).SendKeys(GenerateRandomString(10));
            driver.FindElement(By.XPath("//input[@name='status' and @value='1']")).Click();
            ToggleOnRandomCheckbox(By.XPath("//input[@name='categories[]']"));
            SelectRandomValue(By.XPath("//select[@name='default_category_id']"));
            ToggleOnRandomCheckbox(By.XPath("//input[@name='product_groups[]']"));
            driver.FindElement(By.XPath("//input[@name='quantity']")).Clear();
            driver.FindElement(By.XPath("//input[@name='quantity']")).SendKeys(GenerateRandomNumber(5).ToString());
            SelectRandomValue(By.XPath("//select[@name='quantity_unit_id']"));
            SelectRandomValue(By.XPath("//select[@name='delivery_status_id']"));
            SelectRandomValue(By.XPath("//select[@name='sold_out_status_id']"));
            string projectFolder = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            driver.FindElement(By.XPath("//input[@name='new_images[]']")).SendKeys(Path.Combine(projectFolder, @"duck.jpg"));
            driver.ExecuteJavaScript("arguments[0].setAttribute('value', arguments[1]);", driver.FindElement(By.XPath("//input[@name='date_valid_from']")), GenerateRandomDate().ToString("yyyy-MM-dd"));

            //Вкладка Information
            driver.FindElement(By.XPath("//div[contains(@class, 'tab')]//a[@href='#tab-information']")).Click();
            SelectRandomValue(By.XPath("//select[@name='manufacturer_id']"));
            SelectRandomValue(By.XPath("//select[@name='supplier_id']"));
            driver.FindElement(By.XPath("//input[@name='keywords']")).SendKeys(GenerateRandomString(10));
            driver.FindElement(By.XPath("//input[@name='short_description[en]']")).SendKeys(GenerateRandomString(10));
            driver.FindElement(By.XPath("//textarea[@name='description[en]']")).SendKeys(GenerateRandomString(100));
            driver.FindElement(By.XPath("//input[@name='head_title[en]']")).SendKeys(GenerateRandomString(10));
            driver.FindElement(By.XPath("//input[@name='meta_description[en]']")).SendKeys(GenerateRandomString(10));

            //Вкладка Prices
            driver.FindElement(By.XPath("//div[contains(@class, 'tab')]//a[@href='#tab-prices']")).Click();
            driver.FindElement(By.XPath("//input[@name='purchase_price']")).Clear();
            driver.FindElement(By.XPath("//input[@name='purchase_price']")).SendKeys(GenerateRandomNumber(50).ToString());
            SelectRandomValue(By.XPath("//select[@name='purchase_price_currency_code']"));
            SelectRandomValue(By.XPath("//select[@name='tax_class_id']"));
            driver.FindElement(By.XPath("//input[@name='prices[USD]']")).SendKeys(GenerateRandomNumber(50).ToString());
            driver.FindElement(By.XPath("//input[@name='prices[EUR]']")).SendKeys(GenerateRandomNumber(50).ToString());
            
            driver.FindElement(By.XPath("//a[@id='add-campaign']")).Click();
            //driver.ExecuteJavaScript("arguments[0].setAttribute('value', arguments[1]);", driver.FindElement(By.XPath("//input[@name='campaigns[new_1][start_date]']")), GenerateRandomDate().ToString("yyyy-MM-ddThh:mm:ss"));           
            //driver.ExecuteJavaScript("arguments[0].setAttribute('value', arguments[1]);", driver.FindElement(By.XPath("//input[@name='campaigns[new_1][end_date]']")), GenerateRandomDate().ToString("yyyy-MM-ddThh:mm:ss"));
            driver.FindElement(By.XPath("//input[@name='campaigns[new_1][percentage]']")).Clear();
            driver.FindElement(By.XPath("//input[@name='campaigns[new_1][percentage]']")).SendKeys(GenerateRandomNumber(50).ToString());
            driver.FindElement(By.XPath("//button[@name='save']")).Click();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//div[@id='notices']//div[contains(@class,'success')]")));
                    
            List<String> newProductsList = new List<String>();
            rows = driver.FindElements(By.XPath("//table[contains(@class, 'dataTable')]//tr[contains(@class, 'row')]"));
            foreach (IWebElement row in rows)
            {
                List<IWebElement> cells = row.FindElements(By.XPath("./td")).ToList();
                newProductsList.Add(cells[2].Text);
            }

            oldProductsList.Add(name);
            oldProductsList.Sort();
            newProductsList.Sort();
            Assert.AreEqual(oldProductsList, newProductsList);
        }

        [Test]
        public void OpeningCountryLinksTest()
        {
            driver.FindElement(By.XPath("//ul[@id='box-apps-menu']//a[contains(@href, 'doc=countries')]")).Click();
            driver.FindElement(By.XPath("//td[@id='content']//a[contains(@href, 'doc=edit_country')]")).Click();

            List<IWebElement> links = driver.FindElements(By.XPath("//td[@id='content']//form//a[@target='_blank']")).ToList();

            for (int i = 0; i < links.Count(); i++)
            {
                string currentWindow = driver.CurrentWindowHandle;
                ICollection<String> oldWindows = driver.WindowHandles;
                links[i].Click();
                IEnumerable<String> openedWindow = wait.Until(d =>
                {
                    IEnumerable<String> list = driver.WindowHandles.Except(oldWindows);
                    return list.Count() > 0 ? list : null;
                });
                driver.SwitchTo().Window(openedWindow.First());
                driver.Close();
                driver.SwitchTo().Window(currentWindow);
                links = driver.FindElements(By.XPath("//td[@id='content']//form//a[@target='_blank']")).ToList();
            }         
        }

        [Test]
        public void VerificationBrowserLogsTest()
        {
            driver.FindElement(By.XPath("//ul[@id='box-apps-menu']//a[contains(@href, 'doc=catalog')]")).Click();
            driver.FindElement(By.XPath("//table[contains(@class, 'dataTable')]//a[contains(@href, 'doc=catalog&category_id=1')]")).Click();

            List<IWebElement> rows = driver.FindElements(By.XPath("//table[contains(@class, 'dataTable')]//tr[contains(@class, 'row')]")).ToList();
            for (int i = 0; i < rows.Count(); i++)
            {
                ICollection<IWebElement> links = rows[i].FindElements(By.XPath(".//a[contains(@href, 'doc=edit_product')]"));
                if (links.Count == 0) continue;
                links.First().Click();
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("Edit Product:"));
                driver.Navigate().Back();
                rows = driver.FindElements(By.XPath("//table[contains(@class, 'dataTable')]//tr[contains(@class, 'row')]")).ToList();

                //foreach (LogEntry entry in driver.Manage().Logs.GetLog("browser"))
                //{
                //    Console.WriteLine(entry.Message);
                //}
            }
        }
    }
}
