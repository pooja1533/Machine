using Dapper;
using Hutech.Application.Interfaces;
using Hutech.Core.Constants;
using Hutech.Core.Entities;
using Hutech.Sql.Queries;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration configuration;
        public UserRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public async Task<string> DeleteUser(string userId)
        {
            using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
            {
                connection.Open();
                var deleteUserId = userId.Split('=');
                var result = await connection.QueryAsync<UserDetail>(UserQueries.DeleteUser, new { userId = deleteUserId[1] });
                return result.ToString();
            }
        }
        public async Task<List<AspNetUsers>> GetUsers()
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    List<AspNetUsers> users = new List<AspNetUsers>();
                    connection.Open();
                    var result = await connection.QueryAsync<AspNetUsers>(UserQueries.GetUsers);
                    users = result.ToList();
                    return users;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<AspNetUsers>> GetAllUSers(string userRole, string userId)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    List<AspNetUsers> users = new List<AspNetUsers>();
                    connection.Open();
                    string RoleId = await connection.QuerySingleAsync<string>(UserQueries.GetRoleId, new { Role = userRole });
                    //if(UserRole.SUPERADMIN==userRole)
                    //{
                    //    var result = await connection.QueryAsync<AspNetUsers>(UserQueries.GetAllUsers, new { Id=userId});
                    //    users = result.ToList();
                    //}
                    //else if (UserRole.Admin == userRole)
                    //{
                    //    var result = await connection.QueryAsync<AspNetUsers>(UserQueries.GetAllUsersForAdmin, new { Id = userId });
                    //    result=result.Except(result.Where(x=>x.RoleName==UserRole.SUPERADMIN));
                    //    users = result.ToList();
                    //}
                    //else if (UserRole.Manager == userRole)
                    //{
                    //    var result = await connection.QueryAsync<AspNetUsers>(UserQueries.GetAllUsersForAdmin, new { Id = userId });
                    //    result = result.Except(result.Where(x => x.RoleName == UserRole.SUPERADMIN || x.RoleName==UserRole.Admin));
                    //    users = result.ToList();
                    //}
                    var result = await connection.QueryAsync<AspNetUsers>(UserQueries.GetAllUsers, new { Id = userId, RoleId = RoleId });
                    users = result.ToList();
                    return users;
                }
            
            }
            catch(Exception ex) {
                throw ex;
            }
        }

        public async Task<AspNetUsers> GetUserById(string Id)
        {
           using(IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
            {
                connection.Open();
                var editUserId = Id.Split('=');
                var result = await connection.QueryAsync<AspNetUsers>(UserQueries.GetUserById, new { Id= editUserId[1] });
                return result.First();
            }
        }

        public async Task<string> UpdateUser(AspNetUsers aspNetUsers)
        {
            using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<AspNetUsers>(UserQueries.UpdateUser, new { Id = aspNetUsers.Id, UserName = aspNetUsers.UserName, NormalizedUserName = aspNetUsers.UserName.ToUpper()});
                var userdetail = await connection.QueryAsync<AspNetUsers>(UserQueries.UpdateUserDetail, new { Id = aspNetUsers.Id, FirstName = aspNetUsers.FirstName, LastName = aspNetUsers.LastName, PhoneNumber = aspNetUsers.PhoneNumber, Address = aspNetUsers.Address });
                var deleteExistingRole = await connection.QueryAsync<AspNetRole>(UserQueries.DeleteRoleOfUser, new { Id = aspNetUsers.Id });
                var role = await connection.QueryAsync<AspNetRole>(UserQueries.AddUserRole, new { UserId = aspNetUsers.Id, RoleId = aspNetUsers.RoleId });
                return result.ToString();;
            }
        }
    }
}
