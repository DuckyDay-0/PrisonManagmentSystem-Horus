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
        public int Allergies { get; set; }
        public int ChronicConditions { get; set; }


        public int PrisonerID { get; set; }
        public Prisoner prisoner { get; set; }
    }
}
