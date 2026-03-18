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
    internal class PrisonerExtensionServices : IPrisonerExtensionServices
    {
        public PrisonDBContext context;
        //Can be accessed by admin or medical staff
        public PrisonerExtensionServices(PrisonDBContext context) 
        {
            this.context = context;
        }
        public async Task AddMedicalRecord(MedicalRecord medicalRecord, int prisonerId, string currentUserRole)
        {
            if (currentUserRole != "Medic" || currentUserRole != "Admin")
            {
                throw new UnauthorizedAccessException("You are not authorized to perform this actions!");
            }
            //Check here
            var prisoner = await context.Prisoners.FirstOrDefaultAsync(p => p.PersonalIDNumber == prisonerId);

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

        public async Task<MedicalRecord> GetMedicalRecord(int prisonerId)
        {
            return await context.MedicalRecords.FirstOrDefaultAsync(m => m.PrisonerID == prisonerId);
        }

        public async Task RemoveMedicalRecord(int prisonerId)
        {
            var medicalRecordPrisoner = await context.MedicalRecords.FirstOrDefaultAsync(p => p.PrisonerID == prisonerId);
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

        public Task UpdateMedicalRecord(int prisonerId)
        {
            throw new NotImplementedException();
        }
    }
}
