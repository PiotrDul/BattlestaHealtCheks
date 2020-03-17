using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BattlestaHealthChecks.Models
{
    public class Sample
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<CheckedElement> Elements { get; set; } = new List<CheckedElement>();

        public Sample()
        {
            Date = DateTime.Now;
            if (Elements == null) Elements = new List<CheckedElement>();
        }

        public void AddElement(CheckedElement element)
        {
            var elements = Elements.ToList();
            elements.Add(new CheckedElement(element.Url, element.Name, element.LoadTime, this));
            Elements = elements;
        }
    }
}
