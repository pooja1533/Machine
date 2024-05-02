using Dapper;
using Hutech.Application.Interfaces;
using Hutech.Core.ApiResponse;
using Hutech.Core.Entities;
using Hutech.Sql.Queries;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Infrastructure.Repository
{
    public class InstrumentIdRepository : IInstrumentIdRepository
    {
        public IConfiguration configuration;
        public InstrumentIdRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public async Task<List<InstrumentsIds>> GetActiveInstrumentId()
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<InstrumentsIds>(InstrumentIdQueries.GetActiveInstrumentId);
                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ExecutionResult<GridData<InstrumentsIds>>> GetInstrumentId(int pageNumber = 1)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<InstrumentsIds>(InstrumentIdQueries.GetInstrumentId);
                    var recordsPerPage = 10;
                    var skipRecords = (pageNumber - 1) * recordsPerPage;
                    if (pageNumber > 0)
                    {
                        var totalRecords = result.Count();
                        var instrumentIdList = result.Skip(skipRecords).Take(recordsPerPage).ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var instrumentIds = new GridData<InstrumentsIds>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = instrumentIdList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<InstrumentsIds>>(instrumentIds);
                    }
                    else
                    {
                        var totalRecords = result.Count();
                        var instrumentIdList = result.ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var instrumentIds = new GridData<InstrumentsIds>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = instrumentIdList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<InstrumentsIds>>(instrumentIds);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> PostInstrumentId(InstrumentsIds instrumentId)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    instrumentId.IsActive = true;
                    instrumentId.IsDeleted = false;
                    var result = await connection.ExecuteAsync(InstrumentIdQueries.PostInstrumentId, instrumentId);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> DeleteInstrumentId(long Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<InstrumentsIds>(InstrumentIdQueries.DeleteInstrumentId, new { Id = Id });
                    return result.ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<InstrumentsIds> GetInstrumentIdDetail(long Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<InstrumentsIds>(InstrumentIdQueries.GetInstrumentIdDetail, new { Id = Id });
                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> PutInstrumentId(InstrumentsIds instrumentsIds)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.ExecuteAsync(InstrumentIdQueries.UpdateInstrumentId, instrumentsIds);
                    return result.ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ExecutionResult<GridData<InstrumentsIds>>> GetAllFilterInstrumentId(string? instrumentIdName, string? model, string? instrumentName, string? instrumentSerial, string? instrumentLocation, string? teamName, int pageNumber, string? updatedBy, string? status, string? updatedDate)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    updatedBy = !string.IsNullOrEmpty(updatedBy) ? updatedBy = "%" + updatedBy + "%" : updatedBy;
                    bool isactive = false;
                    if (string.IsNullOrEmpty(status) || status == "1")
                        isactive = true;
                    instrumentName = !string.IsNullOrEmpty(instrumentName) ? instrumentName +"%": null;
                    instrumentLocation = !string.IsNullOrEmpty(instrumentLocation) ? instrumentLocation+"%" : null;
                    teamName = !string.IsNullOrEmpty(teamName) ? teamName+"%" : null;
                    instrumentSerial = !string.IsNullOrEmpty(instrumentSerial) ? instrumentSerial = instrumentSerial + "%" : instrumentSerial;
                    model = !string.IsNullOrEmpty(model) ? model = model + "%" : model;
                    instrumentIdName = !string.IsNullOrEmpty(instrumentIdName) ? instrumentIdName = instrumentIdName + "%" : instrumentIdName;
                    var result = await connection.QueryAsync<InstrumentsIds>(InstrumentIdQueries.GetAllFilterInstrumentId, new { Name = instrumentIdName,Model=model,InstrumentName=instrumentName,InstrumentSerial=instrumentSerial,InstrumentLocation=instrumentLocation,TeamName=teamName, UpdatedBy = updatedBy, Status = isactive, UpdatedDate = updatedDate });
                    var recordsPerPage = 10;
                    var skipRecords = (pageNumber - 1) * recordsPerPage;
                    if (pageNumber > 0)
                    {
                        var totalRecords = result.Count();
                        var instrumentIdList = result.Skip(skipRecords).Take(recordsPerPage).ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var instrumentIds = new GridData<InstrumentsIds>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = instrumentIdList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<InstrumentsIds>>(instrumentIds);
                    }
                    else
                    {
                        var totalRecords = result.Count();
                        var instrumentIdList = result.ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var instrumentIds = new GridData<InstrumentsIds>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = instrumentIdList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<InstrumentsIds>>(instrumentIds);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> AddInstrumentIdDocumentMapping(InstrumentIdDocumentMapping instrumentIdDocumentMapping)
        {
            try
            {
                bool result = false;
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var instrumentIdDocumentMappingresult = await connection.QueryAsync<string>(InstrumentIdQueries.AddInstrumentIdDocumentMapping, instrumentIdDocumentMapping);
                    if (instrumentIdDocumentMappingresult != null)
                    {
                        result = true;
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
                    var instrumentDocumentMappingresult = await connection.QueryAsync<long>(InstrumentIdQueries.GetLastInsertedInstrumentId);
                    lastinstrumentId = instrumentDocumentMappingresult.First();
                }
                return lastinstrumentId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> AddInstrumentDocumentMapping(InstrumentIdDocumentMapping instrumentIdDocumentMapping)
        {
            try
            {
                bool result = false;
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var instrumentIdDocumentMappingresult = await connection.QueryAsync<string>(InstrumentIdQueries.AddInstrumentDocumentMapping, instrumentIdDocumentMapping);
                    if (instrumentIdDocumentMappingresult != null)
                    {
                        result = true;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> DeleteDocument(long documentId, long instrumentId)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<InstrumentIdDocumentMapping>(InstrumentIdQueries.DeleteInstrumentIdDocument, new { DocumentId = documentId, InstrumentId = instrumentId });
                    var data = await connection.QueryAsync<Document>(DocumentQueries.DeleteDocument, new { Id = documentId });
                    return result.ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
