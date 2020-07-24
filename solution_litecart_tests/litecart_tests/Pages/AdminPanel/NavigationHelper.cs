
namespace litecart_tests
{
    public class NavigationHelper : HelperBase
    {
        public NavigationHelper(ApplicationManager app) : base(app)
        {
        }

        public string GetUrl() 
        {
            return string.Format("{0}/litecart/{1}{2}", app.baseURL, app.language, string.IsNullOrWhiteSpace(app.language) ? "" : "/");
        }

        //URL
        public void Return()
        {
            driver.Navigate().Back();
        }

        public void GoToUrl(string url)
        {
            if (driver.Url == url)
                return;

            driver.Url = url;
        }

        public void GoToMainPage()
        {
            GoToUrl(GetUrl());
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("Online Store"));
        }

        public void GoToAdminPage()
        {
            GoToUrl(app.baseURL + "/litecart/admin/");
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TitleContains("My Store"));
        }
    }
}
