using System;
using System.Collections.Generic;
using System.Text;
using BananaScrape.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace BananaScrape.Models
{
    class FirefoxWebDriver: IDriver<OpenQA.Selenium.Remote.RemoteWebDriver>
    {
        // public FirefoxDriver driver { get; set; }

        public FirefoxWebDriver(string directory)
        {
			//This option causes the browser to not load images. To reduce load on server.
			FirefoxOptions options = new FirefoxOptions();
			// options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);

			// _driver = new FirefoxDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), options);
			FirefoxDriverService geckoService = FirefoxDriverService.CreateDefaultService(directory);
			geckoService.Host = "::1";
			// var firefoxOptions = new FirefoxOptions();
			options.AcceptInsecureCertificates = true;
			driver = new FirefoxDriver(geckoService, options);
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
