using Hutech.Core.ApiResponse;
using Hutech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Application.Interfaces
{
    public interface IMenuRepository
    {
        public  Task<List<Menu>> GetLoggedInUserMenuPermission(Guid loggedInUserId);

    }
}
