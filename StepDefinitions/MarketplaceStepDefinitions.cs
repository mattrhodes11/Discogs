using Discogs.Pages;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Discogs.StepDefinitions
{
    [Binding]
    public sealed class MarketplaceStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly MarketplacePage _marketplacePage;

        public MarketplaceStepDefinitions(IWebDriver driver, ScenarioContext scenarioContext)
        {
            _marketplacePage = new MarketplacePage(driver);
            _scenarioContext = scenarioContext;
        }

        [Given(@"I navigate to the marketplace for format type '([^']*)'")]
        public void GivenINavigateToTheMarketplaceForFormatType(string formatType)
        {
            _marketplacePage.ClickMarketplaceBtnInHeader();
            _marketplacePage.BrowseMarketPlaceByFormat(formatType);
        }

        [When(@"I filter items")]
        public void WhenIFilterItems(Table table)
        {
            foreach (var row in table.Rows)
            {
                _scenarioContext.Add(row[0], row[1]);
                _marketplacePage.FilterItems(row[1]);
            }
        }

        [Then(@"my results are filtered correctly")]
        public void ThenMyResultsAreFilteredCorrectly()
        {
            List<string> filtersAppliedInScenario = _scenarioContext.Values.ToList().Select(s => (string)s).ToList();

            foreach (var activeFilter in _marketplacePage.GetActiveFilters())
            {
                if (!activeFilter.Text.Contains("Format"))
                {
                   Assert.True(filtersAppliedInScenario.Any(activeFilter.Text.Contains));
                }
            }
        }
    }
}