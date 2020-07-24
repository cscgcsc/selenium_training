using NUnit.Framework;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace litecart_tests
{
    public class TestBase
    {
        public static Random rnd = new Random();
        protected ApplicationManager app;

        public static int GenerateRandomNumber(int max)
        {
            return Convert.ToInt32(rnd.NextDouble() * max);
        }

        public static int GenerateRandomNumber(int min, int max)
        {
            return rnd.Next(min, max);
        }

        public static string GenerateRandomString(int maxLength)
        {
            int rndLengh = rnd.Next(1, maxLength);
            StringBuilder text = new StringBuilder();

            for (int i = 0; i < rndLengh; i++)
            {
                text.Append(Convert.ToChar(rnd.Next(97, 122)));
            }

            return text.ToString();
        }

        public static DateTime GenerateRandomDate()
        {
            DateTime start = new DateTime(2020, 1, 1);
            int range = (DateTime.Today - start).Days;

            return start.AddDays(rnd.Next(range));
        }

        public static DateTime GenerateRandomDate(DateTime start)
        {
            int range = (DateTime.Today - start).Days;

            return start.AddDays(rnd.Next(range));
        }

        public static string GeneratePostcode()
        {
            StringBuilder text = new StringBuilder();

            for (int i = 0; i < 6; i++)
            {
                text.Append(rnd.Next(0, 9));
            }

            return text.ToString();
        }

        public double GetNumber(string cssSize)
        {
            cssSize = Regex.Replace(cssSize, @"[^\d\.,]", "").Replace(".", ",");
            return double.Parse(cssSize);
        }

        public string[] GetColor(string cssColor)
        {
            return Regex.Replace(cssColor, @"[^\d,]", "").Split(new char[] { ',' });
        }

        public bool IsGreyColor(string cssColor)
        {
            string[] rgb = GetColor(cssColor);

            return rgb[0] == rgb[1]
                && rgb[0] == rgb[2];
        }

        public bool IsRedColor(string cssColor)
        {
            string[] rgb = GetColor(cssColor);

            return rgb[1] == "0"
                && rgb[2] == "0";
        }

        [SetUp]
        protected void SetupTest()
        {
            app = ApplicationManager.GetInstance();
        }
    }
}
