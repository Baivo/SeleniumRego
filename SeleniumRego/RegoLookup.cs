using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
namespace RegoLookup
{
    public class Rego
    {
        [FunctionName("RunLookup")]
        public async Task<IActionResult> RunLookup(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("TMR Rego Lookup HTTP trigger function processed a request.");
            string plate = req.Query["plate"];
            
            
            if (string.IsNullOrEmpty(plate))
            {
                return new BadRequestObjectResult(new Dictionary<string, string> { { "error", "Plate number not provided." } });
            }

            var data = await RegoLookup(plate);

            if (data.Count == 0)
            {
                return new NotFoundObjectResult(new Dictionary<string, string> { { "error", "Vehicle information not found or an error occurred." } });
            }

            return new OkObjectResult(data);
        }
        public static async Task<Dictionary<string, string>> RegoLookup(string plate)
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--headless", "--no-sandbox", "--disable-dev-shm-usage");
            new DriverManager().SetUpDriver(new ChromeConfig());
            
            IWebDriver webDriver = new ChromeDriver(chromeOptions);
            try
            {
                webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);

                webDriver.Navigate().GoToUrl("https://www.service.transport.qld.gov.au/checkrego/application/VehicleSearch.xhtml");

                var tosAccept = By.Id("tAndCForm:confirmButton");
                bool tosWait = await WaitForPageLoad(webDriver, TimeSpan.FromSeconds(10), tosAccept);
                if (!tosWait)
                {
                    return new Dictionary<string, string>();
                }
                var tosButton = webDriver.FindElement(tosAccept);
                tosButton.Click();

                var waitForElementRegoInput = By.Id("vehicleSearchForm:plateNumber");
                bool regoInputVisible = await WaitForPageLoad(webDriver, TimeSpan.FromSeconds(10), waitForElementRegoInput);
                if (!regoInputVisible)
                {
                    return new Dictionary<string, string>(); // Return an empty dictionary
                }

                var regoInput = webDriver.FindElement(waitForElementRegoInput);
                var regoPlate = plate;
                regoInput.SendKeys(regoPlate);

                var searchButton = webDriver.FindElement(By.Id("vehicleSearchForm:confirmButton"));
                searchButton.Click();

                var waitForElementResultsPage = By.Id("j_id_61");
                var waitTimeResultsPage = new TimeSpan(0, 0, 5);
                var waitResultsPage = await WaitForPageLoad(webDriver, waitTimeResultsPage, waitForElementResultsPage);
                if (waitResultsPage != true)
                {
                    return new Dictionary<string, string>();
                }
                else
                {
                    var data = ExtractRegoData(webDriver);
                    return data;
                }
            }
            finally
            {
                webDriver.Quit();
            }
            
        }
        public static Dictionary<string, string> ExtractRegoData(IWebDriver driver)
        {
            var dataElements = driver.FindElements(By.CssSelector("dl.data"));
            var result = new Dictionary<string, string>();

            foreach (var dataElement in dataElements)
            {
                var dtElements = dataElement.FindElements(By.TagName("dt"));
                var ddElements = dataElement.FindElements(By.TagName("dd"));

                for (int i = 0; i < dtElements.Count; i++)
                {
                    var key = dtElements[i].Text.Trim();
                    var value = ddElements[i].Text.Trim();
                    result[key] = value;
                }
            }
            return result;
        }

        public static async Task<bool> WaitForPageLoad(IWebDriver driver, TimeSpan waitTime, By waitForElement)
        {
            var endTime = DateTime.Now.Add(waitTime);

            while (DateTime.Now < endTime)
            {
                try
                {
                    var elementToBeDisplayed = driver.FindElement(waitForElement);
                    if (elementToBeDisplayed.Displayed)
                    {
                        return true;
                    }
                }
                catch (StaleElementReferenceException) { }
                catch (NoSuchElementException) { }
                await Task.Delay(TimeSpan.FromMilliseconds(100));
            }
            return false;
        }
    }
}
