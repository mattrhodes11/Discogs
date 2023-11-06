using OpenQA.Selenium;

namespace Discogs.Pages.Base
{
    public abstract class PageObject
    {
        private readonly IWebDriver _driver;

        public PageObject(IWebDriver driver)
        {
            _driver = driver;
        }

        public void NavigateToPage(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }

        public string GetPageTitle()
        {
            return _driver.Title;
        }
    }
}
