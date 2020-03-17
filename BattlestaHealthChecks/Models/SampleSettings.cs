using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattlestaHealthChecks.Models
{
    public class SampleSettings
    {
        public int Id { get; set; }
        public int TimeInterval { get; set; }
        public int MaxLoadTime { get; set; }
        public string StartPage { get; set; }
        public string Email { get; set; }
        
    }
}
