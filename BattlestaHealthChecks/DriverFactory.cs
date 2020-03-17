using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BattlestaHealthChecks
{
    public static class DriverFactory
    {
        public static IWebDriver ReturnDriver()
        {
            IWebDriver _driver = new ChromeDriver(@"C:\Users\sieradzan_m\Downloads\chromedriver_win32", GetOptions());
            return _driver;
        }

        private static ChromeOptions GetOptions()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("start-maximized");

            return chromeOptions;
        }

    }
}
