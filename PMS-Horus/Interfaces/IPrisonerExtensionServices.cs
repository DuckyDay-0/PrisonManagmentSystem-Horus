using PMS_Horus.Migrations;
using PMS_Horus.Models;
using PMS_Horus.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS_Horus.Interfaces
{  
    interface IPrisonerExtensionServices
    {
        Task<ResultService<MedicalRecord>> AddMedicalRecordAsync(MedicalRecord medicalRecord,string currentUserRole);
        Task<ResultService<MedicalRecord>> RemoveMedicalRecordAsync(int prisonerId);
        Task<ResultService<MedicalRecord>> UpdateMedicalRecordAsync(int prisonerId);
        Task<ResultService<MedicalRecord>> GetMedicalRecordAsync(int prisonerId);
    }   
}
