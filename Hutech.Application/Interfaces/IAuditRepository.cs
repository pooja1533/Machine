﻿using Hutech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Application.Interfaces
{
    public interface IAuditRepository
    {
        long InsertAuditLogs(AuditModels objauditmodel);
        void AddExceptionDetails(long auditId, string Message);
    }
}
