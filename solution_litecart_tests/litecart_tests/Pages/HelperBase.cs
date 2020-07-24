using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace litecart_tests
{
    public class HelperBase
    {
        protected ApplicationManager app;
        protected IWebDriver driver;
        protected WebDriverWait wait;
        private ICollection<string> oldWindows = null;
        private string currentWindow = null;
        public static Random rnd = new Random();

        public HelperBase(ApplicationManager app)
        {
            this.app = app;
            driver = app.Driver;
            wait = app.Wait;
        }

        protected bool IsElementPresent(By by)
        {
            return driver.FindElements(by).Count > 0;
        }

        protected bool IsElementPresent(By by, out IWebElement element)
        {
            ICollection<IWebElement> elements = driver.FindElements(by);
            if (elements.Count > 0)
            {
                element = elements.First();
                return true;
            }
            else
            {
                element = null;
                return false;
            }
        }

        protected bool IsElementPresentContext(By by, IWebElement context)
        {
            return context.FindElements(by).Count > 0;
        }

        protected bool IsElementPresentContext(By by, IWebElement context, out IWebElement element)
        {
            ICollection<IWebElement> elements = context.FindElements(by);
            if (elements.Count > 0)
            {
                element = elements.First();
                return true;
            }
            else
            {
                element = null;
                return false;
            }
        }

        protected bool IsDocumentReadyStateComplete()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            string readyState = (string)js.ExecuteScript("return document.readyState");
            return readyState.Equals("complete");
        }       

        protected void Select(By by, string value)
        {
            if (value != null)
            {
                new SelectElement(driver.FindElement(by)).SelectByText(value);
            }
        }

        protected void SelectByValue(By by, string value)
        {
            if (value != null)
            {
                new SelectElement(driver.FindElement(by)).SelectByValue(value);
            }
        }

        protected void SelectByValue(IWebElement element, string value)
        {
            if (value != null)
            {
                new SelectElement(element).SelectByValue(value);
            }
        }

        protected void SelectRandomValue(By by)
        {
            IWebElement element = driver.FindElement(by);
            List<string> valueList = new List<string>();
            foreach (IWebElement option in element.FindElements(By.XPath("./option")))
            {
                string value = option.GetAttribute("value");
                if (string.IsNullOrEmpty(value)) continue;
                valueList.Add(value);
            }
            if (valueList.Count() == 0)
                throw new Exception("There is no options in select menu");

            SelectByValue(by, valueList[TestBase.GenerateRandomNumber(valueList.Count() - 1)]);
        }

        protected void SelectRandomValue(IWebElement element)
        {
            List<string> valueList = new List<string>();
            foreach (IWebElement option in element.FindElements(By.XPath("./option")))
            {
                string value = option.GetAttribute("value");
                if (string.IsNullOrEmpty(value)) continue;
                valueList.Add(value);
            }
            if (valueList.Count() == 0)
                throw new Exception("There is no options in select menu");

            SelectByValue(element, valueList[TestBase.GenerateRandomNumber(valueList.Count() - 1)]);
        }

        protected void ClickRandomRadiobutton(By by)
        {
            List<IWebElement> radiobuttonList = driver.FindElements(by).ToList();
            if (radiobuttonList.Count() == 0)
                throw new Exception("There is no requested radiobutton");

            radiobuttonList[TestBase.GenerateRandomNumber(radiobuttonList.Count() - 1)].Click();
        }

        protected void ToggleOnRandomCheckbox(By by)
        {
            List<IWebElement> checkboxList = driver.FindElements(by).ToList();
            if (checkboxList.Count() == 0)
                throw new Exception("There is no requested checkbox"); 

            IWebElement checkbox = checkboxList[TestBase.GenerateRandomNumber(checkboxList.Count() - 1)];
            if (!checkbox.Selected) checkbox.Click();
        }

        protected void ToggleOn(By by)
        {
            if (!IsElementPresent(by, out IWebElement element))
                throw new Exception("There is no requested checkbox");

            if (!element.Selected) element.Click();
        }

        protected void Type(By by, string value)
        {
            if (value != null)
            {
                IWebElement element = driver.FindElement(by);
                element.Clear();
                element.SendKeys(value);
            }
        }

        protected void TryType(By by, string value)
        {
            if (value != null)
            {
                IWebElement element = driver.FindElement(by);
                wait.Until(d =>
                {
                    element.Clear();
                    element.SendKeys(value);
                    return element.GetAttribute("value") == value;
                });
            }
        }

        protected void SwitchToPreviousWindow()
        {
            driver.Close();
            driver.SwitchTo().Window(currentWindow);
            currentWindow = null;

        }

        protected void SetWindowsHandles()
        {
            currentWindow = driver.CurrentWindowHandle;
            oldWindows = driver.WindowHandles;
        }

        protected void WaitAndSwitchToWindow()
        {
            IEnumerable<string> openedWindow =  wait.Until(d =>
            {
                IEnumerable<string> list = driver.WindowHandles.Except(oldWindows);
                return list.Count() > 0 ? list : null;
            });
            driver.SwitchTo().Window(openedWindow.First());
        }

        public List<string> BrowserLog()
        {
            List<string> messageList = new List<string>();

            foreach (LogEntry entry in driver.Manage().Logs.GetLog("browser"))
            {               
                messageList.Add(entry.Message);                
            }

            return messageList;
        }

        public static string GetProjectPath()
        {
            return Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
        }
    }
}
