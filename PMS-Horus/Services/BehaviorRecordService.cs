using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
    public class BehaviorRecordService : IBehaviorRecordService
    {
        private PrisonDBContext context;
        private string currentUserRole = "Correctional Officer";

        public BehaviorRecordService(PrisonDBContext context) 
        {
            this.context = context;
        }  
        public async Task<ResultService<BehaviorRecord>> AddBehaviorRecordAsync(BehaviorRecord behaviorRecord, string currentUserRole)
        {
            if (currentUserRole != "Correctional Officer" && currentUserRole != "Admin")
            {
                return new ResultService<BehaviorRecord>(false, "You are not authorized to perform this kind of action!");
            }
            var prisoner = await context.Prisoners.FirstOrDefaultAsync(p => p.PersonalIDNumber == behaviorRecord.PrisonerId);
            if (prisoner == null)
            {
                return new ResultService<BehaviorRecord>(false, "There are no prisoners registered in the system");
            }
            behaviorRecord.PrisonerId = prisoner.PrisonerId;

            if (string.IsNullOrWhiteSpace(behaviorRecord.Description) ||
                string.IsNullOrWhiteSpace(behaviorRecord.Severity))
            {
                return new ResultService<BehaviorRecord>(false, "Missing Data!Try Again!");
            }
            
            try
            {
                await context.BehaviorRecords.AddAsync(behaviorRecord);
                await context.SaveChangesAsync();
                return new ResultService<BehaviorRecord>(true, "Behavior Record Added!");
            }
            catch
            {
                return new ResultService<BehaviorRecord>(false, "There was a problem with the data being added!");
            }
        }

        public async Task<ResultService<List<BehaviorRecord>>> GetBehaviorRecordAsync(int pidn, string currentUserRole)
        {
            if (currentUserRole != "Admin" && currentUserRole != "Correctional Officer")
            {
                return new ResultService<List<BehaviorRecord>>(false, "You are not authorized to perform this actions!");
            }
            var prisoner = context.Prisoners.FirstOrDefault(p => p.PersonalIDNumber == pidn);
            if (prisoner == null)
            {
                return new ResultService<List<BehaviorRecord>>(false, "No Prisoner Found With This PIDN");
            }

            var behaviorRecords = await context.BehaviorRecords
                                  .Where(b => b.PrisonerId == prisoner.PrisonerId)
                                  .ToListAsync();

            if (behaviorRecords.IsNullOrEmpty())
            {
                return new ResultService<List<BehaviorRecord>>(false, "No Behavior Records Available");
            }
            return new ResultService<List<BehaviorRecord>>(true, "Behavior Record is Available", behaviorRecords);
        }

        public async Task<ResultService<BehaviorRecord>> RemoveBehaviorRecordAsync(int pidn, string currentUserRole)
        {
            if (currentUserRole != "Admin" && currentUserRole != "Correctional Officer")
            {
                return new ResultService<BehaviorRecord>(false, "You are not authorized to perform this kind of action!");
            }
            var prisoner = await context.Prisoners.FirstOrDefaultAsync(p => p.PersonalIDNumber == pidn);
            if (prisoner == null)
            {
                return new ResultService<BehaviorRecord>(false, "No prisoner found with the given PIDN");
            }

            var behaviorRecord = await context.BehaviorRecords.FirstOrDefaultAsync(m => m.PrisonerId == prisoner.PrisonerId);
            if (behaviorRecord == null)
            {
                return new ResultService<BehaviorRecord>(false, "No Behavior Records Found for this prisoner.");
            }
            try
            {
                context.BehaviorRecords.Remove(behaviorRecord);
                await context.SaveChangesAsync();
                return new ResultService<BehaviorRecord>(true, $"Behavior Record for Prisoner removed");
            }
            catch
            {
                return new ResultService<BehaviorRecord>(false, "An error occured");
            }
        }

        public async Task<ResultService<BehaviorRecord>> UpdateBehaviorRecordAsync(int pidn, int choice, string currentUserRole, string newValue)
        {
            if (currentUserRole != "Admin" && currentUserRole != "Correctional Officer")
            {
                return new ResultService<BehaviorRecord>(false, "You don't have the authorization to perform this action!");
            }

            var behaviorRecord = await context.BehaviorRecords.FirstOrDefaultAsync(p => p.Prisoner.PersonalIDNumber == pidn);
            if (behaviorRecord == null)
            {
                return new ResultService<BehaviorRecord>(false, "No Prisoner found!");
            }

            if (newValue.IsNullOrEmpty())
            {
                return new ResultService<BehaviorRecord>(false, "Invalid Data!");
            }
            try
            {
                switch (choice)
                {
                    case 1:
                        behaviorRecord.Severity = newValue;
                        break;
                    case 2:
                        behaviorRecord.Description = newValue;
                        break;
                    case 3:
                        behaviorRecord.DateTime = DateTime.Parse(newValue);
                        break;

                    default:
                        throw new InvalidDataException("Invalid Option");
                }
                await context.SaveChangesAsync();
                return new ResultService<BehaviorRecord>(true, "Behavior Record Updated");
            }
            catch
            {
                return new ResultService<BehaviorRecord>(false, "Error updating");
            }
        }
    }
}
