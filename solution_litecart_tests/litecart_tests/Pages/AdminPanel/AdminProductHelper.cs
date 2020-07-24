using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace litecart_tests
{
    public class AdminProductHelper : HelperBase
    {
        private List<IWebElement> productLinksCache = null;

        public AdminProductHelper(ApplicationManager app) : base(app)
        {
        }

        public void Create(Product product)
        {
            OpenNewForm();
            FillGeneralInformation(product);
            FillAdditionalInformation(product);
            FillPricesInformation(product);
            AddCampaigns(product);
            SaveForm();
        }

        public ICollection<IWebElement> GetRows()
        {
            return driver.FindElements(By.XPath("//table[contains(@class, 'dataTable')]//tr[contains(@class, 'row')]"));
        }

        public List<IWebElement> GetProductsLinks()
        {
            if (productLinksCache == null)
            {
                productLinksCache = new List<IWebElement>();
                foreach (IWebElement row in GetRows())
                {
                    ICollection<IWebElement> links = row.FindElements(By.XPath(".//a[contains(@href, 'doc=edit_product')]"));
                    //Если это группа товаров, то пропустим
                    if (links.Count == 0)
                        continue;
                    productLinksCache.Add(links.First());
                }
            }
            return new List<IWebElement>(productLinksCache);
        }

        public List<Product> GetAll()
        {
            List<Product> productList = new List<Product>();
            foreach (IWebElement row in GetRows())
            {
                List<IWebElement> cells = row.FindElements(By.XPath("./td")).ToList();
                ICollection<IWebElement> inputs = cells[0].FindElements(By.XPath(".//input"));
                //Если это Root, то пропустим
                if (inputs.Count == 0)
                    continue;

                productList.Add(new Product()
                {
                    Id = inputs.First().GetAttribute("value"),
                    Name = cells[2].Text
                });
            }

            return productList;
        }

        private void OpenNewForm()
        {
            driver.FindElement(By.XPath("//td[@id='content']//a[contains(@href, 'doc=edit_product')]")).Click();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("Add New Product"));
        }

        private void SaveForm()
        {
            driver.FindElement(By.XPath("//button[@name='save']")).Click();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//div[@id='notices']//div[contains(@class,'success')]")));
        }

        private void FillGeneralInformation(Product product)
        {
            SelectTab("tab-general");
            TryType(By.XPath("//input[@name='name[en]']"), product.Name);
            Type(By.XPath("//input[@name='code']"), product.Code);          
            driver.FindElement(By.XPath("//input[@name='status' and @value='" + (product.Enable ? "1" : "0") + "']")).Click();
            ToggleOnCategories(product.CategoriesId);           
            SelectByValue(By.XPath("//select[@name='default_category_id']"), product.DefaultCategoryId);
            ToggleOnProductGroup(product.ProductGroupsId);
            Type(By.XPath("//input[@name='quantity']"), product.Quantity.ToString());
            SelectByValue(By.XPath("//select[@name='quantity_unit_id']"), product.QuantityUnitId);
            SelectByValue(By.XPath("//select[@name='delivery_status_id']"), product.DeliveryStatusId);
            SelectByValue(By.XPath("//select[@name='sold_out_status_id']"), product.SoldOutStatusId);
            Type(By.XPath("//input[@name='new_images[]']"), product.ImagePath);
            driver.ExecuteJavaScript("arguments[0].setAttribute('value', arguments[1]);", driver.FindElement(By.XPath("//input[@name='date_valid_from']")), product.DateValidFrom.ToString("yyyy-MM-dd"));
        }

        private void FillAdditionalInformation(Product product)
        {
            SelectTab("tab-information");
            SelectByValue(By.XPath("//select[@name='manufacturer_id']"), product.ManufacturerId);
            SelectByValue(By.XPath("//select[@name='supplier_id']"), product.SupplierId);
            Type(By.XPath("//input[@name='keywords']"), product.Keywords);
            Type(By.XPath("//input[@name='short_description[en]']"), product.ShortDescription);
            if (!IsElementPresent(By.XPath("//button[contains(@class,'trumbowyg-active')]")))
                driver.FindElement(By.XPath("//button[contains(@class,'trumbowyg-viewHTML-button')]")).Click();
            Type(By.XPath("//textarea[@name='description[en]']"), product.Description);            
            Type(By.XPath("//input[@name='head_title[en]']"), product.HeadTitle);
            Type(By.XPath("//input[@name='meta_description[en]']"), product.MetaDescription);
        }

        private void FillPricesInformation(Product product)
        {
            SelectTab("tab-prices");
            Type(By.XPath("//input[@name='purchase_price']"), product.PurchasePrice.ToString());
            SelectByValue(By.XPath("//select[@name='purchase_price_currency_code']"), product.PurchasePriceCurrencyCode);
            SelectByValue(By.XPath("//select[@name='tax_class_id']"), product.TaxClassId);
            FillPriceAndCurrency(product.Prices);
        }

        private void AddCampaigns(Product product)
        {
            if(product.Campaigns != null)
            {
                for (int i = 0; i < product.Campaigns.Count(); i++)
                {
                    driver.FindElement(By.XPath("//a[@id='add-campaign']")).Click();
                    //driver.ExecuteJavaScript("arguments[0].setAttribute('value', arguments[1]);", driver.FindElement(By.XPath("//input[@name='campaigns[new_" + (i + 1) + "][start_date]']")), GenerateRandomDate().ToString("yyyy-MM-ddThh:mm:ss"));           
                    //driver.ExecuteJavaScript("arguments[0].setAttribute('value', arguments[1]);", driver.FindElement(By.XPath("//input[@name='campaigns[new_" + (i + 1) + "][end_date]']")), GenerateRandomDate().ToString("yyyy-MM-ddThh:mm:ss"));
                    Type(By.XPath("//input[@name='campaigns[new_" + (i + 1) + "][percentage]']"), product.Campaigns[i].Percentage.ToString());
                    FillPriceAndCurrencyCampaign(product.Campaigns[i].Prices, i);
                }
            }            
        }

        private void ToggleOnCategories(List<string> categoriesId)
        {
            if (categoriesId != null)
            {
                foreach (string categoryId in categoriesId)
                {
                    ToggleOn(By.XPath("//input[@name='categories[]' and @value='" + categoryId + "']"));
                }
            }
        }

        private void ToggleOnProductGroup(List<string> groupsId)
        {
            if (groupsId != null)
            {
                foreach (string groupId in groupsId)
                {
                    ToggleOn(By.XPath("//input[@name='product_groups[]' and @value='" + groupId + "']"));
                }
            }
        }

        private void FillPriceAndCurrency(List<Price> prices)
        {
            if (prices != null)
            {
                foreach (Price price in prices)
                {
                    Type(By.XPath("//input[@name='prices[" + price.Currency.Code + "]']"), price.Cost.ToString());
                }
            }
        }

        private void FillPriceAndCurrencyCampaign(List<Price> prices, int i)
        {
            if (prices != null)
            {
                foreach (Price price in prices)
                {
                    Type(By.XPath("//input[@name='campaigns[new_" + (i + 1) + "][" + price.Currency.Code + "]']"), price.Cost.ToString());
                }
            }
        }

        private void SelectTab(string name)
        {
            ICollection<IWebElement> tabs = driver.FindElements(By.XPath("//div[contains(@class, 'tab')]//a[@href='#" + name + "']"));
            if (tabs.Count == 0)
                throw new Exception("There is no '" + name + "' tab");

            tabs.First().Click();
            wait.Until(d => tabs.First().FindElement(By.XPath(".//parent::li[contains(@class, 'active')]")));
        }

        public void OpenProductByIndex(int i)
        {
            List<IWebElement> productLinks = GetProductsLinks();
            productLinks[i].Click();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("Edit Product"));
            productLinksCache = null;
        }

        public void OpenGroupById(int id)
        {
            FindGroupLink(id).Click();
            wait.Until(d => IsDocumentReadyStateComplete());
        }      

        private IWebElement FindGroupLink(int id)
        {
            ICollection<IWebElement> linkList = driver.FindElements(By.XPath("//table[contains(@class, 'dataTable')]//a[contains(@href, 'doc=catalog&category_id=" + id + "')]"));
            if (linkList.Count == 0)
                throw new Exception("There is no product's group with id: " + id + "");

            return linkList.First();
        }
    }
}
