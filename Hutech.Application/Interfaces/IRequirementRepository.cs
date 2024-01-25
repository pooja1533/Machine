using Hutech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Application.Interfaces
{
    public interface IRequirementRepository
    {
        Task<bool> PostRequirement(Requirement requirement);
        public Task<List<Requirement>> GetRequirement();
        public Task<List<Requirement>> GetActiveRequirement();
        public Task<Requirement> GetRequirementDetail(long Id);
        public Task<String> DeleteRequirement(long Id);
        public Task<String> PutRequirement(Requirement requirement);
    }
}
