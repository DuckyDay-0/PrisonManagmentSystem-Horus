using PMS_Horus.Migrations;
using PMS_Horus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS_Horus.Interfaces
{  
    interface IPrisonerExtensionServices
    {
        Task AddMedicalRecord(MedicalRecord medicalRecord, int prisonerId);
        Task RemoveMedicalRecord(int prisonerId);
        Task UpdateMedicalRecord(int prisonerId);
        Task GetMedicalRecord(int prisonerId);
    }   
}
