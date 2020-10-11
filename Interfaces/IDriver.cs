using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Text;

namespace BananaScrape.Interfaces
{
    public class IDriver<T>
    {
        public T driver {get; set;}

        public void JavascriptClick(string arg) {}
    }
}
