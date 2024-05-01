﻿using Dapper;
using Hutech.Application.Interfaces;
using Hutech.Core.ApiResponse;
using Hutech.Core.Constants;
using Hutech.Core.Entities;
using Hutech.Sql.Queries;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
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
        public async Task<bool> CheckEmployeeIdExist(string employeeId)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    bool result = await connection.QuerySingleOrDefaultAsync<bool>(UserQueries.CheckEmployeeIdExist, new { EmployeeId = employeeId });
                    connection.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> DeleteUser(string userId)
        {
            using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
            {
                connection.Open();
                long deleteUserId = System.Convert.ToInt64(userId);
                var result = await connection.QueryAsync<UserDetail>(UserQueries.DeleteUser, new { userId = deleteUserId });
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
        public async Task<ExecutionResult<GridData<UserDetail>>> GetAllFilterUser(string? fullName, int pageNumber, string? userName, string? status, string? email,string? loggedInUserId,string? employeeId, int? userType, long? departmentId,long? locationId, string? roleId)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    fullName = !string.IsNullOrEmpty(fullName) ? fullName = "%" + fullName + "%" : fullName;
                    bool isactive = false;
                    if (string.IsNullOrEmpty(status) || status == "1")
                        isactive = true;
                    userName = !string.IsNullOrEmpty(userName) ? userName = userName + "%" : userName;
                    employeeId = !string.IsNullOrEmpty(employeeId) ? employeeId = employeeId + "%" : employeeId;
                    email=!string.IsNullOrEmpty(email) ? email=email + "%" : email;
                    roleId = !string.IsNullOrEmpty(roleId) ? roleId = roleId : roleId;
                    var result = await connection.QueryAsync<UserDetail>(UserQueries.GetAllFilterUser, new { FullName = fullName, userName = userName, Status = isactive, email = email ,Id=loggedInUserId,EmployeeId= employeeId,UserTypeId=userType,DepartmentId=departmentId,LocationId=locationId,RoleId= roleId });
                    var recordsPerPage = 10;
                    var skipRecords = (pageNumber - 1) * recordsPerPage;
                    if (pageNumber > 0)
                    {
                        var totalRecords = result.Count();
                        var userList = result.Skip(skipRecords).Take(recordsPerPage).ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var users = new GridData<UserDetail>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = userList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<UserDetail>>(users);
                    }
                    else
                    {
                        var totalRecords = result.Count();
                        var userList = result.ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var users = new GridData<UserDetail>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = userList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<UserDetail>>(users);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<UserDetail>> GetAllUSers(string userRole, string userId)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    List<UserDetail> users = new List<UserDetail>();
                    connection.Open();
                    string RoleId = await connection.QuerySingleAsync<string>(UserQueries.GetRoleId, new { Role = userRole });
                    var result = await connection.QueryAsync<UserDetail>(UserQueries.GetAllUsers, new { Id = userId, RoleId = RoleId });
                    users = result.ToList();
                    return users;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserDetail> GetUserById(long Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<UserDetail>(UserQueries.GetUserById, new { Id = Id });
                    return result.First();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<UserDetail> GetUserDetail(long Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<UserDetail>(UserQueries.GetUserDetail, new { Id = Id });
                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> UpdateUser(AspNetUsers aspNetUsers)
        {
            using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<AspNetUsers>(UserQueries.UpdateUser, new { Id = aspNetUsers.Id, UserName = aspNetUsers.UserName, NormalizedUserName = aspNetUsers.UserName.ToUpper() });
                //var userdetail = await connection.QueryAsync<AspNetUsers>(UserQueries.UpdateUserDetail, new { Id = aspNetUsers.Id, FirstName = aspNetUsers.FirstName, LastName = aspNetUsers.LastName, PhoneNumber = aspNetUsers.PhoneNumber, Address = aspNetUsers.Address });
                var deleteExistingRole = await connection.QueryAsync<AspNetRole>(UserQueries.DeleteRoleOfUser, new { Id = aspNetUsers.Id });
                //var role = await connection.QueryAsync<AspNetRole>(UserQueries.AddUserRole, new { UserId = aspNetUsers.Id, RoleId = aspNetUsers.RoleId });
                return result.ToString(); ;
            }
        }
        public async Task<bool> PostUser(UserDetail userDetail)
        {
            try
            {
                bool success = false;
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    if (userDetail.UserId > 0)
                    {
                        userDetail.DateModifiedUtc = DateTime.UtcNow;
                        var result = await connection.QueryAsync<string>(UserQueries.PutUser, userDetail);
                        success = true;
                        var deleteExistingRole = await connection.QueryAsync<string>(UserQueries.DeleteExistingRoleOfUser, new { UserId=userDetail.AspNetUserId });
                        if (success)
                        {
                            foreach (var data in userDetail.SelectedUserRoleId)
                            {
                                string Id = data;
                                var role = await connection.QueryAsync<string>(UserQueries.AddUserRole, new { UserId = userDetail.AspNetUserId, RoleId = Id });
                            }
                        }
                        return success;

                    }
                    else
                    {
                        userDetail.IsActive = true;
                        userDetail.CreatedDate = DateTime.UtcNow;
                        int userStatusId = await connection.QuerySingleAsync<int>(UserQueries.GetUserDefualtStatus);
                        userDetail.UserstatusId = userStatusId;
                        var result = await connection.QueryAsync<string>(UserQueries.PostUser, userDetail);
                        success = true;
                        if (success)
                        {
                            foreach (var data in userDetail.SelectedUserRoleId)
                            {
                                string Id = data;
                                var role = await connection.QueryAsync<string>(UserQueries.AddUserRole, new { UserId = userDetail.AspNetUserId, RoleId = Id });
                            }
                        }
                        return success;

                    }



                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<bool> RejectUser(string comment, long userId)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.ExecuteAsync(UserQueries.RejectUser, new { Id = userId, Comment = comment });
                    if (result == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<bool> ApproveUser(long UserId)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    bool IsApprove = true;
                    var result = await connection.ExecuteAsync(UserQueries.ApproveUser, new { Id = UserId });
                    if (result == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
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
