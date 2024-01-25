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
        public Task<List<Instrument>> GetInstrument();
        public Task<List<Instrument>> GetActiveInstrument();
        public Task<Instrument> GetInstrumentDetail(long Id);
        public Task<String> DeleteInstrument(long Id);
        public Task<String> PutInstrument(Instrument instrument);
    }
}
