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
    }
}
