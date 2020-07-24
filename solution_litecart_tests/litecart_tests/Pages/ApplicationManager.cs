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
        public string language;
        private static ThreadLocal<ApplicationManager> instance = new ThreadLocal<ApplicationManager>();
        public CountryHelper CountryHelper { get; set; }
        public MenuHelper MenuHelper { get; set; }
        public LoginHelper LoginHelper { get; set; }
        public ZoneHelper ZoneHelper { get; set; }
        public GeozoneHelper GeozoneHelper { get; set; }
        public NavigationHelper NavigationHelper { get; set; }
        public AdminProductHelper AdminProductHelper { get; set; }
        public ProductHelper ProductHelper { get; set; }
        public CustomerHelper CustomerHelper { get; set; }

        private ApplicationManager()
        {           
            baseURL = "http://localhost";
            language = "en";
            Driver = Chrome();
            Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));

            CountryHelper = new CountryHelper(this);
            MenuHelper = new MenuHelper(this);
            LoginHelper = new LoginHelper(this);
            ZoneHelper = new ZoneHelper(this);
            GeozoneHelper = new GeozoneHelper(this);
            NavigationHelper = new NavigationHelper(this);
            AdminProductHelper = new AdminProductHelper(this);
            ProductHelper = new ProductHelper(this);
            CustomerHelper = new CustomerHelper(this);
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

        private ChromeDriver Chrome()
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
            //options.Proxy = GetProxy();

            return new ChromeDriver(options);
        }

        private FirefoxDriver Firefox()
        {
            FirefoxOptions options = new FirefoxOptions();
            //options.BrowserExecutableLocation = @"C:\Program Files\Mozilla Firefox\firefox.exe";
            options.BrowserExecutableLocation = @"C:\Program Files\Firefox Nightly\firefox.exe";
            //options.LogLevel = FirefoxDriverLogLevel.Trace;  

            FirefoxProfile profile = new FirefoxProfile();
            profile.SetPreference("network.proxy.allow_hijacking_localhost", true);
            options.Profile = profile;
                     
            return new FirefoxDriver(options);
        }

        private InternetExplorerDriver InternetExplorer()
        {
            InternetExplorerOptions options = new InternetExplorerOptions();
            //InternetExplorerDriverService IEDriverService = InternetExplorerDriverService.CreateDefaultService();
            //IEDriverService.LoggingLevel = InternetExplorerDriverLogLevel.Trace;
            //IEDriverService.LogFile = @"D:\q\111.log";

            return new InternetExplorerDriver(options);
        }

        private Proxy GetProxy()
        {
            Proxy proxy = new Proxy();
            proxy.Kind = ProxyKind.Manual;
            proxy.HttpProxy = "localhost:8866";

            return proxy;
        }

        private void StartRemoteBrowser()
        {
            string seleniumHubURL = "http://10.22.9.86:4444/wd/hub";
            ChromeOptions options = new ChromeOptions();
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