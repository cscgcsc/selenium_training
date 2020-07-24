using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;
using System;
using System.IO;

namespace litecart_tests
{
    public class CustomEventFiringWebDriver : EventFiringWebDriver
    {
        public CustomEventFiringWebDriver(IWebDriver parentDriver) : base(parentDriver)
        {

        }

        protected override void OnFindElementCompleted(FindElementEventArgs e)
        {
            Console.WriteLine("Element is found: " + e.FindMethod);
        }

        protected override void OnFindingElement(FindElementEventArgs e)
        {
            Console.WriteLine("Finding element: " + e.FindMethod);
        }

        protected override void OnElementClicking(WebElementEventArgs e)
        {
            Console.WriteLine("Clicking on element Name: " + e.Element.GetAttribute("name")
                + " Id: " + e.Element.GetAttribute("id")
                + " Enabled: " + e.Element.Enabled
                + " Displayed: " + e.Element.Displayed);
        }

        protected override void OnElementClicked(WebElementEventArgs e)
        {
            Console.WriteLine("Element is clicked");
        }

        protected override void OnException(WebDriverExceptionEventArgs e)
        {
            string timestamp = DateTime.Now.ToString("yyyy_MM_dd_hhmm");
            string filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Error_" + timestamp + ".png");
            Console.WriteLine("Exception: " + e.ThrownException);
            Console.WriteLine("Screenshot: " + filename);
            ((ITakesScreenshot)e.Driver).GetScreenshot().SaveAsFile(filename);
        }

    }
}
