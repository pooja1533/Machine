using Hutech.Core.ApiResponse;
using Hutech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Application.Interfaces
{
    public interface IInstrumentActivityRepository
    {
        Task<bool> PostInstrumentActivity(InstrumentActivity activity);
        
        public Task<ExecutionResult<GridData<InstrumentActivity>>> GetInstrumentActivity(int pageNumber);
        public Task<List<InstrumentActivity>> GetActiveInstrumentActivity();
        public Task<String> DeleteInstrumentActivity(long Id);
        public Task<InstrumentActivity> GetInstrumentActivityDetail(long Id);
        public Task<InstrumentActivity> GetInstrumentActivityDetailData(long Id);
        public Task<String> PutInstrumentActivity(InstrumentActivity activity);
        public Task<DateTime> GetLastPerformedDateForInstrumentActivity(long instrumentActivityId);
    }
}
