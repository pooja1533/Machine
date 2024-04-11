using Hutech.Core.ApiResponse;
using Hutech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Application.Interfaces
{
    public interface IRoleRepository
    {
        public Task<bool> AddRole(AspNetRole aspnetRole);
        public Task<ExecutionResult<GridData<AspNetRole>>> GetAllRoles(int pageNumber);
        public Task<List<AspNetRole>> GetRoleAccordingToRole(string roleName);
        public Task<AspNetRole> GetRoleDetail(Guid Id);
        public Task<String> DeleteRole(Guid Id);
        public Task<String> UpdateRole(AspNetRole aspNetRole);
        public Task<ExecutionResult<GridData<AspNetRole>>> GetAllFilterRoles(string? roleName, int pageNumber, string? updatedBy,string? updatedDate);
        public Task<List<AspNetRole>> GetAllRoles();
    }
}
