using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace litecart_tests
{
    class MainShopPageTests : MainTestBase
    {
        [Test]
        public void ProductsStickerTest()
        {
            ICollection<IWebElement> productsList = driver.FindElements(By.XPath("//ul[contains(@class, 'products')]/li"));

            foreach(IWebElement product in productsList)
            {
                object[] args = {product.FindElement(By.XPath(".//div[contains(@class, 'name')]")).Text};
                Assert.AreEqual(1, product.FindElements(By.XPath(".//div[contains(@class, 'sticker')]")).Count, "The number of stickers for '{0}' is not equal 1.", args);
            }
        }

        [Test]
        public void ComparisonProductsInformationTest()
        {
            //Список товаров
            List<IWebElement> productsList = driver.FindElements(By.XPath("//div[@id='box-campaigns']//li")).ToList();

            string[] oldArgs = {productsList[0].FindElement(By.XPath(".//div[contains(@class, 'name')]")).Text};
            Assert.IsTrue(IsElementPresent(By.XPath(".//s[contains(@class, 'regular-price')]"), productsList[0]), "Regular price for '{0}' is not strikethrough.", oldArgs);
            Assert.IsTrue(IsElementPresent(By.XPath(".//strong[contains(@class, 'campaign-price')]"), productsList[0]), "Sale price for '{0}' is not bold.", oldArgs);
            
            IWebElement oldRegularPrice = productsList[0].FindElement(By.XPath(".//s[contains(@class, 'regular-price')]"));
            IWebElement oldSalePrice = productsList[0].FindElement(By.XPath(".//strong[contains(@class, 'campaign-price')]"));
            string oldRegularPriceText = oldRegularPrice.Text;
            string oldSalePriceText = oldSalePrice.Text;
            Assert.IsTrue(IsGreyColor(oldRegularPrice.GetCssValue("color")), "Regular price for '{0}' is not grey color.", oldArgs);
            Assert.IsTrue(IsRedColor(oldSalePrice.GetCssValue("color")), "Sale price for '{0}' is not red color.", oldArgs);
            Assert.Greater(GetNumber(oldSalePrice.GetCssValue("font-size")), GetNumber(oldRegularPrice.GetCssValue("font-size")), "Regular price for '{0}' is equal or greater than sale price.", oldArgs);       

            //Страница товара
            productsList[0].FindElement(By.XPath("./a")).Click();            
            IWebElement productInfo = driver.FindElement(By.XPath("//div[@id='box-product']"));

            Assert.IsTrue(IsElementPresent(By.XPath(".//s[contains(@class, 'regular-price')]"), productInfo), "Regular price for '{0}' is not strikethrough.", oldArgs);
            Assert.IsTrue(IsElementPresent(By.XPath(".//strong[contains(@class, 'campaign-price')]"), productInfo), "Sale price for '{0}' is not bold.", oldArgs);

            IWebElement newRegularPrice = productInfo.FindElement(By.XPath(".//s[contains(@class, 'regular-price')]"));
            IWebElement newSalePrice = productInfo.FindElement(By.XPath(".//strong[contains(@class, 'campaign-price')]"));
            Assert.IsTrue(IsGreyColor(newRegularPrice.GetCssValue("color")), "Regular price for '{0}' is not grey color.", oldArgs);
            Assert.IsTrue(IsRedColor(newSalePrice.GetCssValue("color")), "Sale price for '{0}' is not red color.", oldArgs);
            Assert.Greater(GetNumber(newSalePrice.GetCssValue("font-size")), GetNumber(newRegularPrice.GetCssValue("font-size")), "Regular price for '{0}' is equal or greater than sale price.", oldArgs);

            //Общие проверки
            Assert.AreEqual(newRegularPrice.Text, oldRegularPriceText, "Regular price for '{0}' on main page and product's page do not coincide.", oldArgs);
            Assert.AreEqual(newSalePrice.Text, oldSalePriceText, "Sale price for '{0}' on main page and product's page do not coincide.", oldArgs);
            Assert.AreEqual(productInfo.FindElement(By.XPath(".//h1")).Text, oldArgs[0], "Product name for '{0}' on main page and product's page do not coincide.", oldArgs);
        }

        [Test]
        public void CreationUserTest()
        {
            if (IsLoggedInMainPage())
            {
                driver.FindElement(By.XPath("//div[@id='box-account']//a[contains(@href,'logout')]")).Click();
                wait.Until(d => !IsLoggedInMainPage());
            }

            //Создание пользователя
            driver.FindElement(By.XPath("//form[@name='login_form']//a[contains(@href,'create_account')]")).Click();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//button[@name='create_account']")));
            TrySendKeys(driver.FindElement(By.XPath("//input[@name='tax_id']")), GenerateRandomString(10));
            driver.FindElement(By.XPath("//input[@name='company']")).SendKeys(GenerateRandomString(10));
            string firstname = GenerateRandomString(10);
            string lastname = GenerateRandomString(10);
            driver.FindElement(By.XPath("//input[@name='firstname']")).SendKeys(firstname);
            driver.FindElement(By.XPath("//input[@name='lastname']")).SendKeys(lastname);
            driver.FindElement(By.XPath("//input[@name='address1']")).SendKeys(GenerateRandomString(10));
            driver.FindElement(By.XPath("//input[@name='address2']")).SendKeys(GenerateRandomString(10));
            driver.FindElement(By.XPath("//input[@name='postcode']")).SendKeys(GeneratePostcode());
            driver.FindElement(By.XPath("//input[@name='city']")).SendKeys(GenerateRandomString(10));
            SelectByValue(By.XPath("//select[@name='country_code']"), "RU");   
            string email = String.Format("{0}@{1}.{2}", GenerateRandomString(10), GenerateRandomString(10), GenerateRandomString(3));
            driver.FindElement(By.XPath("//input[@name='email']")).SendKeys(email);
            driver.FindElement(By.XPath("//input[@name='phone']")).SendKeys(GeneratePostcode());
            ToggleOnCheckbox(By.XPath("//input[@name='newsletter']"));
            string password = GenerateRandomString(20);
            driver.FindElement(By.XPath("//input[@name='password']")).SendKeys(password);
            driver.FindElement(By.XPath("//input[@name='confirmed_password']")).SendKeys(password);
            driver.FindElement(By.XPath("//button[@name='create_account']")).Click();
            wait.Until(d => IsLoggedInMainPage());

            //Logout 
            TryClick(By.XPath("//div[@id='box-account']//a[contains(@href,'logout')]"));
            //driver.FindElement(By.XPath("//div[@id='box-account']//a[contains(@href,'logout')]")).Click();
            wait.Until(d => !IsLoggedInMainPage());

            //Login
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//input[@name='email']"))).SendKeys(email);
            driver.FindElement(By.XPath("//input[@name='password']")).SendKeys(password);
            driver.FindElement(By.XPath("//button[@name='login']")).Click();
            Assert.IsTrue(IsElementPresent(By.XPath("//div[@id='notices']//div[contains(text()," + 
                String.Format("'You are now logged in as {0} {1}'", firstname, lastname) + ")]")), 
                "You are not logged in as {0} {1}", firstname, lastname);
        }

        [Test]
        public void AddingProductToCartTest()
        {
            ICollection<IWebElement> productsList = driver.FindElements(By.XPath("//ul[contains(@class, 'products')]/li"));           
            if (productsList.Count == 0) return;
            
            //Добавим 3 товара 
            for (int i = 0; i < 3; i++)
            {
                productsList = driver.FindElements(By.XPath("//ul[contains(@class, 'products')]/li"));               
                productsList.First().Click();               
                if (IsElementPresent(By.XPath("//select[@name='options[Size]']"))) SelectRandomValue(By.XPath("//select[@name='options[Size]']"));
                IWebElement cartQuantity = driver.FindElement(By.XPath("//div[@id='cart']//span[contains(@class, 'quantity')]"));
                driver.FindElement(By.XPath("//button[@name='add_cart_product']")).Click();
                
                string quantity = driver.FindElement(By.XPath("//input[@name='quantity']")).GetAttribute("value");
                string newQuantity = (int.Parse(cartQuantity.Text) + int.Parse(quantity)).ToString();
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TextToBePresentInElement(cartQuantity, newQuantity));

                driver.Navigate().Back();
            }

            //Удалим из корзины все товары
            driver.FindElement(By.XPath("//div[@id='cart']//a[contains(text(),'Checkout')]")).Click();
            ICollection<IWebElement> buttonsList = driver.FindElements(By.XPath("//button[@name='remove_cart_item']"));

            Thread.Sleep(1000);
            for (int i = buttonsList.Count; i > 0; i--)
            {
                IWebElement removeButton = buttonsList.First();              
                removeButton.Click();  
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.StalenessOf(removeButton));
                //Удалена последняя строка
                if (i == 1)
                {
                    Assert.IsTrue(driver.FindElement(By.XPath("//div[@id='checkout-cart-wrapper']")).Text.Contains("There are no items in your cart"), "Cart is not empty");
                    break;
                }
                //Проверка иконок, если товаров больше 2
                if (i > 2) Assert.AreEqual(i - 1, driver.FindElements(By.XPath("//ul[contains(@class,'shortcuts')]/li")).Count, "Shortcuts is not deleted");
                //Проверка таблицы с товарами
                buttonsList = driver.FindElements(By.XPath("//button[@name='remove_cart_item']"));            
                Assert.AreEqual(i - 1, driver.FindElements(By.XPath("//table[contains(@class,'dataTable')]//td[contains(@class,'item')]")).Count, "Row is not deleted");
            }
            //driver.ExecuteJavaScript("arguments[0].classList.remove('items');", driver.FindElement(By.XPath("//ul[contains(@class,'items')]")));
        }
    }
}
