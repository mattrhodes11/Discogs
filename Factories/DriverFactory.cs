using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace Discogs.Factories
{
    public class DriverFactory
    {
        public static IWebDriver CreateDriver(DriverOptions options) => new ChromeDriver((ChromeOptions)options);
    }
}
