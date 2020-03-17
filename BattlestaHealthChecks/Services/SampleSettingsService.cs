using BattlestaHealthChecks.Context;
using BattlestaHealthChecks.Interfeces.Services;
using BattlestaHealthChecks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattlestaHealthChecks.Services
{
    public class SampleSettingsService : ISampleSettingsService
    {
        private readonly DatabaseContext _context;
        public SampleSettingsService(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<SampleSettings> GetSampleSettings()
        {
            return await _context.SampleSettings.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task UpdateBackupSettings(BackupSettings newBackupSettings)
        {
            var settings = await _context.BackupSettings.AsNoTracking().FirstOrDefaultAsync();
            if (settings == null)
            {
                settings = new BackupSettings { HourInterval = newBackupSettings.HourInterval, CredentialPath = newBackupSettings.CredentialPath};
                await _context.BackupSettings.AddAsync(settings);
                await _context.SaveChangesAsync();
                return;
            }
            settings.HourInterval = newBackupSettings.HourInterval;
            settings.CredentialPath = newBackupSettings.CredentialPath;
            _context.BackupSettings.Update(settings);
            await _context.SaveChangesAsync();
            return;
        }

        public async Task<BackupSettings> GetBackupSettings()
        {
            return await _context.BackupSettings.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<SampleSettings> UpdateSampleSettings(SampleSettings newSettings)
        {
            var settings = await _context.SampleSettings.AsNoTracking().FirstOrDefaultAsync();
            if (settings == null)
            {
                settings = new SampleSettings { TimeInterval = newSettings.TimeInterval, MaxLoadTime = newSettings.MaxLoadTime };
                await _context.SampleSettings.AddAsync(settings);
                await _context.SaveChangesAsync();
                return settings;
            }
            settings.TimeInterval = newSettings.TimeInterval;
            settings.MaxLoadTime = newSettings.MaxLoadTime;
            settings.StartPage = newSettings.StartPage;
            settings.Email = newSettings.Email;
            _context.SampleSettings.Update(settings);
            await _context.SaveChangesAsync();

            return settings;
        }
    }
}
