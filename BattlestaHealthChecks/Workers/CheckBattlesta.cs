using BattlestaHealthChecks.Context;
using BattlestaHealthChecks.Interfeces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenQA.Selenium;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BattlestaHealthChecks.Jobs
{
    public class CheckBattlesta : IHostedService, IDisposable
    {
        private IWebDriver _driver;
        private readonly ICheckWeb _checkWeb;
        private readonly IServiceScopeFactory _scopeFactory;

        public CheckBattlesta(ICheckWeb checkWeb, IServiceScopeFactory scopeFactory)
        {
            _checkWeb = checkWeb;
            _scopeFactory = scopeFactory;
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Factory.StartNew(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    _driver = DriverFactory.ReturnDriver();

                    await _checkWeb.Start(_driver);
                    _driver.Quit();
                    _driver.Dispose();
                    await Task.Delay(await GetTime());
                }
            });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _driver.Quit();
            _driver.Dispose();
            return Task.CompletedTask;
        }

        private async Task<TimeSpan> GetTime()
        {
            var time = 10;
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var settings = await dbContext.SampleSettings.AsNoTracking().FirstOrDefaultAsync();
                if(settings != null)
                {
                    time = settings.TimeInterval;
                }
            }
            return TimeSpan.FromMinutes(time);
        }
    }
}
