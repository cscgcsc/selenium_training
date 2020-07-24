using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace litecart_tests
{
    [TestFixture]
    public class AdminPageTests : AdminAuthorizationTestBase
    {
        [Test]
        public void LeftMenuVerificationTest()
        {
            string[] args = new string[1];
            
            for (int i = 0; i < app.MenuHelper.GetMenuItems().Count(); i++)
            {               
                app.MenuHelper.ClickMenuByIndex(i, args);

                Assert.IsTrue(app.MenuHelper.IsPageHeaderExist(), "Page '{0}' doesn't contain h1 header", args);
                Assert.IsTrue(app.MenuHelper.IsMenuSelected(i), "Menu '{0}' is not selected", args);
               
                for (int n = 0; n < app.MenuHelper.GetSubmenuItems(i).Count(); n++)
                {
                    app.MenuHelper.ClickSubmenuByIndex(i, n, args);
                    Assert.IsTrue(app.MenuHelper.IsPageHeaderExist(), "Page '{0}' doesn't contain h1 header", args);
                    Assert.IsTrue(app.MenuHelper.IsSubmenuSelected(i, n), "Menu '{0}' is not selected", args);
                }
            }
        }
    
        [Test]
        public void CountriesOrderingTest()
        {
            app.MenuHelper.ClickMenuCountry();
            List<Country> countryList = app.CountryHelper.GetAll();
            List<Country> sortedCountryList = new List<Country>(countryList);
            sortedCountryList.Sort();
            Assert.AreEqual(sortedCountryList, countryList, "List of countries is not sorted");
        }

        [Test]
        public void GeozonesOrderingFromCountryPageTest()
        {
            app.MenuHelper.ClickMenuCountry();
            string[] args = new string[1];

            for (int i = 0; i < app.CountryHelper.GetCellsWithGeozone().Count(); i++)
            {
                app.CountryHelper.OpenCountryByIndex(i, args);

                List<Zone> zoneList = app.ZoneHelper.GetAllFromCountryPage();
                List<Zone> sortedZoneList = new List<Zone>(zoneList);
                sortedZoneList.Sort();
                Assert.AreEqual(sortedZoneList, zoneList, "List of {0} geozones is not sorted", args);

                //Вернемся назад
                app.NavigationHelper.Return();
            }
        }

        [Test]
        public void GeozonesOrderingTest()
        {
            app.MenuHelper.ClickMenuGeozone();
            string[] args = new string[1];

            for (int i = 0; i < app.GeozoneHelper.GetRows().Count(); i++)
            {
                app.GeozoneHelper.OpenByIndex(i, args);
                
                //Получим список зон
                List<Zone> zoneList  = app.ZoneHelper.GetAllFromGeozonePage();
                List<Zone> sortedZoneList = new List<Zone>(zoneList);
                sortedZoneList.Sort();
                Assert.AreEqual(sortedZoneList, zoneList, "List of {0} geozones is not sorted", args);

                //Вернемся назад
                app.NavigationHelper.Return();
            }
        }

        [Test, TestCaseSource(typeof(DataProviders), "ValidProduct")]
        public void AddingNewProductTest(Product product)
        {
            app.MenuHelper.ClickMenuCatalog();
           
            List<Product> oldProductsList = app.AdminProductHelper.GetAll();
            app.AdminProductHelper.Create(product);
            List<Product> newProductsList = app.AdminProductHelper.GetAll();

            oldProductsList.Add(product);
            oldProductsList.Sort();
            newProductsList.Sort();
            Assert.AreEqual(oldProductsList, newProductsList);
        }

        [Test]
        public void OpeningCountryLinksTest()
        {
            app.MenuHelper.ClickMenuCountry();
            app.CountryHelper.OpenEditForm();
            for (int i = 0; i < app.CountryHelper.GetEditFormLinks().Count(); i++)
            {
                app.CountryHelper.OpenLinkByIndex(i);
            }         
        }

        [Test]
        public void VerificationBrowserLogsTest()
        {
            app.MenuHelper.ClickMenuCatalog();
            app.AdminProductHelper.OpenGroupById(1);

            for (int i = 0; i < app.AdminProductHelper.GetProductsLinks().Count(); i++)
            {
                app.AdminProductHelper.OpenProductByIndex(i);
                List<string> messageList = app.ProductHelper.BrowserLog();
                foreach (string message in messageList)
                {
                    Console.WriteLine(message);
                }
                Assert.IsTrue(messageList.Count() != 0);
                app.NavigationHelper.Return();
            }
        }
    }
}
