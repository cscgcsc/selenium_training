using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace litecart_tests
{
    [TestFixture]
    public class AdminPageTests
    {
        private IWebDriver driver;
        public static Random rnd = new Random();

        [SetUp]
        public void Start()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("start-maximized");
            driver = new ChromeDriver(options);

            //FirefoxOptions options = new FirefoxOptions();
            //driver = new FirefoxDriver(options);

            //driver = new InternetExplorerDriver();
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

        [Test]
        public void AddingNewProduct()
        {
            Login();
            driver.FindElement(By.XPath("//ul[@id='box-apps-menu']//a[contains(@href, 'doc=catalog')]")).Click();
            
            List<String> oldProductsList = new List<String>();
            ICollection<IWebElement> rows = driver.FindElements(By.XPath("//table[contains(@class, 'dataTable')]//tr[contains(@class, 'row')]"));
            foreach (IWebElement row in rows)
            {
                List<IWebElement> cells = row.FindElements(By.XPath("./td")).ToList();
                oldProductsList.Add(cells[2].Text);
            }

            driver.FindElement(By.XPath("//td[@id='content']//a[contains(@href, 'doc=edit_product')]")).Click();                   
            
            //tab General
            driver.FindElement(By.XPath("//div[contains(@class, 'tab')]//a[@href='#tab-general']")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//input[@name='status' and @value='1']")).Click();
            string name = GenerateRandomString(10);
            driver.FindElement(By.XPath("//input[@name='name[en]']")).SendKeys(name);
            driver.FindElement(By.XPath("//input[@name='code']")).SendKeys(GenerateRandomString(10));
            ToggleOnRandomCheckbox(By.XPath("//input[@name='categories[]']"));
            SelectRandomValue(By.XPath("//select[@name='default_category_id']"));
            ToggleOnRandomCheckbox(By.XPath("//input[@name='product_groups[]']"));
            driver.FindElement(By.XPath("//input[@name='quantity']")).Clear();
            driver.FindElement(By.XPath("//input[@name='quantity']")).SendKeys(GenerateRandomNumber(5).ToString());
            SelectRandomValue(By.XPath("//select[@name='quantity_unit_id']"));
            SelectRandomValue(By.XPath("//select[@name='delivery_status_id']"));
            SelectRandomValue(By.XPath("//select[@name='sold_out_status_id']"));
            driver.FindElement(By.XPath("//input[@name='new_images[]']")).SendKeys(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"duck.jpg"));
            driver.ExecuteJavaScript("arguments[0].setAttribute('value', arguments[1]);", driver.FindElement(By.XPath("//input[@name='date_valid_from']")), GenerateRandomDate().ToString("yyyy-MM-dd"));

            //tab Information
            driver.FindElement(By.XPath("//div[contains(@class, 'tab')]//a[@href='#tab-information']")).Click();
            SelectRandomValue(By.XPath("//select[@name='manufacturer_id']"));
            SelectRandomValue(By.XPath("//select[@name='supplier_id']"));
            driver.FindElement(By.XPath("//input[@name='keywords']")).SendKeys(GenerateRandomString(10));
            driver.FindElement(By.XPath("//input[@name='short_description[en]']")).SendKeys(GenerateRandomString(10));
            driver.FindElement(By.XPath("//div[@class='trumbowyg-editor']")).SendKeys(GenerateRandomString(100));
            driver.FindElement(By.XPath("//input[@name='head_title[en]']")).SendKeys(GenerateRandomString(10));
            driver.FindElement(By.XPath("//input[@name='meta_description[en]']")).SendKeys(GenerateRandomString(10));

            //tab Prices
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

            WaitForElementPresent(By.XPath("//div[@id='notices']//div[contains(text(),'Changes saved')]"));
            
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

        private void SelectRandomValue(By by)
        {
            IWebElement element = driver.FindElement(by);
            List<String> valueList = new List<String>();
            foreach(IWebElement option in element.FindElements(By.XPath("./option")))
            {
                string value = option.GetAttribute("value");
                if (String.IsNullOrEmpty(value)) continue;
                valueList.Add(value);
            }
            if (valueList.Count() == 0) return;
            SelectByValue(by, valueList[GenerateRandomNumber(valueList.Count() - 1)]);
        }

        private void ClickRandomRadiobutton(By by)
        {
            List<IWebElement> radiobuttonList = driver.FindElements(by).ToList();
            if (radiobuttonList.Count() == 0) return;
            radiobuttonList[GenerateRandomNumber(radiobuttonList.Count() - 1)].Click();
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


        //public static IJavaScriptExecutor Scripts(this IWebDriver driver)
        //{
        //    return (IJavaScriptExecutor)driver;
        //}

        private void ToggleOnRandomCheckbox(By by)
        {
            List<IWebElement> checkboxList = driver.FindElements(by).ToList();
            if (checkboxList.Count() == 0) return;
            IWebElement checkbox = checkboxList[GenerateRandomNumber(checkboxList.Count() - 1)];
            if (!checkbox.Selected) checkbox.Click();
        }

        private void FillPatternField(By by, string text)
        {
            IWebElement element = driver.FindElement(by);
            element.Click();
            element.SendKeys(Keys.Home);          
            element.SendKeys(text);
        }

        private bool IsLoggedIn()
        {
            return IsElementPresent(By.XPath("//a[contains(@href ,'logout.php')]"));
        }

        private void WaitForElementPresent(By by)
        {
            for (int second = 0; ; second++)
            {
                if (second >= 10) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(by)) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
        }

        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        
        //служебные
        public static int GenerateRandomNumber(int maxNumber)
        {
            return rnd.Next(0, maxNumber);
        }

        public static string GenerateRandomString(int maxLength)
        {
            int rndLengh = rnd.Next(1, maxLength);
            StringBuilder text = new StringBuilder();

            for (int i = 0; i < rndLengh; i++)
            {
                text.Append(Convert.ToChar(rnd.Next(97, 122)));
            }

            return text.ToString();
        }

        public static DateTime GenerateRandomDate()
        {
            DateTime start = new DateTime(2020, 1, 1);
            int range = (DateTime.Today - start).Days;

            return start.AddDays(rnd.Next(range));
        }

        public static DateTime GenerateRandomDate(DateTime start)
        {
            int range = (DateTime.Today - start).Days;

            return start.AddDays(rnd.Next(range));
        }

        protected void Select(By element, string value)
        {
            if (value != null)
            {
                new SelectElement(driver.FindElement(element)).SelectByText(value);
            }
        }

        protected void SelectByValue(By element, string value)
        {
            if (value != null)
            {
                new SelectElement(driver.FindElement(element)).SelectByValue(value);
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
