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
        public async Task AddMedicalRecordAsync(MedicalRecord medicalRecord, string currentUserRole)
        {
            
            if (currentUserRole != "Medic" && currentUserRole != "Admin")
            {
                throw new UnauthorizedAccessException("You are not authorized to perform this actions!");
            }
            //Check here
            var prisoner = await context.Prisoners.FirstOrDefaultAsync(p => p.PersonalIDNumber == medicalRecord.PrisonerId);

            try
            {
                await context.MedicalRecords.AddAsync(medicalRecord);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new InvalidOperationException("//AddMedicalRecordException");
            }         
        }

        public async Task<MedicalRecord> GetMedicalRecordAsync(int prisonerId)
        {
            return await context.MedicalRecords.FirstOrDefaultAsync(m => m.PrisonerId == prisonerId);
        }

        public async Task RemoveMedicalRecordAsync(int prisonerId)
        {
            var medicalRecordPrisoner = await context.MedicalRecords.FirstOrDefaultAsync(p => p.PrisonerId == prisonerId);
            try
            {
                context.MedicalRecords.Remove(medicalRecordPrisoner);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new Exception("//RemoveRecordException");
            }
        }

        public Task UpdateMedicalRecordAsync(int prisonerId)
        {
            throw new NotImplementedException();
        }
    }
}
