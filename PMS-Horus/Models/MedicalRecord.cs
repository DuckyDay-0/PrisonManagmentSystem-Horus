using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS_Horus.Models
{
    public class MedicalRecord
    {
        public int Id { get; set; }
        public string BloodType { get; set; }
        public string Allergies { get; set; }
        public string ChronicConditions { get; set; }


        public int PrisonerId { get; set; }
        public Prisoner Prisoner { get; set; }
    }
}
