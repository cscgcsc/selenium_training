using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace litecart_tests
{
    public class ApplicationManager
    {
        public string baseURL;
        private static ThreadLocal<ApplicationManager> instance = new ThreadLocal<ApplicationManager>();
        private string seleniumHubURL;

        private ApplicationManager()
        {           
            baseURL = "http://localhost";
            seleniumHubURL = "http://10.22.9.86:4444/wd/hub";

            StartRemoteBrowser(GetChromeOptions());
            Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            //Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
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

        private ChromeOptions GetChromeOptions()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("start-maximized");
            options.PlatformName = "WINDOWS";
            //options.AddArgument("--window-size=500,500");
            //options.PageLoadStrategy = PageLoadStrategy.Normal;
            return options;
        }

        private FirefoxOptions GetFirefoxOptions()
        {
            FirefoxOptions options = new FirefoxOptions();
            return options;
        }

        private InternetExplorerOptions GetIEOptions()
        {
            InternetExplorerOptions options = new InternetExplorerOptions();        
            return options;
        }

        private void StartChromeBrowser()
        {          
            Driver = new ChromeDriver(GetChromeOptions());           
        }

        private void StartFirefoxBrowser()
        {
            Driver = new FirefoxDriver(GetFirefoxOptions());
        }

        private void StartIEBrowser()
        {
            Driver = new InternetExplorerDriver(GetIEOptions());
        }

        private void StartRemoteBrowser(DriverOptions options)
        {
            Driver = new RemoteWebDriver(new Uri(seleniumHubURL), options);
        }

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

        //java -jar D:\Programs\allure-2.13.0\bin\selenium-server-standalone-3.141.59.jar -role node -hub "http://10.22.9.86:4444/grid/register" -capabilities browserName=firefox,maxInstances=1 -capabilities browserName=chrome,maxInstances=1 -capabilities browserName=iexplorer,maxInstances=1
    }
}