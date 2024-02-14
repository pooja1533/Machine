using Hutech.Core.ApiResponse;
using Hutech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Application.Interfaces
{
    public interface IAuditTrailRepository
    {
        public Task<ExecutionResult<GridData<Audit>>> GetAuditTrail(string startDate,string endDate, string keyword,int pageNumber);
        public Task<ExecutionResult<GridData<Audit>>> GetUserAuditTrail(string startDate, string endDate, string keyword, int pageNumber, string userId);
    }
}
