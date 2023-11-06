using Discogs.Pages;
using OpenQA.Selenium;

namespace Discogs.StepDefinitions
{
    [Binding]
    public sealed class SearchArtistsStepDefinitions
    {
        private readonly HomePage _homePage;
        private readonly SearchResultsPage _searchResultsPage;

        public SearchArtistsStepDefinitions(IWebDriver driver)
        {
            _homePage = new HomePage(driver);
            _searchResultsPage = new SearchResultsPage(driver);
        }

        [Given(@"I search for (.*)")]
        public void GivenISearchForArtist(string artist)
        {
            _homePage.SearchForArtist(artist);
        }

        [Then(@"results for the (.*) are displayed")]
        public void ThenResultsForTheArtistAreDisplayed(string artist)
        {
            _searchResultsPage.CheckResultsForArtistAreDisplayed(artist);
        }
    }
}