using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PMS_Horus.Data;
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

            context.Prisoners.Add(prisoner);
            await context.SaveChangesAsync();
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

        public Task<List<Prisoner>> GetExpiringSentencesAsync()
        {
           throw new NotImplementedException(); 
        }

        public async Task<Prisoner> GetPrisonerByIDAsync(int id)
        {
            return await context.Prisoners.FindAsync(id);
        }

        public async Task<Prisoner> GetPrisonerByNameAsync(string name)
        {
            return await context.Prisoners.FirstOrDefaultAsync(n => n.FirstName.Contains(name) || n.LastName.Contains(name));
        }

        public async Task<List<Prisoner>> SearchPrisonerAsync(string keyword)
        {           
            string toLowerKeyword = keyword.ToLower();
            List<Prisoner> results = new List<Prisoner>();
            return results = context.Prisoners
                .Where(p => p.FirstName.ToLower().Contains(toLowerKeyword) ||
                            p.Crime.ToLower().Contains(toLowerKeyword))
                .ToList();
        }

        public async Task UpdatePrisonerAsync(int id, int choice, string currentUserRole, string newValue)
        {
            if (currentUserRole != "Admin")
            {
                throw new UnauthorizedAccessException("You don't have the authorization to perform this action!");
            }
            if (id < 0)
            {
                throw new InvalidDataException("Invalid ID! Try Again!");
            }

            var prisoner = await context.Prisoners.FindAsync(id);

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
