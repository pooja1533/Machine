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
using Hutech.Core.ApiResponse;
using Microsoft.AspNet.Identity;

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
                    var result = await connection.QueryAsync<AspNetRole>(UserQueries.GetRoleAccordingToRole, new { roleId = roleId });
                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ExecutionResult<GridData<AspNetRole>>> GetAllRoles(int pageNumber)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<AspNetRole>(RoleQueries.GetAllRoles);
                    var recordsPerPage = 10;
                    var skipRecords = (pageNumber - 1) * recordsPerPage;
                    if (pageNumber > 0)
                    {
                        var totalRecords = result.Count();
                        var rolelist = result.Skip(skipRecords).Take(recordsPerPage).ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var roles = new GridData<AspNetRole>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = rolelist,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<AspNetRole>>(roles);
                    }
                    else
                    {
                        var totalRecords = result.Count();
                        var rolelist = result.ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var roles = new GridData<AspNetRole>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = rolelist,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<AspNetRole>>(roles);
                    }
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
                    var result = await connection.QueryAsync<AspNetRole>(RoleQueries.UpdateRole, new { Id = aspNetRole.Id, Name = aspNetRole.Name, NormalizedName = aspNetRole.Name.ToUpper(), DateModifiedUtc = aspNetRole.DateModifiedUtc, ModifiedByUserId = aspNetRole.ModifiedByUserId });
                    return result.ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ExecutionResult<GridData<AspNetRole>>> GetAllFilterRoles(string? roleName, int pageNumber, string? updatedBy, string? updatedDate)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    updatedBy = !string.IsNullOrEmpty(updatedBy) ? updatedBy = "%" + updatedBy + "%" : updatedBy;
                    roleName = !string.IsNullOrEmpty(roleName) ? roleName = roleName + "%" : roleName;
                    var result = await connection.QueryAsync<AspNetRole>(RoleQueries.GetAllFilterRoles, new { Name = roleName, UpdatedBy = updatedBy, UpdatedDate = updatedDate });
                    var recordsPerPage = 10;
                    var skipRecords = (pageNumber - 1) * recordsPerPage;
                    if (pageNumber > 0)
                    {
                        var totalRecords = result.Count();
                        var roleList = result.Skip(skipRecords).Take(recordsPerPage).ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var roles = new GridData<AspNetRole>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = roleList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<AspNetRole>>(roles);
                    }
                    else
                    {
                        var totalRecords = result.Count();
                        var roleList = result.ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var roles = new GridData<AspNetRole>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = roleList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<AspNetRole>>(roles);
                    }
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

        public async Task<List<Menu>> GetMenuAceessRightForRole(string roleId)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                   var menu = await connection.QueryAsync<Menu>(RoleQueries.GetMenuAceessRightForRole, new { Role = roleId });
                    return menu.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> SaveMenuAccessOfRole(UserMenuPermission userMenuPermission)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    userMenuPermission.ModifiedByUserId = userMenuPermission.CreatedByUserId;
                    var deleteExistingRole = await connection.QueryAsync<UserMenuPermission>(RoleQueries.DeleteExistingPermissionOfRole, new { RoleId = userMenuPermission.RoleId, DateModifiedUtc=DateTime.UtcNow, ModifiedByUserId =userMenuPermission.ModifiedByUserId});
                    userMenuPermission.IsActive=true;
                    userMenuPermission.IsDeleted = false;
                    var menuIds = userMenuPermission.MenuIds.Split(",");
                    foreach (var item in menuIds)
                    {
                        userMenuPermission.MenuId =System.Convert.ToInt64(item);
                        var result = await connection.QueryAsync<string>(RoleQueries.SaveMenuAccessOfRole, userMenuPermission);

                    }
                    // var result =  connection.Query(RoleQueries.AddRole,aspnetRole);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}