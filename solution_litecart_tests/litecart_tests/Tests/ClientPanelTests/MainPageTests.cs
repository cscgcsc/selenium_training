using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace litecart_tests
{
    class MainPageTests : MainTestBase
    {
        [Test]
        public void ProductsStickerTest()
        {
            List<Product> products = app.ProductHelper.GetAll();

            foreach(Product product in products)
            {
                object[] args = {product.Name};
                Assert.AreEqual(1, product.Stickers.Count(), "The number of stickers for '{0}' is not equal 1.", args);
            }
        }

        [Test]
        public void ComparisonProductsInformationTest()
        {
            Product oldProduct = app.ProductHelper.FirstProductFromCampaign();
            string[] args = new string[2];
            args[0] = oldProduct.Name;

            Assert.IsNotNull(oldProduct.PurchasePriceText, "Regular price for '{0}' is not strikethrough.", args);
            Assert.IsNotNull(oldProduct.SalePriceText, "Sale price for '{0}' is not bold.", args);           
            Assert.IsTrue(IsGreyColor(oldProduct.PurchasePriceColor), "Regular price for '{0}' is not grey color.", args);
            Assert.IsTrue(IsRedColor(oldProduct.SalePriceColor), "Sale price for '{0}' is not red color.", args);
            Assert.Greater(GetNumber(oldProduct.SalePriceFont), GetNumber(oldProduct.PurchasePriceFont), "Regular price for '{0}' is equal or greater than sale price.", args);

            //Страница товара 
            app.ProductHelper.OpenProductPage(oldProduct);
            Product newProduct = app.ProductHelper.ProductInfoFromCard();
            args[1] = newProduct.Name;
            Assert.IsNotNull(newProduct.PurchasePriceText, "Regular price for '{1}' is not strikethrough.", args);
            Assert.IsNotNull(newProduct.SalePriceText, "Sale price for '{1}' is not bold.", args); 
            Assert.IsTrue(IsGreyColor(newProduct.PurchasePriceColor), "Regular price for '{1}' is not grey color.", args);
            Assert.IsTrue(IsRedColor(newProduct.SalePriceColor), "Sale price for '{1}' is not red color.", args);
            Assert.Greater(GetNumber(newProduct.SalePriceFont), GetNumber(newProduct.PurchasePriceFont), "Regular price for '{1}' is equal or greater than sale price.", args);

            //Общие проверки
            Assert.AreEqual(newProduct.PurchasePriceText, oldProduct.PurchasePriceText, "Regular price for '{1}' on main page and product's page do not coincide.", args);
            Assert.AreEqual(newProduct.SalePriceText, oldProduct.SalePriceText, "Sale price for '{1}' on main page and product's page do not coincide.", args);
            Assert.AreEqual(newProduct.Name, oldProduct.Name, "Product name on main page and product's page do not coincide. '{0}' and '{1}'", args);
        }

        [Test, TestCaseSource(typeof(DataProviders), "ValidCustomer")]
        public void CreationCustomerTest(Customer customer)
        {
            if (app.LoginHelper.IsLoggedInMainPage())
                app.LoginHelper.LogoutInMainPage();

            //Создание пользователя
            app.CustomerHelper.Create(customer);

            //Logout 
            app.LoginHelper.LogoutInMainPage();

            //Login
            app.LoginHelper.LoginInMainPage(customer);
            Assert.IsTrue(app.LoginHelper.LoginMainPageMessage()
                .Contains(String.Format("You are now logged in as {0} {1}", customer.Firstname, customer.Lastname)), 
                "You are not logged in as {0} {1}", customer.Firstname, customer.Lastname);
        }

        [Test]
        public void AddingProductToCartTest()
        {           
            //Добавим 3 товара 
            for (int i = 0; i < 3; i++)
            {
                app.ProductHelper.AddProductToCart();               
            }

            app.ProductHelper.OpenCart();

            //Удалим из корзины все товары
            for (int i = app.ProductHelper.GetRemoveButtons().Count; i > 0; i--)
            {
                app.ProductHelper.Remove();               
                //Удалена последняя строка
                if (i == 1)
                {
                    Assert.IsTrue(app.ProductHelper.CartMessage().Contains("There are no items in your cart"), "Cart is not empty");
                    break;
                }
                //Проверка иконок, если товаров больше 2
                if (i > 2) 
                    Assert.AreEqual(i - 1, app.ProductHelper.GetProductsShortcuts().Count, "Shortcuts is not deleted");                
                //Проверка таблицы с товарами
                Assert.AreEqual(i - 1, app.ProductHelper.GetOrderTableRows().Count, "Order row is not deleted");
            }

            app.NavigationHelper.GoToMainPage();
        }
    }
}
