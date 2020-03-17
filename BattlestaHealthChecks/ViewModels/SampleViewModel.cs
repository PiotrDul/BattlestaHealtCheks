using BattlestaHealthChecks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattlestaHealthChecks.ViewModels
{
    public class SampleViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<CheckedElementViewModel> Elements { get; set; } = new List<CheckedElementViewModel>();
        public int PageIndex { get; private set; }
    }
}
