using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace litecart_tests
{
    public class ApplicationManager
    {
        public string baseURL;
        private static ThreadLocal<ApplicationManager> instance = new ThreadLocal<ApplicationManager>();

        private ApplicationManager()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("start-maximized");
            //options.AddArgument("--window-size=500,500");
            //options.PageLoadStrategy = PageLoadStrategy.Normal;
            Driver = new ChromeDriver(options);
            //Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            //FirefoxOptions options = new FirefoxOptions();
            //Driver = new FirefoxDriver(options);

            //Driver = new InternetExplorerDriver();

            baseURL = "http://localhost";
            Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));      
        }

        public static ApplicationManager GetInstance()
        {
            if (!instance.IsValueCreated)
            {
                instance.Value = new ApplicationManager();
            }
            return instance.Value;
        }

        ~ApplicationManager()
        {
            try
            {
                Driver.Quit();
                Driver = null;
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }

        public IWebDriver Driver { get; set; }

        public WebDriverWait Wait { get; set; }

        //InternetExplorerDriverService IEDriverService = InternetExplorerDriverService.CreateDefaultService();
        //IEDriverService.LoggingLevel = InternetExplorerDriverLogLevel.Trace;
        //IEDriverService.LogFile = @"D:\q\1.log"; 
        //driver = new InternetExplorerDriver(IEDriverService);

        //FirefoxOptions options = new FirefoxOptions();
        //options.BrowserExecutableLocation = @"C:\Program Files\Mozilla Firefox\firefox.exe";
        //options.LogLevel = FirefoxDriverLogLevel.Trace;
        //options.BrowserExecutableLocation = @"C:\Program Files\Firefox Nightly\firefox.exe";       
        //options.BrowserExecutableLocation = @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe";
        //options.UseLegacyImplementation = true;

        //ChromeOptions options = new ChromeOptions();
        //options.BinaryLocation = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
        //options.AddArgument("start-maximized");
        //driver = new ChromeDriver(options);
    }
}