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
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Infrastructure.Repository
{
    public class AuditTrailRepository : IAuditTrailRepository
    {
        private readonly IConfiguration configuration;
        public AuditTrailRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public async Task<List<Audit>> GetAuditTrail(string startDate, string endDate, string keyword)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Audit>(AuditQueries.GetAuditTrail, new { fromDate=startDate,toDate=endDate,keyword=keyword });
                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
