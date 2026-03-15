using PMS_Horus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS_Horus.Services
{
    public interface IPrisonerServices
    {
        Task AddPrisonerAsync(Prisoner prisoner, string userCurrentRole);
        Task<Prisoner> GetPrisonerByIDAsync(int id);
        Task<Prisoner> GetPrisonerByNameAsync(string name);
        Task<List<Prisoner>> GetAllPrisonersAsync();
        Task UpdatePrisonerAsync(int id, int choice, string currentUserRole, string newValue);
        Task DeletePrisonerAsync(int id, string currentUserRole);
        Task<List<Prisoner>> SearchPrisonerAsync(string keyword);
        Task<List<Prisoner>> GetExpiringSentencesAsync();
    }
}
