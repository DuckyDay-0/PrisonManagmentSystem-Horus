using PMS_Horus.Models;
using PMS_Horus.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS_Horus.Interfaces
{
    public interface IPrisonerServices
    {
        Task<ResultService<Prisoner>> AddPrisonerAsync(Prisoner prisoner, string userCurrentRole);
        Task<ResultService<Prisoner>> GetPrisonerByIDAsync(int id);
        Task<ResultService<Prisoner>> GetPrisonerByNameAsync(string firstName, string lastName);
        Task<ResultService<List<Prisoner>>> GetAllPrisonersAsync();
        Task<ResultService<Prisoner>> UpdatePrisonerAsync(int id, int choice, string currentUserRole, string newValue);
        Task<ResultService<Prisoner>> DeletePrisonerAsync(int id, string currentUserRole);
    }
}
