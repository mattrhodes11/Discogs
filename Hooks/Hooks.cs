using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using BoDi;
using Discogs.Factories;
using Discogs.Utility;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Discogs.Hooks
{
    [Binding]
    public sealed class Hooks : ExtentReport
    {
        public string baseUrl = "https://www.discogs.com/";

        private readonly IObjectContainer _container;
        private IWebDriver _driver;
        private static DriverFactory _driverFactory;

        public Hooks(IObjectContainer container) 
        {
            _container = container;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            _driverFactory = new DriverFactory();
            ExtentReportInit();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            ExtentReportTearDown();
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            _feature = _extentReports.CreateTest<Feature>(featureContext.FeatureInfo.Title);
        }

        [BeforeScenario(Order = 1)]
        public void FirstBeforeScenario(ScenarioContext scenarioContext)
        {
            // Add ad-block extension to chrome options
            ChromeOptions options = new ChromeOptions();
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            options.AddExtensions(projectDirectory + "\\BrowserExtensions\\CFHDOJBKJHNKLBPKDAIBDCCDDILIFDDB_3_19_0_0.crx");

            // Start chrome driver and navigate to site under test
            _driver = DriverFactory.CreateDriver(options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(baseUrl);

            // Switch back to the original tab whilst ad-block extension installs
            int attempts = 0;
            int maxAttempts = 20;
            while (_driver.WindowHandles.Count <= 1 && attempts < maxAttempts)
            {
                Thread.Sleep(1000);
                attempts++;
            }

            if (attempts == maxAttempts)
            {
                throw new Exception("Additional browser tab for extension not found");
            }

            _driver.SwitchTo().Window(_driver.WindowHandles[0]);

            // Accept privacy/cookies notice
            var wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 30));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("onetrust-accept-btn-handler"))).Click();

            _container.RegisterInstanceAs(_driver);

            _scenario = _feature.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
        }

        [AfterScenario]
        public void AfterScenario()
        {
             _container.Resolve<IWebDriver>();
            _driver?.Dispose();
        }

        [AfterStep]
        public void AfterStep(ScenarioContext scenarioContext)
        {
            string stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            string stepName = scenarioContext.StepContext.StepInfo.Text;

            _container.Resolve<IWebDriver>();

            // If scenario passed
            if (scenarioContext.TestError == null)
            {
                switch (stepType)
                {
                    case "Given":
                        _scenario.CreateNode<Given>(stepName);
                        break;
                    case "When":
                        _scenario.CreateNode<When>(stepName);
                        break;
                    case "Then":
                        _scenario.CreateNode<Then>(stepName);
                        break;
                    case "And":
                        _scenario.CreateNode<And>(stepName);
                        break;
                }
            }

            // If scenario failed
            if (scenarioContext.TestError != null)
            {
                switch (stepType)
                {
                    case "Given":
                        _scenario.CreateNode<Given>(stepName).Fail(scenarioContext.TestError.Message,
                        MediaEntityBuilder.CreateScreenCaptureFromPath(AddScreenshot(_driver, scenarioContext)).Build());
                        break;
                    case "When":
                        _scenario.CreateNode<When>(stepName).Fail(scenarioContext.TestError.Message,
                        MediaEntityBuilder.CreateScreenCaptureFromPath(AddScreenshot(_driver, scenarioContext)).Build());
                        break;
                    case "Then":
                        _scenario.CreateNode<Then>(stepName).Fail(scenarioContext.TestError.Message,
                         MediaEntityBuilder.CreateScreenCaptureFromPath(AddScreenshot(_driver, scenarioContext)).Build());
                        break;
                    case "And":
                        _scenario.CreateNode<And>(stepName).Fail(scenarioContext.TestError.Message,
                        MediaEntityBuilder.CreateScreenCaptureFromPath(AddScreenshot(_driver, scenarioContext)).Build());
                        break;
                }
            }
        }
    }
}