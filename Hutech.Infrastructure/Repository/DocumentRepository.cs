using Dapper;
using Hutech.Application.Interfaces;
using Hutech.Core.Entities;
using Hutech.Sql.Queries;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Infrastructure.Repository
{
    public class DocumentRepository:IDocumentRepository
    {
        public IConfiguration configuration;
        public DocumentRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public async Task<List<long>> UpdateDocument(List<Document> document, long instrumentId)
        {
            try
            {
                long ids = 0;
                List<long> idList = new List<long>();
                foreach (var data in document)
                {
                    using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                    {
                        connection.Open();
                        var result = await connection.QueryAsync<long>(DocumentQueries.UploadInstrumentDocument, data);
                        ids = result.First();
                        idList.Add(ids);
                        //InstrumentDocumentMapping instrumentDocument = new InstrumentDocumentMapping()
                        //{
                        //    InstrumentId = instrumentId,
                        //    DocumentId = result.First(),
                        //    IsDeleted = false,
                        //    CreatedDate = DateTime.UtcNow,
                        //    CreatedBy = data.CreatedBy
                        //};
                        //var instrumentDocumentMapping = await connection.QueryAsync<long>(InstrumentQueries.AddInstrumentDocumentMapping, instrumentDocument);
                    }
                }
                return idList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<long>> UploadDocument(List<Document> document)
        {
            try
            {
                long ids = 0;
                List<long> idList = new List<long>();
                foreach (var data in document)
                {
                    using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                    {
                        connection.Open();
                        var result = await connection.QueryAsync<long>(DocumentQueries.UploadInstrumentDocument, data);
                        ids = result.First();
                        idList.Add(ids);
                        //var instrumentId = await connection.QueryAsync<long>(InstrumentQueries.GetLastAddedInstrumentId);

                        //InstrumentDocumentMapping instrumentDocument = new InstrumentDocumentMapping()
                        //{
                        //    InstrumentId = instrumentId.First(),
                        //    DocumentId = result.First(),
                        //    IsDeleted = false,
                        //    CreatedDate = DateTime.UtcNow,
                        //    CreatedBy = data.CreatedBy
                        //};
                        //var instrumentDocumentMapping = await connection.QueryAsync<long>(InstrumentQueries.AddInstrumentDocumentMapping, instrumentDocument);
                    }
                }
                return idList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
