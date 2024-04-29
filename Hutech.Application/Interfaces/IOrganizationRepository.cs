using Hutech.Core.ApiResponse;
using Hutech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Application.Interfaces
{
    public interface IOrganizationRepository
    {
        public Task<bool> AddOrganization(Organization organization);
        public Task<ExecutionResult<GridData<Organization>>> GetOrganization(int pageNumber);
        public Task<String> DeleteOrganization(long Id);
        public Task<Organization> GetOrganizationDetail(long Id);
    }
}
