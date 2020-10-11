using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Firefox;
using System.IO;
using System.Reflection;
using BananaScrape.Interfaces;

namespace BananaScrape.Models
{
    class ChromeWebDriver : IDriver<OpenQA.Selenium.Remote.RemoteWebDriver>
    {
        // public ChromeDriver driver { get; set; }

        public ChromeWebDriver(string directory)
        {
            //This option causes the browser to not load images. To reduce load on server.
            ChromeOptions options = new ChromeOptions();
            options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);

            driver = new ChromeDriver(directory, options);
        }

        public void JavascriptClick(string querySelector)
        {
            //First wait for the element to be visible.
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(x => x.FindElementNullable(By.CssSelector(querySelector)) != null);

            driver.ExecuteScript($"document.querySelector(\"{querySelector}\").click()");
        }
    }
}
