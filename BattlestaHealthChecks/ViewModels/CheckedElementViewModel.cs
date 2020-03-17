using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattlestaHealthChecks.ViewModels
{
    public class CheckedElementViewModel
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public double LoadTime { get; set; }
        public DateTime Date { get; set; }
    }
}
