using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;

namespace litecart_tests
{
    public class GeozoneHelper : HelperBase
    {
        public GeozoneHelper(ApplicationManager app) : base(app)
        {
        }

        public ICollection<IWebElement> GetRows()
        {
            return driver.FindElements(By.XPath("//table[contains(@class, 'dataTable')]//tr[contains(@class, 'row')]"));
        }

        public List<Geozone> GetAll()
        {
            List<Geozone> geozoneList = new List<Geozone>();
            ICollection<IWebElement> rows = GetRows();
            foreach (IWebElement row in rows)
            {
                List<IWebElement> cells = row.FindElements(By.XPath("./td")).ToList();
                geozoneList.Add(new Geozone()
                {
                    Id = cells[1].Text,
                    Name = cells[2].Text
                });
            }
            return geozoneList;
        }

        public void OpenByIndex(int i, string[] args)
        {
            List<IWebElement> geozoneList = GetRows().ToList();
            List<IWebElement> cells = geozoneList[i].FindElements(By.XPath("./td")).ToList();
            args[0] = cells[2].Text;
            cells[2].FindElement(By.XPath("./a")).Click();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("Edit Geo Zone"));
        }
    }
}
