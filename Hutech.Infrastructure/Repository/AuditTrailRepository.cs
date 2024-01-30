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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hutech.Infrastructure.Repository
{
    public class AuditTrailRepository : IAuditTrailRepository
    {
        private readonly IConfiguration configuration;
        public AuditTrailRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public async Task<ExecutionResult<GridData<Audit>>> GetAuditTrail(string startDate, string endDate, string keyword, int pageNumber)
        {
            try
            {
                int maxRows = 10;
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var recordsPerPage = 10;
                    var skipRecords = (pageNumber - 1) * recordsPerPage;
                    var result = await connection.QueryAsync<Audit>(AuditQueries.GetAuditTrail, new { fromDate = startDate, toDate = endDate, keyword = keyword });
                    if (pageNumber > 0)
                    {
                        var totalRecords = result.Count();


                        var auditList = result.Skip(skipRecords).Take(recordsPerPage).ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var audits = new GridData<Audit>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = auditList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<Audit>>(audits);
                    }
                    else{
                        var totalRecords = result.Count();
                        var auditList = result.ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var audits = new GridData<Audit>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = auditList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<Audit>>(audits);
                    }
                    
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
