using Hutech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Application.Interfaces
{
    public interface IInstrumentRepository
    {
        Task<bool> PostInstrument(Instrument instrument);
        Task<bool> AddInstrumentDocumentMapping(InstrumentDocumentMapping instrumentDocumentMapping);
        public Task<List<Instrument>> GetInstrument();
        public Task<List<Instrument>> GetActiveInstrument();
        public Task<Instrument> GetInstrumentDetail(long Id);
        public Task<String> DeleteInstrument(long Id);
        public Task<string> DeleteDocument(long documentId,long instrumentId);
        public Task<string> DeleteExistingInstrumentDocument(long Id);
        public Task<String> PutInstrument(Instrument instrument);
        Task<long> GetLastInsertedInstrumentId();
        //Task<bool> UploadInstrumentDocument(List<Document> document);
        //Task<bool> UpdateInstrumentDocument(List<Document> documents, long instrumentId);
    }
}
