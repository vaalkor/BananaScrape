using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

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

        public static void JavascriptClick(this ChromeDriver driver, string querySelector) => driver.ExecuteScript(@$"document.querySelector(""{querySelector}"").click()");
    }
}
