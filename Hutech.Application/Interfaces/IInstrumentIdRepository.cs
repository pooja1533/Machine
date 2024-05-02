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
        public Task<ExecutionResult<GridData<InstrumentsIds>>> GetAllFilterInstrumentId(string? instrumentIdName,string? model,string? instrumentName,string? instrumentSerial,string? instrumentLocation,string? teamName, int pageNumber, string? updatedBy, string? status, string? updatedDate);
        Task<bool> AddInstrumentIdDocumentMapping(InstrumentIdDocumentMapping instrumentIdDocumentMapping);
        Task<long> GetLastInsertedInstrumentId();
        Task<bool> AddInstrumentDocumentMapping(InstrumentIdDocumentMapping instrumentDocumentMapping);
        public Task<string> DeleteDocument(long documentId, long instrumentId);
    }
}
