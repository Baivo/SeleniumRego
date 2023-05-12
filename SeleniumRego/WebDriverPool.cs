using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager;
using System.IO;

namespace SeleniumRego
{
    public class WebDriverPool
    {
        private readonly ConcurrentBag<IWebDriver> _driverPool;
        private readonly int _maxSize;
        private int _currentSize;

        public WebDriverPool(int maxSize)
        {
            _driverPool = new ConcurrentBag<IWebDriver>();
            _maxSize = maxSize;
            _currentSize = 0;
        }

        public async Task<IWebDriver> AcquireAsync()
        {
            IWebDriver driver;

            if (!_driverPool.TryTake(out driver))
            {
                if (Interlocked.Increment(ref _currentSize) <= _maxSize)
                {
                    driver = CreateWebDriver();
                }
                else
                {
                    Interlocked.Decrement(ref _currentSize);

                    while (!_driverPool.TryTake(out driver))
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(100));
                    }
                }
            }

            return driver;
        }

        public void Release(IWebDriver driver)
        {
            _driverPool.Add(driver);
        }

        private IWebDriver CreateWebDriver()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("--headless", "--no-sandbox", "--disable-dev-shm-usage");
            new DriverManager().SetUpDriver(new ChromeConfig());
            return new ChromeDriver(chromeOptions);
        }
    }

}
