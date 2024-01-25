using Hutech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Application.Interfaces
{
    public interface IActivityDetailRepository
    {
        Task<bool> PostActivityDetail(ActivityDetails activityDetails);
        public Task<List<ActivityDetails>> GetAllActivityDetails(string userId);

        public Task<ActivityDetails> GetActivityDetails(long Id);
        public Task<String> DeleteActivityDetails(long Id);
    }
}
