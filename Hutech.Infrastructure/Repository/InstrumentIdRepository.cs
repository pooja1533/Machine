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
        public async Task<string> PutInstrumentId(InstrumentsIds activity)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.ExecuteAsync(InstrumentIdQueries.UpdateInstrumentId, activity);
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
