using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.IO;
using System.Reflection;

namespace BananaScrape
{
    public static class Drivers
    {
        public static ChromeDriver CreateChromeDriver(string installLocation = "")
        {
            //Disable loading of images.
            ChromeOptions options = new ChromeOptions();
            options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);

            if (!String.IsNullOrEmpty(installLocation))
            {
                options.AddUserProfilePreference("download.default_directory", $"{installLocation}");
            }

            return new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), options);
        }

        public static FirefoxDriver CreateFirefoxDriver(string installLocation = "")
        {

            FirefoxOptions options = new FirefoxOptions();

            //Disable loading of images.
            options.SetPreference("permissions.default.image", 2);

            //Reassign the loopback to ::1. By default it is localhost which is very slow in .net core for some reason: https://stackoverflow.com/questions/53629542/selenium-geckodriver-executes-findelement-10-times-slower-than-chromedriver-ne
            FirefoxDriverService geckoService = FirefoxDriverService.CreateDefaultService(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            geckoService.Host = "::1";
            options.AcceptInsecureCertificates = true;

            if (!String.IsNullOrEmpty(installLocation))
            {
                options.SetPreference("browser.download.folderList", 2);    // Tells Firefox we want to set the download location to a user defined one
                options.SetPreference("browser.download.dir", $"{installLocation}");
            }

            return new FirefoxDriver(geckoService, options);
        }
    }
}
