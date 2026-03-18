using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS_Horus.Models
{
    public class BehaviorRecord
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public string Severity { get; set; }

        public int PrisonerId { get; set; }
        public Prisoner prisoner { get; set; }
    }
}
