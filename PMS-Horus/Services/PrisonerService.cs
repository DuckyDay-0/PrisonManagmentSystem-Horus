using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PMS_Horus.Data;
using PMS_Horus.Interfaces;
using PMS_Horus.Models;
using Reqnroll.Formatters.PayloadProcessing.Cucumber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS_Horus.Services
{
    public class PrisonerService : IPrisonerServices
    {
        private readonly PrisonDBContext context;

        public PrisonerService(PrisonDBContext context)
        {
            this.context = context;
        }

        public async Task<ResultService<Prisoner>> AddPrisonerAsync(Prisoner prisoner, string currentUserRole)
        {

            int minimumAgeForPrison = 18;
            if (currentUserRole != "Admin")
            {
                return new ResultService<Prisoner>(false, "You are not authorized to perform this action!");
            }

            if (string.IsNullOrWhiteSpace(prisoner.FirstName) ||
                string.IsNullOrWhiteSpace(prisoner.LastName) ||
                string.IsNullOrWhiteSpace(prisoner.Crime) ||
                string.IsNullOrWhiteSpace(prisoner.PrisonBlock))
            {
                return new ResultService<Prisoner>(false, "Missing Data!Try Again!");
            }
            
            if (prisoner.SentenceLenght <= 0 || prisoner.PrisonCell < 0 || prisoner.PersonalIDNumber <= 0)
            {
                return new ResultService<Prisoner>(false, "Missing Data!Try Again!");
            }
            var pidnCheck = context.Prisoners.FirstOrDefaultAsync(p => p.PersonalIDNumber == prisoner.PersonalIDNumber).Result;

            if (pidnCheck != null)
            {
                return new ResultService<Prisoner>(false, "Prisoner with this PIDN already exists!", prisoner);
            }

            if (prisoner.Age < minimumAgeForPrison)
            {
                return new ResultService<Prisoner>(false, "Prisoner's Age can't be under 18! If he is please refer to the correct facility!", prisoner);
            }
            prisoner.ReleaseDate = prisoner.EntryDate.AddYears(prisoner.SentenceLenght);
            
            try
            {
                context.Prisoners.Add(prisoner);
                await context.SaveChangesAsync();
                return new ResultService<Prisoner>(true, "Prisoner Added!", prisoner);
            }
            catch
            {       
               return new ResultService<Prisoner>(false,"There was a problem with the data being added. Try Again!", prisoner);
            }

        }

        public async Task<ResultService<Prisoner>> DeletePrisonerAsync(int pidn, string currentUserRole)
        {
            if (currentUserRole != "Admin")
            {
                return new ResultService<Prisoner>(false, "You don't have the authorization to perform this action!");
            }

            var prisoner = await context.Prisoners.FirstOrDefaultAsync(p => p.PersonalIDNumber == pidn);

            if (prisoner == null)
            {
                return new ResultService<Prisoner>(false,$"Prisoner with this PIDN does not exist!");
            }
            
            context.Prisoners.Remove(prisoner);
            await context.SaveChangesAsync();
            return new ResultService<Prisoner>(true, "Prisoner Deleted");
        }

        public async Task<ResultService<List<Prisoner>>> GetAllPrisonersAsync()
        {
            var prisoners = await context.Prisoners.ToListAsync();
            if (prisoners.IsNullOrEmpty())
            {
                return new ResultService<List<Prisoner>>(false, "No prisoners are registered!", new List<Prisoner>());
            }

            return new ResultService<List<Prisoner>>(true, "Prisoner Added!", prisoners);
        }
      
        public async Task<ResultService<Prisoner>> GetPrisonerByIDAsync(int PersonalIDNumber)
        {
            var prisoner = await context.Prisoners.FirstOrDefaultAsync(p => p.PersonalIDNumber == PersonalIDNumber);
            if (prisoner == null)
            {
                return new ResultService<Prisoner>(false, "No Prisoner with those details!");
            }
            return new ResultService<Prisoner>(true, "", prisoner);
        }

        public async Task<ResultService<Prisoner>> GetPrisonerByNameAsync(string firstName, string lastName)
        {
            var prisoner = await context.Prisoners.FirstOrDefaultAsync(n => n.FirstName.Contains(firstName) || n.LastName.Contains(lastName));
            if (prisoner == null)
            {
                return new ResultService<Prisoner>(false, "No Prisoner with those details!");
            }

            return new ResultService<Prisoner>(true, "", prisoner);
        }

        public async Task<ResultService<Prisoner>> UpdatePrisonerAsync(int pidn, int choice, string currentUserRole, string newValue)
        {
            if (currentUserRole != "Admin")
            {
                return new ResultService<Prisoner>(false, "You don't have the authorization to perform this action!");
            }
            
            var prisoner = await context.Prisoners.FirstOrDefaultAsync(p => p.PersonalIDNumber == pidn);
            if (prisoner == null)
            {
                return new ResultService<Prisoner>(false, "No Prisoner found", prisoner);
            }
            if (newValue.IsNullOrEmpty())
            {
                return new ResultService<Prisoner>(false, "Invalid Data!");
            }
            try
            {
                switch (choice)
                {
                    case 1:
                        prisoner.FirstName = newValue;
                        break;
                    case 2:
                        prisoner.Age = int.Parse(newValue);
                        break;
                    case 3:
                        prisoner.Crime = newValue;
                        break;
                    case 4:
                        prisoner.SentenceLenght = int.Parse(newValue);
                        prisoner.ReleaseDate = prisoner.EntryDate.AddYears(prisoner.SentenceLenght);
                        break;
                    case 5:
                        prisoner.PrisonBlock = newValue;
                        prisoner.PrisonCell = int.Parse(newValue);
                        break;
                    case 6:
                        prisoner.EntryDate = DateOnly.Parse(newValue);
                        prisoner.ReleaseDate = prisoner.EntryDate.AddYears(prisoner.SentenceLenght);
                        break;
                    case 7:
                        prisoner.PersonalIDNumber = int.Parse(newValue);
                        break;

                    default:
                        throw new InvalidDataException("Invalid Option");
                }
                await context.SaveChangesAsync();
                return new ResultService<Prisoner>(true, "Prisoner Updated", prisoner);
            }
            catch
            {
                return new ResultService<Prisoner>(false, "Error updating");
            }

        }
    }
}
