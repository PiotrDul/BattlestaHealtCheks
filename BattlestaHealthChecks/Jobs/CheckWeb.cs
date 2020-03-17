using BattlestaHealthChecks.Context;
using BattlestaHealthChecks.Interfeces;
using BattlestaHealthChecks.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BattlestaHealthChecks.Jobs
{
    public class CheckWeb : ICheckWeb
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public CheckWeb(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task Start(IWebDriver _driver)
        {
            var sample = new Sample();
            var testSettings = await GetSettings();
            var startPage = testSettings.StartPage.ToString();


            long startWeb = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            _driver.Navigate().GoToUrl(startPage);
            long endWeb = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            var mainPageTime = endWeb - startWeb;


            var mainPageElement = new CheckedElement(startPage, "Strona startowa", mainPageTime, sample);
            sample.AddElement(mainPageElement);

            var mainCategory = _driver.FindElement(By.XPath("//div[contains(@class, 'block-categories')]"));
            var categories = mainCategory.FindElements(By.XPath("//*[contains(@class, 'tree')]/child::li"));

            Random rmd = new Random();
            var rIndex = rmd.Next(0, categories.Count - 1);

            var rCategory = categories[rIndex];
            var subcategoryElement = CheckElement(rCategory, _driver, sample);
            sample.AddElement(subcategoryElement);

            var SubCategories = _driver.FindElements(By.XPath("//*[@id='left-column']/div[1]/div[2]/ul/li[" + (rIndex + 1) + "]/ul/child::li"));

            if (SubCategories.Count > 0)
            {
                var rIndex2 = rmd.Next(0, SubCategories.Count - 1);
                var rNextSubcategory = SubCategories[rIndex2];
                var nextSubcategory = CheckElement(rNextSubcategory, _driver, sample);
                sample.AddElement(nextSubcategory);
                Thread.Sleep(2000);

                var anotherElements = _driver.FindElements(By.XPath("//*[@id='left-column']/div[1]/div[2]/ul/li[" + (rIndex + 1) + "]/ul/li[" + (rIndex2 + 1) + "]/ul/child::li"));

                if (anotherElements.Count > 0)
                {
                    var rIndex3 = rmd.Next(0, anotherElements.Count - 1);
                    var anotherElement = anotherElements[rIndex3];
                    var anotherRandomElement = CheckElement(anotherElement, _driver, sample);
                    sample.AddElement(anotherRandomElement);
                }
            }
            try
            {
                await SaveSample(sample);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            var sendMail = false;
            foreach (var element in sample.Elements)
            {
                if (element.LoadTime >= testSettings.MaxLoadTime*1000)
                {
                    sendMail = true;
                    break;
                }
            }
            if(sendMail){
                var emailMessage = $"Sprawdź raport - jedna ze stron ładuje się dłużej niż {testSettings.MaxLoadTime} s";
                SendEmail(emailMessage, testSettings.Email);
            }
            Thread.Sleep(5000);
        }

        private CheckedElement CheckElement(IWebElement webElement, IWebDriver _driver, Sample sample)
        {
            var elementName = webElement.Text;
            var startDate = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            webElement.Click();
            var endDate = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            var loadtime = endDate - startDate;
            Thread.Sleep(2000);
            var url = _driver.Url;

            return new CheckedElement(url, elementName, loadtime, sample);
        }
        
        private async Task SaveSample(Sample sample)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await dbContext.Sample.AddAsync(sample);
                await dbContext.SaveChangesAsync();
            }
        }

        private async Task<SampleSettings> GetSettings()
        {
            SampleSettings settings;
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                settings = await dbContext.SampleSettings.AsNoTracking().FirstOrDefaultAsync();
                if (settings == null)
                {
                    settings = new SampleSettings
                    {
                        MaxLoadTime = 3,
                        StartPage = "https://dev.battlesta.sh/",
                        TimeInterval = 10,
                        Email = "Piotr.Dul@polskieradio.pl"

                    };
                    await dbContext.SampleSettings.AddAsync(settings);
                    await dbContext.SaveChangesAsync();
                }
            }
            return settings;
        }

        private static bool SendEmail(string message, string emial)
        {
            string appkey = "7c2a7e30d1938a1cef6dc9b47bd25c2b4ec8a2f7";
            string secretKey = "873b7c9396ba610db3f17011d2630fa2d54e2f7f";
            var auth = System.Text.Encoding.ASCII.GetBytes((String.Format("{0}:{1}", appkey, secretKey)));
            string authToBase64 = Convert.ToBase64String(auth);
            RestClient client = new RestClient("https://api.emaillabs.net.pl/");
            client.Authenticator = new HttpBasicAuthenticator(appkey, secretKey);
            RestRequest request = new RestRequest("api/new_sendmail", Method.POST);
            request.AddHeader("Authorization", "Basic " + authToBase64);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("smtp_account", "1.pio3m.smtp");
            request.AddParameter("to["+ emial + "][message_id]", emial);
            request.AddParameter("subject", "Czas ładowania strony przekroczył założoną wartość");
            request.AddParameter("text", message);
            request.AddParameter("from", "BattlestaHealthCheck@rojecki.com");
            IRestResponse response = client.Execute(request);
            var content = response.Content;
            return true;
        }

    }
}
