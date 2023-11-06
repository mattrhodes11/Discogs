using Discogs.Pages.Base;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Discogs.Pages
{
    public class MarketplacePage : PageObject
    {
        private readonly IWebDriver _driver;

        IWebElement MarketplaceHeaderBtn => _driver.FindElement(By.XPath("//button[contains(text(), 'Marketplace')]"));

        IWebElement MarketplaceFormatMenuItem(string formatType) => _driver.FindElement(By.XPath("//a[@href='/sell/list?format=" + formatType + "']"));

        IWebElement FilterOption(string selectedFilter) => _driver.FindElement(By.XPath("//a[@class='link_with_count']//span[contains(text(), '" + selectedFilter + "')]"));

        ICollection<IWebElement> ActiveFilters => _driver.FindElements(By.XPath("//div[@class='selected_marketplace_filters']//ul//li//a"));

        public MarketplacePage(IWebDriver driver) : base(driver)
        {
            _driver = driver;
        }

        public void ClickMarketplaceBtnInHeader()
        {
            MarketplaceHeaderBtn.Click();
        }

        public void BrowseMarketPlaceByFormat(string formatType)
        {
            MarketplaceFormatMenuItem(formatType).Click();
            Assert.True(GetPageTitle().Contains(formatType));
        }

        public void FilterItems(string selectedFilter)
        {
            FilterOption(selectedFilter).Click();
        }

        public ICollection<IWebElement> GetActiveFilters()
        {
            return ActiveFilters;
        }
    }
}
