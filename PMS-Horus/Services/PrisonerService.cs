using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PMS_Horus.Data;
using PMS_Horus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS_Horus.Services
{
    internal class PrisonerService : IPrisonerServices
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

            if (prisoner.Age < 0 && prisoner.Crime.IsNullOrEmpty())
            {
                throw new InvalidDataException("Invalid Data");
            }

            prisoner.ReleaseDate = prisoner.EntryDate.AddYears(prisoner.SentenceLenght);

            context.Prisoners.Add(prisoner);
            await context.SaveChangesAsync();
        }

        public async Task DeletePrisonerAsync(Prisoner prisoner, string currentUserRole)
        {
            if (currentUserRole != "Admin")
            {
                throw new UnauthorizedAccessException("You don't have the authorization to perform this action!");
            }

            context.Prisoners.Remove(prisoner);
            await context.SaveChangesAsync();
        }

        public async Task<List<Prisoner>> GetAllPrisonersAsync()
        {
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
            return await context.Prisoners.FirstOrDefaultAsync(n => n.Name == name);
        }

        public Task<List<Prisoner>> SearchPrisonerAsync(string keyword)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePrisonerAsync(Prisoner prisoner, string currentUserRole)
        {
            throw new NotImplementedException();
        }
    }
}
