using OpenQA.Selenium;

namespace litecart_tests
{
    public class CustomerHelper : HelperBase
    {
        public CustomerHelper(ApplicationManager app) : base(app)
        {
        }

        public void Create(Customer customer)
        {
            OpenEditForm();
            FillInformation(customer);
            SaveForm();
        }

        private void SaveForm()
        {
            driver.FindElement(By.XPath("//button[@name='create_account']")).Click();
            wait.Until(d => app.LoginHelper.IsLoggedInMainPage());
        }

        private void FillInformation(Customer customer)
        {
            TryType(By.XPath("//input[@name='tax_id']"), customer.TaxId);
            Type(By.XPath("//input[@name='company']"), customer.Company);
            Type(By.XPath("//input[@name='firstname']"), customer.Firstname);
            Type(By.XPath("//input[@name='lastname']"), customer.Lastname);
            Type(By.XPath("//input[@name='address1']"), customer.Address1);
            Type(By.XPath("//input[@name='address2']"), customer.Address2);
            Type(By.XPath("//input[@name='postcode']"), customer.Postcode);
            Type(By.XPath("//input[@name='city']"), customer.City);
            SelectByValue(By.XPath("//select[@name='country_code']"), customer.CountryCode);
            Type(By.XPath("//input[@name='email']"), customer.Email);
            Type(By.XPath("//input[@name='phone']"), customer.Phone);
            if(customer.Newsletter) 
                ToggleOn(By.XPath("//input[@name='newsletter']"));
            Type(By.XPath("//input[@name='password']"), customer.Password);
            Type(By.XPath("//input[@name='confirmed_password']"), customer.ConfirmedPassword);
        }

        private void OpenEditForm()
        {
            driver.FindElement(By.XPath("//form[@name='login_form']//a[contains(@href,'create_account')]")).Click();
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("Create Account"));
        }

        public Customer GenerateCustomer()
        {
            string password = TestBase.GenerateRandomString(10);
            return new Customer()
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
}
