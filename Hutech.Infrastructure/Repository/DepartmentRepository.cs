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
    public class DepartmentRepository : IDepartmentRepository
    {
        public IConfiguration configuration;
        public DepartmentRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public async Task<string> DeleteDepartment(long Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Department>(DepartmentQueries.DeleteDepartment, new { Id = Id });
                    return result.ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ExecutionResult<GridData<Department>>> GetDepartment(int pageNumber)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Department>(DepartmentQueries.GetDepartment);
                    var recordsPerPage = 10;
                    var skipRecords = (pageNumber - 1) * recordsPerPage;
                    if (pageNumber > 0)
                    {
                        var totalRecords = result.Count();
                        var departmentlist = result.Skip(skipRecords).Take(recordsPerPage).ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var departments = new GridData<Department>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = departmentlist,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<Department>>(departments);
                    }
                    else
                    {
                        var totalRecords = result.Count();
                        var departmentlist = result.ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var departments = new GridData<Department>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = departmentlist,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<Department>>(departments);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Department>> GetActiveDepartment()
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Department>(DepartmentQueries.GetActiveDepartment);
                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Department> GetDepartmentDetail(long Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Department>(DepartmentQueries.GetDepartmentDetail, new { Id = Id });
                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> PostDepartment(Department department)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    department.IsActive = true;
                    var result = await connection.QueryAsync<string>(DepartmentQueries.PostDepartment, department);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> PutDepartment(Department department)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Department>(DepartmentQueries.UpdateDepartment, new { Id = department.Id, Name = department.Name, IsActive = department.IsActive, DateModifiedUtc = department.DateModifiedUtc, ModifiedByUserId = department.ModifiedByUserId });
                    return result.ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Department>> GetAllFilterDepartment(string? DepartmentName, string? updatedBy, string? status, string? updatedDate)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    updatedBy = !string.IsNullOrEmpty(updatedBy) ? updatedBy = "%" + updatedBy + "%" : updatedBy;
                    bool isactive = status == "1" ? true : false;
                    DepartmentName = !string.IsNullOrEmpty(DepartmentName) ? DepartmentName = DepartmentName + "%" : DepartmentName;
                    var result = await connection.QueryAsync<Department>(DepartmentQueries.GetAllFilterDepartment, new { Name = DepartmentName, UpdatedBy = updatedBy, Status = isactive, UpdatedDate = updatedDate });
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
