using OpenQA.Selenium;

namespace litecart_tests
{
    public class LoginHelper : HelperBase
    {
        public LoginHelper(ApplicationManager app) : base(app)
        {
        }

        public void LoginInAdminPage()
        {
            app.NavigationHelper.GoToAdminPage();
            if (!IsLoggedInAdminPage())
            {
                Type(By.XPath("//input[@name='username']"), "admin");
                Type(By.XPath("//input[@name='password']"), "secret");
                driver.FindElement(By.XPath("//button[@name='login']")).Click();
                wait.Until(d=> IsLoggedInAdminPage());           
            }
        }

        public void LoginInAdminPage(User user)
        {
            app.NavigationHelper.GoToAdminPage();
            if (!IsLoggedInAdminPage())
            {
                Type(By.XPath("//input[@name='username']"), user.Login);
                Type(By.XPath("//input[@name='password']"), user.Password);
                driver.FindElement(By.XPath("//button[@name='login']")).Click();
                wait.Until(d => IsLoggedInAdminPage());
            }
        }

        public void LoginInMainPage()
        {
            app.NavigationHelper.GoToMainPage();
            if (!IsLoggedInMainPage())
            {
                Type(By.XPath("//input[@name='email']"), "test@yandex.ru");
                Type(By.XPath("//input[@name='password']"), "123");
                driver.FindElement(By.XPath("//button[@name='login']")).Click();
                wait.Until(d => IsLoggedInMainPage());
            }
        }

        public void LoginInMainPage(Customer customer)
        {
            app.NavigationHelper.GoToMainPage();
            if (!IsLoggedInMainPage())
            {
                Type(By.XPath("//input[@name='email']"), customer.Email);
                Type(By.XPath("//input[@name='password']"), customer.Password);
                driver.FindElement(By.XPath("//button[@name='login']")).Click();
                wait.Until(d => IsLoggedInMainPage());
            }
        }

        public void LogoutInMainPage()
        {
            IWebElement element = driver.FindElement(By.XPath("//div[@id='box-account']//a[contains(@href,'logout')]"));

            //костыль для chrome
            for (int attempt = 0; attempt < 2; attempt++)
            {
                try
                {
                    element.Click();
                    wait.Until(d => !IsLoggedInMainPage());
                    break;
                }
                catch (WebDriverTimeoutException) { }
            }
        }

        public void LogoutInAdminPage()
        {
            driver.FindElement(By.XPath("//a[contains(@href ,'logout.php')]")).Click();
        }

        public bool IsLoggedInAdminPage()
        {
            return IsElementPresent(By.XPath("//a[contains(@href ,'logout.php')]"));
        }

        public bool IsLoggedInMainPage()
        {
            return IsElementPresent(By.XPath("//div[@id='box-account']//a[contains(@href,'logout')]"));
        }

        public string LoginMainPageMessage()
        {
            return driver.FindElement(By.XPath("//div[@id='notices']//div[contains(@class,'notice')]")).Text;
        }
    }
}
