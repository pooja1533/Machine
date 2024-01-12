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
        public Task<List<AspNetRole>> GetAllRoles();
        public Task<List<AspNetRole>> GetRoleAccordingToRole(string roleName);
        public Task<AspNetRole> GetRoleDetail(Guid Id);
        public Task<String> DeleteRole(Guid Id);
        public Task<String> UpdateRole(AspNetRole aspNetRole);
    }
}
