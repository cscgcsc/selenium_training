using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;

namespace litecart_tests
{
    public class CountryHelper : HelperBase
    {
        private List<IWebElement> cellsWithGeozoneCache = null;

        public CountryHelper(ApplicationManager app) : base(app)
        {
        }

        public ICollection<IWebElement> GetRows()
        {
            return driver.FindElements(By.XPath("//table[contains(@class, 'dataTable')]//tr[contains(@class, 'row')]"));
        }

        public List<Country> GetAll()
        {
            List<Country> countryList = new List<Country>();
            foreach (IWebElement row in GetRows())
            {
                List<IWebElement> cells = row.FindElements(By.XPath("./td")).ToList();
                countryList.Add(new Country()
                {
                    Id = cells[2].Text,
                    Name = cells[4].Text
                });
            }
            return countryList;
        }

        public List<IWebElement> GetCellsWithGeozone()
        {
            if(cellsWithGeozoneCache == null)
            {
                cellsWithGeozoneCache = new List<IWebElement>();
                foreach (IWebElement row in GetRows())
                {
                    List<IWebElement> cells = row.FindElements(By.XPath("./td")).ToList();
                    //Если у страны нет геозоны, то пропустим
                    if (cells[5].Text == "0")
                        continue;
                    cellsWithGeozoneCache.Add(cells[4]);
                }
            }          
            return new List<IWebElement>(cellsWithGeozoneCache);
        }

        public List<IWebElement> GetEditFormLinks()
        {
            return driver.FindElements(By.XPath("//td[@id='content']//form//a[@target='_blank']")).ToList();
        }

        public void OpenCountryByIndex(int i, string[] args)
        {
            List<IWebElement> countryList = GetCellsWithGeozone();           
            args[0] = countryList[i].Text;
            countryList[i].FindElement(By.XPath("./a")).Click();
            cellsWithGeozoneCache = null;
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("Edit Country"));          
        }

        public void OpenLinkByIndex(int i)
        {
            List<IWebElement> linkList = GetEditFormLinks();
            SetWindowsHandles();
            linkList[i].Click();
            WaitAndSwitchToWindow();
            SwitchToPreviousWindow();
        }

        public void OpenEditForm()
        {
            driver.FindElement(By.XPath("//td[@id='content']//a[contains(@href, 'doc=edit_country')]")).Click();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("Add New Country"));
        }
    }
}
