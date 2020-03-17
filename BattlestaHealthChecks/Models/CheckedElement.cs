using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BattlestaHealthChecks.Models
{
    public class CheckedElement
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public long LoadTime { get; set; }
        public DateTime Date { get; set; }
        public Sample Sample { get; set; }

        public CheckedElement()
        {
        }

        public CheckedElement(string url, string name, long seconds, Sample sample)
        {
            Url = url;
            Name = name;
            LoadTime = seconds;
            Date = DateTime.Now;
            Sample = sample;
        }
    }
}
