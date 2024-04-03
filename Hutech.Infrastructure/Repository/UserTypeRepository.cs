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
    public class UserTypeRepository : IUserTypeRepository
    {
        public IConfiguration configuration;
        public UserTypeRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public async Task <List<UserType>> GetActiveUserType()
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<UserType>(UserTypeQueries.GetActiveUserType);
                    return new List<UserType>(result);    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ExecutionResult<GridData<UserType>>> GetUserType(int pageNumber)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<UserType>(UserTypeQueries.GetUserType);
                    var recordsPerPage = 10;
                    var skipRecords = (pageNumber - 1) * recordsPerPage;
                    if (pageNumber > 0)
                    {
                        var totalRecords = result.Count();
                        var userTypeList = result.Skip(skipRecords).Take(recordsPerPage).ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var userTypes = new GridData<UserType>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = userTypeList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<UserType>>(userTypes);
                    }
                    else
                    {
                        var totalRecords = result.Count();
                        var userTypeList = result.ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var userTypes = new GridData<UserType>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = userTypeList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<UserType>>(userTypes);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> PostUserType(UserType userType)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    userType.DateCreatedUtc = DateTime.UtcNow;
                    if (userType.Id > 0)
                    {
                        var result = await connection.QueryAsync<string>(UserTypeQueries.PutUserType, userType);
                        return true;
                    }
                    else
                    {
                        userType.IsActive = true;
                        var result = await connection.QueryAsync<string>(UserTypeQueries.PostUserType, userType);
                        return true;

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> DeleteUserType(long Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<UserType>(UserTypeQueries.DeleteUserType, new { Id = Id });
                    return result.ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserType> GetUserTypeDetail(long Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<UserType>(UserTypeQueries.GetUserTypeDetail, new { Id = Id });
                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
