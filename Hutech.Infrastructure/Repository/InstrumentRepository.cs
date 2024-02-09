using Dapper;
using Hutech.Application.Interfaces;
using Hutech.Core.Entities;
using Hutech.Sql.Queries;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Infrastructure.Repository
{
    public class InstrumentRepository : IInstrumentRepository
    {
        public IConfiguration configuration;
        public InstrumentRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public async Task<string>  DeleteExistingInstrumentDocument(long Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<InstrumentDocumentMapping>(InstrumentQueries.DeleteExistingInstrumentDocument, new { Id = Id });
                    return result.ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> DeleteDocument(long documentId,long instrumentId)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<InstrumentDocumentMapping>(InstrumentQueries.DeleteInstrumentDocument, new { DocumentId = documentId,InstrumentId= instrumentId });
                    var data = await connection.QueryAsync<Document>(DocumentQueries.DeleteDocument, new {Id=documentId });
                    return result.ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> DeleteInstrument(long Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Instrument>(InstrumentQueries.DeleteInstrument, new { Id = Id });
                    return result.ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Instrument>> GetInstrument()
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Instrument>(InstrumentQueries.GetInstrument);
                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Instrument>> GetActiveInstrument()
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Instrument>(InstrumentQueries.GetActiveInstrument);
                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Instrument> GetInstrumentDetail(long Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Instrument>(InstrumentQueries.GetInstrumentDetail, new { Id = Id });
                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public async Task<bool> UpdateInstrumentDocument(List<Document> document,long instrumentId)
        //{
        //    try
        //    {
        //        foreach (var data in document)
        //        {
        //            using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
        //            {
        //                connection.Open();
        //                var result = await connection.QueryAsync<long>(InstrumentQueries.UploadInstrumentDocument, data);
        //                InstrumentDocumentMapping instrumentDocument = new InstrumentDocumentMapping()
        //                {
        //                    InstrumentId = instrumentId,
        //                    DocumentId = result.First(),
        //                    IsDeleted = false,
        //                    CreatedDate = DateTime.UtcNow,
        //                    CreatedBy = data.CreatedBy
        //                };
        //                var instrumentDocumentMapping = await connection.QueryAsync<long>(InstrumentQueries.AddInstrumentDocumentMapping, instrumentDocument);
        //            }
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public async Task<bool> UploadInstrumentDocument(List<Document> document)
        //{
        //    try
        //    {
        //        foreach(var data in document)
        //        {
        //            using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
        //            {
        //                connection.Open();
        //                var result = await connection.QueryAsync<long>(InstrumentQueries.UploadInstrumentDocument, data);
        //                var instrumentId = await connection.QueryAsync<long>(InstrumentQueries.GetLastAddedInstrumentId);

        //                InstrumentDocumentMapping instrumentDocument = new InstrumentDocumentMapping()
        //                {
        //                    InstrumentId = instrumentId.First(),
        //                    DocumentId = result.First(),
        //                    IsDeleted = false,
        //                    CreatedDate = DateTime.UtcNow,
        //                    CreatedBy = data.CreatedBy
        //                };
        //                var instrumentDocumentMapping = await connection.QueryAsync<long>(InstrumentQueries.AddInstrumentDocumentMapping, instrumentDocument);
        //            }
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public async Task<bool> PostInstrument(Instrument instrument)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    instrument.IsActive = true;
                    var result = await connection.QueryAsync<string>(InstrumentQueries.PostInstrument, instrument);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> PutInstrument(Instrument instrument)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Instrument>(InstrumentQueries.UpdateInstrument, new { Id = instrument.Id, Name = instrument.Name, IsActive = instrument.IsActive });
                    return result.ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> AddInstrumentDocumentMapping(InstrumentDocumentMapping instrumentDocumentMapping)
        {
            try
            {
                bool result = false;
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var instrumentDocumentMappingresult = await connection.QueryAsync<string>(InstrumentQueries.AddInstrumentDocumentMapping, instrumentDocumentMapping);
                    if(instrumentDocumentMappingresult != null)
                    {
                        result= true;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<long> GetLastInsertedInstrumentId()
        {
            try
            {
                long lastinstrumentId = 0;
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var instrumentDocumentMappingresult = await connection.QueryAsync<long>(InstrumentQueries.GetLastAddedInstrumentId);
                    lastinstrumentId = instrumentDocumentMappingresult.First();
                }
                return lastinstrumentId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
