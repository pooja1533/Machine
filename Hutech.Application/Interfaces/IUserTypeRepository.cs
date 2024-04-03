using Hutech.Core.ApiResponse;
using Hutech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Application.Interfaces
{
    public interface IUserTypeRepository
    {
        Task<bool> PostUserType(UserType userType);
        public Task<ExecutionResult<GridData<UserType>>> GetUserType(int pageNumber);
        public Task<List<UserType>> GetActiveUserType();
        public Task<String> DeleteUserType(long Id);
        public Task<UserType> GetUserTypeDetail(long Id);
    }
}
