using BattlestaHealthChecks.Context;
using BattlestaHealthChecks.Interfeces.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BattlestaHealthChecks.Workers
{
    public class BattlestaBackup : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ICreateBackup _createBackup;

        public BattlestaBackup(IServiceScopeFactory scopeFactory, ICreateBackup createBackup)
        {
            _scopeFactory = scopeFactory;
            _createBackup = createBackup;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Factory.StartNew(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await _createBackup.Create();
                    await Task.Delay(await GetTime());
                }
            });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task<TimeSpan> GetTime()
        {
            var time = 24;
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var settings = await dbContext.BackupSettings.AsNoTracking().FirstOrDefaultAsync();
                if (settings != null)
                {
                    time = settings.HourInterval;
                }
            }
            return TimeSpan.FromHours(time);
        }
    }


}
