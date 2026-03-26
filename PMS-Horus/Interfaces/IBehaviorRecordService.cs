using PMS_Horus.Models;
using PMS_Horus.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS_Horus.Interfaces
{
    public  interface IBehaviorRecordService
    {
        Task<ResultService<BehaviorRecord>> AddBehaviorRecordAsync(BehaviorRecord behaviorRecord, string currentUserRole);
        Task<ResultService<BehaviorRecord>> RemoveBehaviorRecordAsync(int pidn, string currentUserRole);
        Task<ResultService<BehaviorRecord>> UpdateBehaviorRecordAsync(int prisonerId, int choice, string currentUserRole, string newValue);
        Task<ResultService<List<BehaviorRecord>>> GetBehaviorRecordAsync(int prisonerId, string currentUserRole);
    }
}
