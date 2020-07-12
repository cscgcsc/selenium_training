using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace litecart_tests
{
    public class TestBase
    {
        public static Random rnd = new Random();
        protected ApplicationManager applicationManager;
        protected IWebDriver driver;
        protected WebDriverWait wait;

        protected static int GenerateRandomNumber(int maxNumber)
        {
            return Convert.ToInt32(rnd.NextDouble() * maxNumber);
        }

        protected static string GenerateRandomString(int maxLength)
        {
            int rndLengh = rnd.Next(1, maxLength);
            StringBuilder text = new StringBuilder();

            for (int i = 0; i < rndLengh; i++)
            {
                text.Append(Convert.ToChar(rnd.Next(97, 122)));
            }

            return text.ToString();
        }

        protected static DateTime GenerateRandomDate()
        {
            DateTime start = new DateTime(2020, 1, 1);
            int range = (DateTime.Today - start).Days;

            return start.AddDays(rnd.Next(range));
        }

        protected static DateTime GenerateRandomDate(DateTime start)
        {
            int range = (DateTime.Today - start).Days;

            return start.AddDays(rnd.Next(range));
        }

        protected static string GeneratePostcode()
        {
            StringBuilder text = new StringBuilder();

            for (int i = 0; i < 6; i++)
            {
                text.Append(rnd.Next(0, 9));
            }

            return text.ToString();
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
            List<String> valueList = new List<String>();
            foreach (IWebElement option in element.FindElements(By.XPath("./option")))
            {
                string value = option.GetAttribute("value");
                if (String.IsNullOrEmpty(value)) continue;
                valueList.Add(value);
            }
            if (valueList.Count() == 0) return;
            SelectByValue(by, valueList[GenerateRandomNumber(valueList.Count() - 1)]);
        }

        protected void SelectRandomValue(IWebElement element)
        {
            List<String> valueList = new List<String>();
            foreach (IWebElement option in element.FindElements(By.XPath("./option")))
            {
                string value = option.GetAttribute("value");
                if (String.IsNullOrEmpty(value)) continue;
                valueList.Add(value);
            }
            if (valueList.Count() == 0) return;
            SelectByValue(element, valueList[GenerateRandomNumber(valueList.Count() - 1)]);
        }

        protected void ClickRandomRadiobutton(By by)
        {
            List<IWebElement> radiobuttonList = driver.FindElements(by).ToList();
            if (radiobuttonList.Count() == 0) return;
            radiobuttonList[GenerateRandomNumber(radiobuttonList.Count() - 1)].Click();
        }

        protected void ToggleOnRandomCheckbox(By by)
        {
            List<IWebElement> checkboxList = driver.FindElements(by).ToList();
            if (checkboxList.Count() == 0) return;
            IWebElement checkbox = checkboxList[GenerateRandomNumber(checkboxList.Count() - 1)];
            if (!checkbox.Selected) checkbox.Click();
        }

        protected void ToggleOnCheckbox(By by)
        {
            IWebElement checkbox = driver.FindElement(by);           
            if (!checkbox.Selected) checkbox.Click();
        }

        protected void FillPatternField(By by, string text)
        {
            IWebElement element = driver.FindElement(by);
            element.Click();
            element.SendKeys(Keys.Home);
            element.SendKeys(text);
        }

        protected double GetNumber(string cssSize)
        {
            cssSize = Regex.Replace(cssSize, @"[^\d\.,]", "").Replace(".", ",");
            return double.Parse(cssSize);
        }

        protected string[] GetColor(string cssColor)
        {
            return Regex.Replace(cssColor, @"[^\d,]", "").Split(new char[] { ',' });
        }

        protected bool IsGreyColor(string cssColor)
        {
            string[] rgb = GetColor(cssColor);

            return rgb[0] == rgb[1]
                && rgb[0] == rgb[2];
        }

        protected bool IsRedColor(string cssColor)
        {
            string[] rgb = GetColor(cssColor);

            return rgb[1] == "0"
                && rgb[2] == "0";
        }       

        protected void TrySendKeys(IWebElement element, string text)
        {          
            wait.Until(d => {
                element.SendKeys(text); 
                return element.GetAttribute("value") == text;
            }) ;
        }

        protected void TryClick(By by)
        {
            IWebElement element = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
            wait.Until(d => element.Enabled);
            element.Click();
        }

        protected bool IsElementPresent(By by, out IWebElement element)
        {
            ICollection<IWebElement> elements = driver.FindElements(by);
            if(elements.Count > 0)
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

        protected bool IsLoggedInAdminPage()
        {
            return IsElementPresent(By.XPath("//a[contains(@href ,'logout.php')]"), out IWebElement element);
        }

        protected bool IsLoggedInMainPage()
        {
            return IsElementPresent(By.XPath("//div[@id='box-account']//a[contains(@href,'logout')]"), out IWebElement element);
        }

        protected void WaitLeftMenuSubitems(IWebElement element)
        {
            for (int attempt = 0; ; attempt++)
            {
                if (attempt >= 5) break;
                try
                {
                    element.FindElement(By.XPath("./ul/li"));
                    break;
                }
                catch (Exception)
                { }
                Thread.Sleep(100);
            }
        }

        [SetUp]
        protected void SetupTest()
        {
            applicationManager = ApplicationManager.GetInstance();
            driver = applicationManager.Driver;
            wait = applicationManager.Wait;
        }
    }
}
