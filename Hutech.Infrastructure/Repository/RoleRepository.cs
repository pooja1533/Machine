using Hutech.Application.Interfaces;
using Hutech.Core.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Hutech.Sql.Queries;

namespace Hutech.Infrastructure.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IConfiguration configuration;
        public RoleRepository(IConfiguration _configuration)
        {
            this.configuration = _configuration;
        }
        public async Task<bool> AddRole(AspNetRole aspnetRole)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    aspnetRole.NormalizedName = aspnetRole.Name.ToUpper();
                    aspnetRole.ConcurrencyStamp = null;
                    var result = await connection.QueryAsync<string>(RoleQueries.AddRole, aspnetRole);
                    // var result =  connection.Query(RoleQueries.AddRole,aspnetRole);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> DeleteRole(Guid Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<AspNetRole>(RoleQueries.DeleteRole, new { Id = Id });
                    return result.ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<AspNetRole>> GetRoleAccordingToRole(string role)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    string roleId = await connection.QuerySingleAsync<string>(UserQueries.GetRoleId, new { Role = role });
                    var result = await connection.QueryAsync<AspNetRole>(UserQueries.GetRoleAccordingToRole, new { roleId=roleId});
                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<AspNetRole>> GetAllRoles()
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<AspNetRole>(RoleQueries.GetAllRoles);
                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AspNetRole> GetRoleDetail(Guid Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<AspNetRole>(RoleQueries.GetRoleDetail, new { Id = Id });
                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> UpdateRole(AspNetRole aspNetRole)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<AspNetRole>(RoleQueries.UpdateRole, new { Id = aspNetRole.Id, Name = aspNetRole.Name, NormalizedName = aspNetRole.Name.ToUpper() });
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
