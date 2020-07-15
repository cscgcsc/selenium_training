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
      
        private ApplicationManager()
        {           
            baseURL = "http://localhost";

            Proxy proxy = new Proxy();
            proxy.Kind = ProxyKind.Manual;
            proxy.HttpProxy = "localhost:8866";
            DriverOptions options = GetFirefoxOptions();
            options.Proxy = proxy;

            Driver = new FirefoxDriver((FirefoxOptions)options);          
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

        private ChromeOptions GetChromeOptions()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("start-maximized");
            
            //options.SetLoggingPreference(LogType.Browser, LogLevel.Severe);
            //options.AddArgument("--enable-logging");
            //options.AddArgument(@"--log-net-log=D:\qq\3.json");

            //options.AddArgument(@"user-data-dir=D:\q\");
            //options.AddArgument(@"download.default_directory=D:\q\");

            //options.AddArgument("--window-size=500,500");
            //options.PageLoadStrategy = PageLoadStrategy.Normal;
            //options.BinaryLocation = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";

            return options;
        }

        private FirefoxOptions GetFirefoxOptions()
        {
            FirefoxOptions options = new FirefoxOptions();
            FirefoxProfile profile = new FirefoxProfile();
            profile.SetPreference("network.proxy.allow_hijacking_localhost", true);
            options.Profile = profile;
           
            //options.BrowserExecutableLocation = @"C:\Program Files\Mozilla Firefox\firefox.exe";
            //options.BrowserExecutableLocation = @"C:\Program Files\Firefox Nightly\firefox.exe";
            //options.LogLevel = FirefoxDriverLogLevel.Trace;  
            //options.UseLegacyImplementation = true;

            return options;
        }

        private InternetExplorerOptions GetIEOptions()
        {
            InternetExplorerOptions options = new InternetExplorerOptions();           
            return options;
        }

        private InternetExplorerDriverService GetIEDriverService()
        {
            InternetExplorerDriverService IEDriverService = InternetExplorerDriverService.CreateDefaultService();
            IEDriverService.LoggingLevel = InternetExplorerDriverLogLevel.Trace;
            IEDriverService.LogFile = @"D:\q\111.log";

            return IEDriverService;
        }

        private void StartRemoteBrowser(DriverOptions options)
        {
            string seleniumHubURL = "http://10.22.9.86:4444/wd/hub";
            Driver = new RemoteWebDriver(new Uri(seleniumHubURL), options);
            //java -jar D:\Programs\allure-2.13.0\bin\selenium-server-standalone-3.141.59.jar -role node -hub "http://10.22.9.86:4444/grid/register" -capabilities browserName=firefox,maxInstances=1 -capabilities browserName=chrome,maxInstances=1 -capabilities browserName=iexplorer,maxInstances=1
        }

        private void StartCloudBrowser()
        {           
            string seleniumCloudHubURL = "https://hub-cloud.browserstack.com/wd/hub/";
            string cloudUsername = "ololoev8";
            string cloudAutomateKey = "YiGxGfWWszWY3zQ8pZBg";

            ChromeOptions capability = new ChromeOptions();
            capability.AddAdditionalCapability("os", "Windows", true);
            capability.AddAdditionalCapability("os_version", "10", true);
            capability.AddAdditionalCapability("browser", "Chrome", true);
            capability.AddAdditionalCapability("browser_version", "latest", true);
            capability.AddAdditionalCapability("browserstack.local", "false", true);
            capability.AddAdditionalCapability("browserstack.selenium_version", "3.5.2", true);
            capability.AddAdditionalCapability("browserstack.user", cloudUsername, true);
            capability.AddAdditionalCapability("browserstack.key", cloudAutomateKey, true);

            Driver = new RemoteWebDriver(new Uri(seleniumCloudHubURL), capability);
        }
    }
}