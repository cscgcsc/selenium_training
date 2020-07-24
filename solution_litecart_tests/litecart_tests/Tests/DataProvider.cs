using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace litecart_tests
{
    internal class DataProviders
    {
        public static IEnumerable ValidProduct
        {
            get
            {
                //папки
                List<string> category = new List<string>();
                category.Add("0");
                category.Add("1");

                //группы
                List<string> productGroups = new List<string>();
                productGroups.Add("1-3");
                productGroups.Add("3-5");

                //цены
                List<Price> prices = new List<Price>();
                prices.Add(new Price(TestBase.GenerateRandomNumber(50), "ARG"));
                prices.Add(new Price(TestBase.GenerateRandomNumber(50), "USD"));

                //кампания
                List<Campaign> campaign = new List<Campaign>();
                DateTime startDate = TestBase.GenerateRandomDate();
                campaign.Add(new Campaign()
                {
                    StartDate = startDate,
                    EndDate = TestBase.GenerateRandomDate(startDate),
                    Percentage = TestBase.GenerateRandomNumber(50),
                    Prices = prices
                });

                startDate = TestBase.GenerateRandomDate();

                yield return new Product()
                {
                    Name = TestBase.GenerateRandomString(10),
                    Code = TestBase.GenerateRandomString(10),
                    Enable = true,
                    CategoriesId = category,
                    DefaultCategoryId = "0",
                    ProductGroupsId = productGroups,
                    Quantity = TestBase.GenerateRandomNumber(5),
                    QuantityUnitId = "3",
                    DeliveryStatusId = "1",
                    SoldOutStatusId = "2",
                    ImagePath = Path.Combine(HelperBase.GetProjectPath(), @"duck.jpg"),
                    DateValidFrom = startDate,
                    DateValidTo = TestBase.GenerateRandomDate(startDate),
                    ManufacturerId = "1",
                    SupplierId = "",
                    Keywords = TestBase.GenerateRandomString(10),
                    ShortDescription = TestBase.GenerateRandomString(10),
                    Description = TestBase.GenerateRandomString(100),
                    HeadTitle = TestBase.GenerateRandomString(10),
                    MetaDescription = TestBase.GenerateRandomString(10),
                    PurchasePrice = TestBase.GenerateRandomNumber(50),
                    PurchasePriceCurrencyCode = "EUR",
                    TaxClassId = "",
                    Prices = prices,
                    Campaigns = campaign
                };

                yield return new Product()
                {
                    Name = "TEST",
                    Enable = true               
                };
                /* ... */
            }
        }

        public static IEnumerable ValidCustomer
        {
            get
            {
                string password = TestBase.GenerateRandomString(10);
                yield return new Customer()
                {
                    Firstname = TestBase.GenerateRandomString(10),
                    Lastname = TestBase.GenerateRandomString(10),
                    TaxId = TestBase.GenerateRandomString(10),
                    Company = TestBase.GenerateRandomString(10),
                    Address1 = TestBase.GenerateRandomString(50),
                    Address2 = TestBase.GenerateRandomString(50),
                    Postcode = TestBase.GeneratePostcode(),
                    City = "3",
                    CountryCode = "RU",
                    Email = string.Format("{0}@{1}.{2}", TestBase.GenerateRandomString(10), TestBase.GenerateRandomString(10), TestBase.GenerateRandomString(3)),
                    Phone = TestBase.GenerateRandomNumber(10).ToString(),
                    Newsletter = true,
                    Password = password,
                    ConfirmedPassword = password
                };               
            }
        }

        public static IEnumerable ValidUser
        {
            get
            {               
                yield return new User()
                {
                    Login = "admin",
                    Password = "secret"
                };
            }
        }
    }
}