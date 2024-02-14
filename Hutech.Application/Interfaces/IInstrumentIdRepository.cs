using Hutech.Core.ApiResponse;
using Hutech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Application.Interfaces
{
    public interface IInstrumentIdRepository
    {
        Task<bool> PostInstrumentId(InstrumentsIds instrumentId);
        public Task<ExecutionResult<GridData<InstrumentsIds>>> GetInstrumentId(int pageNumber);
        public Task<List<InstrumentsIds>> GetActiveInstrumentId();
        public Task<String> DeleteInstrumentId(long Id);
        public Task<InstrumentsIds> GetInstrumentIdDetail(long Id);
        public Task<String> PutInstrumentId(InstrumentsIds activity);
    }
}
