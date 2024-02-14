﻿using Hutech.Core.ApiResponse;
using Hutech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Application.Interfaces
{
    public interface IActivityDetailRepository
    {
        Task<bool> PostActivityDetail(ActivityDetails activityDetails);
        public Task<ExecutionResult<GridData<ActivityDetails>>> GetAllActivityDetails(string userId,int pageNumber);

        public Task<ActivityDetails> GetActivityDetails(long Id);
        public Task<String> DeleteActivityDetails(long Id);
        Task<bool> AddActivityDetailDocumentMapping(ActivityDetailDocumentMapping instrumentDocumentMapping);
        Task<long> GetLastInsertedActivityDetailId();
        public Task<string> DeleteDocument(long documentId, long activityDetailId);
    }
}
