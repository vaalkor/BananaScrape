using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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

        public static void JavascriptClick(this ChromeDriver driver, string querySelector, int retryCount = 3, int retryInterval = 100)
        {
            //First wait for the element to be visible.
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(x => x.FindElementNullable(By.CssSelector(querySelector)) != null);

            driver.ExecuteScript($"document.querySelector(\"{querySelector}\").click()");
        } 
    }
}
