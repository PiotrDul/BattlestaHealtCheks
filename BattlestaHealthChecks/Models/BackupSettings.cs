using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattlestaHealthChecks.Models
{
    public class BackupSettings
    {
        public int Id { get; set; }
        public int HourInterval { get; set; }
        public DateTime LastDate { get; set; }
        public string CredentialPath { get; set; }
    }
}
