using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;

namespace litecart_tests
{
    public class ZoneHelper : HelperBase
    {
        public ZoneHelper(ApplicationManager app) : base(app)
        {
        }

        public ICollection<IWebElement> GetRows()
        {
            return driver.FindElements(By.XPath("//table[@id='table-zones']//tr[not(contains(@class, 'header'))]"));
        }

        public List<Zone> GetAll(By by)
        {
            List<Zone> zoneList = new List<Zone>();
            ICollection<IWebElement> rows = GetRows();
            foreach (IWebElement row in rows)
            {
                List<IWebElement> cells = row.FindElements(By.XPath("./td")).ToList();
                //Если это строка добавления новой зоны, то пропустим
                if (row.FindElements(By.XPath(".//*[@id='add_zone' or @name='add_zone']")).Count() != 0)
                    continue;

                zoneList.Add(new Zone()
                {
                    Id = cells[0].Text,
                    Name = cells[2].FindElement(by).Text
                });
            }
            return zoneList;
        }

        public List<Zone> GetAllFromCountryPage()
        {
            return GetAll(By.XPath(".//."));
        }

        public List<Zone> GetAllFromGeozonePage()
        {
            return GetAll(By.XPath(".//option[@selected]"));
        }      
    }
}
