using BattlestaHealthChecks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattlestaHealthChecks.Interfeces.Services
{
    public interface ISampleSettingsService
    {
        Task<SampleSettings> GetSampleSettings();
        Task<SampleSettings> UpdateSampleSettings(SampleSettings newSettings);
        Task UpdateBackupSettings(BackupSettings backupSettings);
        Task<BackupSettings> GetBackupSettings();
    }
}
