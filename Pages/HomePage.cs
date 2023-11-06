using Discogs.Pages.Base;
using OpenQA.Selenium;

namespace Discogs.Pages
{
    public class HomePage : PageObject
    {
        private readonly IWebDriver _driver;

        IWebElement SearchArtistsInput => _driver.FindElement(By.XPath("//input[contains(@aria-label, 'Search artists')]"));

        public HomePage(IWebDriver driver) : base(driver) 
        {
            _driver = driver;
        }

        public SearchResultsPage SearchForArtist(string artist)
        {
            SearchArtistsInput.SendKeys(artist);
            SearchArtistsInput.SendKeys(Keys.Enter);
            return new SearchResultsPage(_driver);
        }
    }
}
