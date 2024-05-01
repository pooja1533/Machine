using Hutech.Core.ApiResponse;
using Hutech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Application.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<UserDetail>> GetAllUSers(string userRole,string userId);
        public Task<List<AspNetUsers>> GetUsers();
        public Task<string> DeleteUser(string userId);
        public Task<UserDetail> GetUserById(long Id);
        public Task<string> UpdateUser(AspNetUsers aspNetUsers);
        public Task<bool> ApproveUser(long Id);
        public Task<bool> RejectUser(string comment, long userId);
        Task<bool> PostUser(UserDetail userDetail);
        public Task<UserDetail> GetUserDetail(long Id);
        public Task<ExecutionResult<GridData<UserDetail>>> GetAllFilterUser(string? fullName, int pageNumber, string? userName, string? status, string? email,string? loggedInUserId,string? employeeId,int? userType,long? departmentId,long? locationId,string? roleId);
        public Task<bool> CheckEmployeeIdExist(string EmployeeId);
    }
}
