using Hutech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Application.Interfaces
{
    public interface IActivityRepository
    {
        Task<bool> PostActivity(Activity activity);
        public Task<List<Activity>> GetActivity();
        public Task<Activity> GetActivityDetail(long Id);
        public Task<String> DeleteActivity(long Id);
        public Task<String> PutActivity(Activity activity);
    }
}
