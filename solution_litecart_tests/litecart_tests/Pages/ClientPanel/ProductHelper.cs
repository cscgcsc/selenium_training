using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace litecart_tests
{
    public class ProductHelper : HelperBase
    {
        public ProductHelper(ApplicationManager app) : base(app)
        {
        }

        public List<Product> GetAll()
        {
            List<Product> productList = new List<Product>();
            foreach (IWebElement li in GetBoxes())
            {
                productList.Add(CurrentInfoFromList(li));
            }
            return productList;
        }

        //------------Cart

        public void AddProductToCart()
        {
            Product product = FirstProductFromBoxes();
            OpenProductPage(product);
            FillRequiredFields();
            Add();
            app.NavigationHelper.GoToMainPage();
        }

        public void RemoveProductFromCart()
        {
            OpenCart();
            Remove();
            app.NavigationHelper.GoToMainPage();
        }

        public void Remove()
        {
            ICollection<IWebElement> elements = GetRemoveButtons();
            if(elements.Count == 0)
                throw new Exception("There is no items in the cart");

            IWebElement orderTable = driver.FindElement(By.XPath("//div[@id='checkout-summary-wrapper']//table"));
            
            //костыль для chrome
            for (int attempt = 0; attempt < 2; attempt++)
            {
                try
                {
                    wait.Until(d => elements.First().Enabled);
                    elements.First().Click();
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.StalenessOf(orderTable));
                    break;
                }
                catch (ElementNotInteractableException) { }
                catch (WebDriverTimeoutException) { }
            }
        }

        private void Add()
        {
            IWebElement element = driver.FindElement(By.XPath("//button[@name='add_cart_product']"));

            //костыль для chrome
            for (int attempt = 0; attempt < 2; attempt++)
            {               
                try
                {
                    string calculatedQuantity = SumCartQuantity();
                    element.Click();
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TextToBePresentInElement(CartQuantityElement(), calculatedQuantity));
                    break;
                }
                catch (WebDriverTimeoutException) { }               
            }         
        }

        public ICollection<IWebElement> GetRemoveButtons()
        {
            return driver.FindElements(By.XPath("//button[@name='remove_cart_item']"));
        }

        private IWebElement CartQuantityElement()
        {
            return driver.FindElement(By.XPath("//div[@id='cart']//span[contains(@class, 'quantity')]"));
        }

        private string CurrentCartQuantity()
        {
            return CartQuantityElement().Text;
        }

        private string ProductQuantity()
        {
            return driver.FindElement(By.XPath("//input[@name='quantity']")).GetAttribute("value");
        }

        public string SumCartQuantity()
        {
            return (int.Parse(CurrentCartQuantity()) + int.Parse(ProductQuantity())).ToString();
        }

        public string SubtractCartQuantity()
        {
            return (int.Parse(CurrentCartQuantity()) - int.Parse(ProductQuantity())).ToString();
        }

        public string CartMessage()
        {
            return driver.FindElement(By.XPath("//div[@id='checkout-cart-wrapper']")).Text;
        }

        //------------Other
        private void FillRequiredFields()
        {
            //Если у товара есть выпадающие списки, то заполним их
            ICollection<IWebElement> selectList = driver.FindElements(By.XPath("//td[contains(@class, 'options')]//select"));
            foreach (IWebElement select in selectList)
            {
                SelectRandomValue(select);
            }
            //Заполним количество
            Type(By.XPath("//input[@name='quantity']"), TestBase.GenerateRandomNumber(1, 5).ToString());
        }

        private ICollection<IWebElement> GetBoxes()
        {
            return driver.FindElements(By.XPath("//ul[contains(@class, 'products')]/li"));
        }

        private ICollection<IWebElement> GetCampaigns()
        {
            return driver.FindElements(By.XPath("//div[@id='box-campaigns']//li"));
        }

        public ICollection<IWebElement> GetOrderTableRows()
        {
            return driver.FindElements(By.XPath("//table[contains(@class,'dataTable')]//td[contains(@class,'item')]"));
        }

        public ICollection<IWebElement> GetProductsShortcuts()
        {
            return driver.FindElements(By.XPath("//ul[contains(@class,'shortcuts')]/li"));
        }

        public Product FirstProductFromBoxes()
        {
            ICollection<IWebElement> elements = GetBoxes();
            return CurrentInfoFromList(elements.First());
        }

        public Product FirstProductFromCampaign()
        {
            ICollection<IWebElement> elements = GetCampaigns();
            return CurrentInfoFromList(elements.First());
        }

        public Product ProductInfoFromCard()
        {
            IWebElement element = driver.FindElement(By.XPath("//div[@id='box-product']"));
            Product product = ProductInfo(element);
            product.Name = element.FindElement(By.XPath(".//h1")).Text;

            return product;
        }

        private Product CurrentInfoFromList(IWebElement element)
        {
            Product product = ProductInfo(element);
            product.Name = element.FindElement(By.XPath(".//div[contains(@class, 'name')]")).Text;
            product.Url = element.FindElement(By.XPath(".//a[contains(@class, 'link')]")).GetAttribute("href");

            return product;
        }

        private Product ProductInfo(IWebElement product)
        {
            Product newProduct = new Product
            {
                Stickers = Stickers(product)
            };

            if (IsElementPresentContext(By.XPath(".//s[contains(@class, 'regular-price')]"), product, out IWebElement regularPrice))
            {
                newProduct.PurchasePriceText = regularPrice.Text;
                newProduct.PurchasePriceColor = regularPrice.GetCssValue("color");
                newProduct.PurchasePriceFont = regularPrice.GetCssValue("font-size");
            }

            if (IsElementPresentContext(By.XPath(".//strong[contains(@class, 'campaign-price')]"), product, out IWebElement salePrice))
            {
                newProduct.SalePriceText = salePrice.Text;
                newProduct.SalePriceColor = salePrice.GetCssValue("color");
                newProduct.SalePriceFont = salePrice.GetCssValue("font-size");
            }

            return newProduct;
        }   

        private List<string> Stickers(IWebElement product)
        {
            List<string> stickers = new List<string>();
            foreach (IWebElement sticker in product.FindElements(By.XPath(".//div[contains(@class, 'sticker')]")))
            {
                stickers.Add(sticker.Text);
            }
            return stickers;
        }

        public void OpenProductPage(Product product)
        {
            app.NavigationHelper.GoToUrl(product.Url);
        }

        //Xpath
        public void OpenCart()
        {
            if (driver.Url == app.NavigationHelper.GetUrl() + "/checkout/")
                return;

            driver.FindElement(By.XPath("//div[@id='cart']//a[contains(text(),'Checkout')]")).Click();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("Checkout"));
        }
    }
}
