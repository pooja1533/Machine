using Hutech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Application.Interfaces
{
    public interface IAuditTrailRepository
    {
        public Task<List<Audit>> GetAuditTrail(string startDate,string endDate, string keyword);

    }
}
