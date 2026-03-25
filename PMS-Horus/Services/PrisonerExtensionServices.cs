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
        string currentUserRole = "Admin";
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
            if (prisoner == null)
            {
                return new ResultService<MedicalRecord>(false, "There are no prisoners registered in the system");
            }
            medicalRecord.PrisonerId = prisoner.PrisonerId;

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

        public async Task<ResultService<MedicalRecord>> GetMedicalRecordAsync(int pidn)
        {
            var prisoner = context.Prisoners.FirstOrDefault(p => p.PersonalIDNumber == pidn);
            if (prisoner == null)
            {
                return new ResultService<MedicalRecord>(false, "No Prisoner Found With This PIDN");
            }

            var medRecord = await context.MedicalRecords.FirstOrDefaultAsync(p => p.PrisonerId == prisoner.PrisonerId);
            if(medRecord == null)
            {
                return new ResultService<MedicalRecord>(false, "No Med Records Available");
            }
            return new ResultService<MedicalRecord>(true, "Medical Record is Available", medRecord);
        }

        public async Task<ResultService<MedicalRecord>> RemoveMedicalRecordAsync(int pidn)
        {
            if (currentUserRole != "Admin" || currentUserRole != "Medic")
            {
                return new ResultService<MedicalRecord>(false, "You are not authorized to perform this kind of action!");
            }
            var prisoner = await context.Prisoners.FirstOrDefaultAsync(p => p.PersonalIDNumber == pidn);
            if (prisoner == null)
            {
                return new ResultService<MedicalRecord>(false, "No prisoner found with the given PIDN");
            }

            var medRecord = await context.MedicalRecords.FirstOrDefaultAsync(m => m.PrisonerId == prisoner.PrisonerId);
            if (medRecord == null)
            {
                return new ResultService<MedicalRecord>(false, "No Med Records Found for this prisoner.");
            }
            try
            {
                context.MedicalRecords.Remove(medRecord);
                await context.SaveChangesAsync();
                return new ResultService<MedicalRecord>(true, $"Medical Record for Prisoner removed");
            }
            catch
            {
                return new ResultService<MedicalRecord>(false, "An error occured");
            }
        }

        public Task<ResultService<MedicalRecord>> UpdateMedicalRecordAsync(int prisonerId)
        {
            throw new NotImplementedException();
        }
    }
}
