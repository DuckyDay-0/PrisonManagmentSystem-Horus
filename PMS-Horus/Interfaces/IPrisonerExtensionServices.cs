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
        Task AddMedicalRecordAsync(MedicalRecord medicalRecord,string currentUserRole);
        Task RemoveMedicalRecordAsync(int prisonerId);
        Task UpdateMedicalRecordAsync(int prisonerId);
        Task<MedicalRecord> GetMedicalRecordAsync(int prisonerId);
    }   
}
