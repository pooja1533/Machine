using Hutech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Application.Interfaces
{
    public interface IDocumentRepository
    {
        Task<List<long>> UploadDocument(List<Document> document);
        Task<List<long>> UpdateDocument(List<Document> documents, long instrumentId);
    }
}
