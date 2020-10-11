using BananaScrape.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;

namespace BananaScrape
{
    public static class ExtensionMethods
    {
        public static IWebElement FindElementNullable(this IWebElement elem, By by)
        {
            try
            {
                var element = elem.FindElement(by);
                return element;
            }
            catch (NoSuchElementException)
            {
                return null;
            }

        }

        public static IWebElement FindElementNullable(this IWebDriver driver, By by)
        {
            try
            {
                var element = driver.FindElement(by);
                return element;
            }
            catch (NoSuchElementException)
            {
                return null;
            }

        }

        
    }
}
