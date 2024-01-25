using Dapper;
using Hutech.Application.Interfaces;
using Hutech.Core.Entities;
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
        public void InsertAuditLogs(AuditModels objauditmodel)
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
                    para.Add("@Message", objauditmodel.Message);
                    para.Add("@ControllerName", objauditmodel.ControllerName);
                    para.Add("@ActionName", objauditmodel.ActionName);
                    para.Add("@UrlReferrer", objauditmodel.UrlReferrer);
                    para.Add("@Area", objauditmodel.Area);
                    para.Add("@RoleId", objauditmodel.RoleId);
                    para.Add("@LangId", objauditmodel.LangId);
                    para.Add("@IsFirstLogin", objauditmodel.IsFirstLogin);
                    connection.Execute("Usp_InsertAuditLogs", para, null, 0, CommandType.StoredProcedure);
                }
                    
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
