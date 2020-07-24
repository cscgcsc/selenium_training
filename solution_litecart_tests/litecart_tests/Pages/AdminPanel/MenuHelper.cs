using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace litecart_tests
{
    public class MenuHelper : HelperBase
    {
        public MenuHelper(ApplicationManager app) : base(app)
        {
        }

        public List<IWebElement> GetMenuItems()
        {
            return driver.FindElements(By.XPath("//ul[@id='box-apps-menu']/li")).ToList();
        }

        public List<IWebElement> GetSubmenuItems(int i)
        {
            List<IWebElement> menuItems = GetMenuItems();
            return menuItems[i].FindElements(By.XPath("./ul/li")).ToList();
        }

        public bool IsMenuSelected(int i)
        {
            List<IWebElement> menuItems = GetMenuItems();
            return menuItems[i].GetAttribute("class").Contains("selected");
        }

        public bool IsSubmenuSelected(int i, int n)
        {
            List<IWebElement> submenuItems = GetSubmenuItems(i);
            return submenuItems[n].GetAttribute("class").Contains("selected");
        }

        public bool IsPageHeaderExist()
        {
            return IsElementPresent(By.XPath("//td[@id='content']/h1"));
        }

        public void ClickMenuCountry()
        {
            if (driver.Url == app.baseURL + "/litecart/admin/?app=countries&doc=countries")
                return;

            ClickMenuItem("doc=countries");
        }

        public void ClickMenuGeozone()
        {
            if (driver.Url == app.baseURL + "/litecart/admin/?app=geo_zones&doc=geo_zones")
                return;

            ClickMenuItem("doc=geo_zones");
        }

        public void ClickMenuCatalog()
        {
            if (driver.Url == app.baseURL + "/litecart/admin/?app=catalog&doc=catalog")
                return;

            ClickMenuItem("doc=catalog");
        }

        private void ClickMenuItem(string itemName)
        {
            ICollection<IWebElement> menuItems = driver.FindElements(By.XPath("//ul[@id='box-apps-menu']//a[contains(@href ,'" + itemName + "')]"));
            if (menuItems.Count == 0) 
                throw new Exception("There is no '" + itemName + "' menu item");

            IWebElement element = menuItems.First();
            wait.Until(d=>
            {
                element.Click();
                return IsElementPresent(By.XPath("//ul[@id='box-apps-menu']//a[contains(@href ,'" + itemName + "')]//parent::li[contains(@class ,'selected')]"));
            });
        }
  
        public void ClickMenuByIndex(int i, string[] args)
        {
            List<IWebElement> menuItems = GetMenuItems();
            IWebElement link = menuItems[i].FindElement(By.XPath("./a"));
            args[0] = menuItems[i].Text;

            wait.Until(d =>
            {
                link.Click();
                return IsMenuSelected(i);
            });
            wait.Until(d => IsDocumentReadyStateComplete());
        }

        public void ClickSubmenuByIndex(int i, int n, string[] args)
        {
            List<IWebElement> submenuItems = GetSubmenuItems(i);
            IWebElement link = submenuItems[n].FindElement(By.XPath("./a"));
            args[0] = submenuItems[n].Text;

            wait.Until(d =>
            {
                link.Click();               
                return IsSubmenuSelected(i, n);
            });
            wait.Until(d => IsDocumentReadyStateComplete());
        }
    }
}
