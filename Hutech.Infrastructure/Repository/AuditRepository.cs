using Dapper;
using Hutech.Application.Interfaces;
using Hutech.Core.Entities;
using Hutech.Sql.Queries;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Infrastructure.Repository
{
    public class AuditRepository:IAuditRepository
    {
        private readonly IConfiguration _configuration;
        public AuditRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async void AddExceptionDetails(long auditId, string Message)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<AspNetRole>(AuditQueries.AddExceptionDetails, new { AuditId = auditId, Exception_Details = Message});
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      
        public long InsertAuditLogs(AuditModels objauditmodel)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(_configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var para = new DynamicParameters();
                    para.Add("@UserID", objauditmodel.UserId);
                    para.Add("@SessionID", objauditmodel.SessionId);
                    para.Add("@IPAddress", objauditmodel.IpAddress);
                    para.Add("@PageAccessed", objauditmodel.PageAccessed);
                    para.Add("@LoggedInAt", objauditmodel.LoggedInAt);
                    para.Add("@LoggedOutAt", objauditmodel.LoggedOutAt);
                    para.Add("@Request_Data", objauditmodel.Request_Data);
                    para.Add("@ModuleName", objauditmodel.ModuleName);
                    para.Add("@ActionName", objauditmodel.ActionName);
                    para.Add("@UrlReferrer", objauditmodel.UrlReferrer);
                    para.Add("@Area", objauditmodel.Area);
                    para.Add("@RoleId", objauditmodel.RoleId);
                    para.Add("@LangId", objauditmodel.LangId);
                    para.Add("@IsFirstLogin", objauditmodel.IsFirstLogin);
                    para.Add("@Exception_Details", objauditmodel.Exception_Details);
                    // @ReturnVal could be any name
                    //para.Add("@ReturnVal", SqlDbType.Int);


                    var result=connection.ExecuteScalar("Usp_InsertAuditLogs", para, null, 0, CommandType.StoredProcedure);
                    long id = System.Convert.ToInt64(result);
                    return id;
                }
                    
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
