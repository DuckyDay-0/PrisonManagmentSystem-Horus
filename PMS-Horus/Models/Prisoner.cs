using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS_Horus.Models
{
    public class Prisoner
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PersonalIDNumber { get; set; }
        public int Age { get; set; }
        public DateOnly EntryDate { get; set; }
        public int SentenceLenght { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public string Crime { get; set; }
        public string PrisonBlock { get; set; }
        public int PrisonCell { get; set; }

        public MedicalRecord? MedicalRecord { get; set; }
        public List<BehaviorRecord>? BehaviorRecords = new List<BehaviorRecord>();

    }
}
