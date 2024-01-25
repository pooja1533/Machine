using Hutech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Group = Hutech.Core.Entities.Group;

namespace Hutech.Application.Interfaces
{
    public interface IGroupRepository
    {
        Task<bool> PostGroup(Group group);
        public Task<List<Group>> GetGroup();
        public Task<List<Group>> GetAllActiveGroup();
        public Task<Group> GetGroupDetail(long Id);
        public Task<String> DeleteGroup(long Id);
        public Task<String> PutGroup(Group group);
    }
}
