using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PMS_Horus.Data;
using PMS_Horus.Interfaces;
using PMS_Horus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS_Horus.Services
{
    public class PrisonerExtensionServices : IPrisonerExtensionServices
    {
        public PrisonDBContext context;
        //Can be accessed by admin or medical staff
        public PrisonerExtensionServices(PrisonDBContext context) 
        {
            this.context = context;
        }
        public async Task<ResultService<MedicalRecord>> AddMedicalRecordAsync(MedicalRecord medicalRecord, string currentUserRole)
        {
            
            if (currentUserRole != "Medic" && currentUserRole != "Admin")
            {
                return new ResultService<MedicalRecord>(false, "You are not authorized to perform this actions!");
            }
            //Check here
            var prisoner = await context.Prisoners.FirstOrDefaultAsync(p => p.PersonalIDNumber == medicalRecord.PrisonerId);

            try
            {
                await context.MedicalRecords.AddAsync(medicalRecord);
                await context.SaveChangesAsync();
                return new ResultService<MedicalRecord>(true, "Medical Record Added!");
            }
            catch
            {
                return new ResultService<MedicalRecord>(false, "//AddMedicalRecordException");
            }         
        }

        public async Task<ResultService<MedicalRecord>> GetMedicalRecordAsync(int prisonerId)
        {
            var medRecord = await context.MedicalRecords.FirstOrDefaultAsync(m => m.PrisonerId == prisonerId);
            if(medRecord == null)
            {
                return new ResultService<MedicalRecord>(false, "No Med Records Available");
            }
            return new ResultService<MedicalRecord>(true, "", medRecord);
        }

        public async Task<ResultService<MedicalRecord>> RemoveMedicalRecordAsync(int prisonerId)
        {
            var medicalRecordPrisoner = await context.MedicalRecords.FirstOrDefaultAsync(p => p.PrisonerId == prisonerId);
            try
            {
                var medRecord = context.MedicalRecords.Remove(medicalRecordPrisoner);
                await context.SaveChangesAsync();
                return new ResultService<MedicalRecord>(true, "Medical Record Removed");
            }
            catch
            {
                return new ResultService<MedicalRecord>(false, "//RemoveRecordException");
            }
        }

        public Task<ResultService<MedicalRecord>> UpdateMedicalRecordAsync(int prisonerId)
        {
            throw new NotImplementedException();
        }
    }
}
