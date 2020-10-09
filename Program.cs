using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace BananaScrape
{
    public class Program
    {
        private static ChromeDriver _driver = null;
        private static List<MapInfo> _mapInfo = new List<MapInfo>();
        private static int _downloadPause;

        public static int Main()
        {
            return CommandLine.Run<Program>(CommandLine.Arguments, defaultCommandName: "Scrape");
        }

        [Description("Scrapes a single Gamebanana page and saves metadata about the maps to a json file. Optionally can download the maps as well.")]
        public static int Scrape(string url, bool download = false, int downloadPause = 1000)
        {
            _downloadPause = downloadPause;

            //var url = "https://gamebanana.com/maps/cats/43"; //A good test page. It only had 47 maps on it...

            using (_driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)))
            {
                ScrapePage(url);

                var now = DateTime.Now;
                var filename = $"scrape_data_{now.Day}_{now.Month}_{now.Year}_{now.TimeOfDay.ToString().Replace('.', '_').Replace(':', '_')}.json";
                File.WriteAllText(filename, JsonConvert.SerializeObject(new { Url = url, MapInfo = _mapInfo }, Formatting.Indented));

                if (download)
                {
                    DownloadMaps(_mapInfo.Select(x => x.Link));
                }
            }

            return 0;
        }

        [Description("Downloads all of the files specified in a scrape json file.")]
        public static int Download(string filename, int downloadPause = 1000)
        {
            _downloadPause = downloadPause;

            var fileData = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(filename));
            var mapInfo = fileData["MapInfo"].ToObject<Dictionary<string, object>[]>();

            using (_driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)))
            {
                DownloadMaps(mapInfo.Select(x => x["Link"]).Cast<string>());
            }

            return 0;
        }

        private static void DownloadMaps(IEnumerable<string> links)
        {
            foreach (var link in links)
            {
                DownloadMap(link);
                Thread.Sleep(_downloadPause);
            }
        }

        private static void DownloadMap(string link)
        {
            _driver.Navigate().GoToUrl(link);
            _driver.JavascriptClick(".DownloadOptions > .GreenColor");
            Thread.Sleep(100);
            _driver.JavascriptClick(".DownloadOptions > .GreenColor");
        }

        private static void ScrapePage(string url)
        {
            _driver.Navigate().GoToUrl(url);

            bool clickAgain;
            do
            {
                clickAgain = (bool)_driver.ExecuteScript(Constants.ClickLoadMoreContentScript);
                Thread.Sleep(200);
            }
            while (clickAgain);
            Thread.Sleep(5000); //Just being paranoid here but heyho let's do it anyhow.

            var records = _driver.FindElementsByTagName("record");
            foreach (var rec in records)
            {
                string link = rec.FindElement(By.CssSelector(".Preview > a")).GetAttribute("href");
                string name = rec.FindElement(By.CssSelector(".Identifiers > .Name")).Text.Replace("\r\n", "");

                var likeCount = rec.FindElementNullable(By.CssSelector(".LikeCount > itemcount"))?.Text ?? "0";
                var viewCount = rec.FindElementNullable(By.CssSelector(".ViewCount > itemcount"))?.Text ?? "0";
                var postCount = rec.FindElementNullable(By.CssSelector(".PostCount > itemcount"))?.Text ?? "0";
                var wasFeatured = rec.FindElementNullable(By.CssSelector(".FeaturedIcon")) != null;

                _mapInfo.Add(new MapInfo(name, link, ToInt(viewCount), ToInt(likeCount), ToInt(postCount), wasFeatured));
            }
        }

        private static int ToInt(string str)
        {
            if (str.EndsWith("k"))  return (int)(float.Parse(str.TrimEnd('k')) * 1000);
            else                    return int.Parse(str);
        }
    }
}
