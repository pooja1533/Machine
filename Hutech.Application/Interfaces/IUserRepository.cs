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
        public Task<List<AspNetUsers>> GetAllUSers(string userRole,string userId);
        public Task<string> DeleteUser(string userId);
        public Task<AspNetUsers> GetUserById(string Id);
        public Task<string> UpdateUser(AspNetUsers aspNetUsers);
    }
}
