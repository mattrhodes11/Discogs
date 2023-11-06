using Discogs.Pages.Base;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Discogs.Pages
{
    public class SearchResultsPage : PageObject
    {
        private readonly IWebDriver _driver;

        IWebElement SearchResultsHeader => _driver.FindElement(By.XPath("//*[@id='page_content']/h1"));

        IWebElement BodyText => _driver.FindElement(By.TagName("body"));

        public SearchResultsPage(IWebDriver driver) : base(driver) 
        {
            _driver = driver;
        }

        public void CheckResultsForArtistAreDisplayed(string artist)
        {
            var wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("search_results")));
            Assert.True(GetPageTitle().Contains(artist));
            SearchResultsHeader.Text.Should().Contain(artist);
            Assert.False(BodyText.Text.Contains("We couldn't find anything from your original search"));
        }
    }
}
