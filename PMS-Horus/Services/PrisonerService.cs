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

        public async Task AddPrisonerAsync(Prisoner prisoner, string currentUserRole)
        {
            if (currentUserRole != "Admin")
            {
                throw new UnauthorizedAccessException("You are not authorized to perform this action!");
            }

            prisoner.ReleaseDate = prisoner.EntryDate.AddYears(prisoner.SentenceLenght);

            try
            {
                context.Prisoners.Add(prisoner);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                
                throw new InvalidDataException("There was a problem with the data being added. Try Again!");
            }
        }

        public async Task DeletePrisonerAsync(int id, string currentUserRole)
        {
            if (currentUserRole != "Admin")
            {
                throw new UnauthorizedAccessException("You don't have the authorization to perform this action!");
            }

            var prisoner = await context.Prisoners.FindAsync(id);

            if (prisoner == null)
            {
                Console.WriteLine($"Prisoner with ID {id} Not Found!");
                return;
            }
            
            context.Prisoners.Remove(prisoner);
            await context.SaveChangesAsync();
        }

        public async Task<List<Prisoner>> GetAllPrisonersAsync()
        {
            if (context.Prisoners.IsNullOrEmpty())
            {
                throw new Exception("No prisoners are registered!");
            }
            return await context.Prisoners.ToListAsync();
        }
      
        public async Task<Prisoner> GetPrisonerByIDAsync(int PersonalIDNumber)
        {
            var prisoner = await context.Prisoners.FirstOrDefaultAsync(p => p.PersonalIDNumber == PersonalIDNumber);
            if (prisoner == null)
            {
                throw new NullReferenceException("No Prisoner with those details!");
            }
            return prisoner;
        }

        public async Task<Prisoner> GetPrisonerByNameAsync(string firstName, string lastName)
        {
            var prisoner = await context.Prisoners.FirstOrDefaultAsync(n => n.FirstName.Contains(firstName) || n.LastName.Contains(lastName));
            if (prisoner == null)
            {
                throw new NullReferenceException("No Prisoner with those details!");
            }
            
            return prisoner;
        }

        public async Task UpdatePrisonerAsync(int pidn, int choice, string currentUserRole, string newValue)
        {
            if (currentUserRole != "Admin")
            {
                throw new UnauthorizedAccessException("You don't have the authorization to perform this action!");
            }
            if (pidn < 0)
            {
                throw new InvalidDataException("Invalid ID! Try Again!");
            }

            var prisoner = await context.Prisoners.FirstOrDefaultAsync(p => p.PersonalIDNumber == pidn);

            if (newValue.IsNullOrEmpty())
            {
                throw new InvalidDataException("Invalid Data!");
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

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            await context.SaveChangesAsync();
        }
    }
}
