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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Infrastructure.Repository
{
    public class RequirementRepository : IRequirementRepository
    {
        public IConfiguration configuration;
        public RequirementRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public async Task<string> DeleteRequirement(long Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Requirement>(RequirementQueries.DeleteRequirement, new { Id = Id });
                    return result.ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Requirement>> GetRequirement()
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Requirement>(RequirementQueries.GetRequirement);
                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Requirement>> GetActiveRequirement()
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Requirement>(RequirementQueries.GetActiveRequirement);
                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Requirement> GetRequirementDetail(long Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Requirement>(RequirementQueries.GetRequirementDetail, new { Id = Id });
                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> PostRequirement(Requirement requirement)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    requirement.IsActive = true;
                    var result = await connection.QueryAsync<string>(RequirementQueries.PostRequirement, requirement);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> PutRequirement(Requirement requirement)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Requirement>(RequirementQueries.UpdateRequirement, new { Id = requirement.Id, Name = requirement.Name, IsActive = requirement.IsActive, DateModifiedUtc= requirement.DateModifiedUtc, ModifiedByUserId=requirement.ModifiedByUserId });
                    return result.ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ExecutionResult<GridData<Requirement>>> GetAllFilterRequirement(string? RequirementName, int pageNumber, string? updatedBy, string? status, string? updatedDate)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    updatedBy = !string.IsNullOrEmpty(updatedBy) ? updatedBy = "%" + updatedBy + "%" : updatedBy;
                    bool isactive = false;
                    if (string.IsNullOrEmpty(status) || status == "1")
                        isactive = true;
                    RequirementName = !string.IsNullOrEmpty(RequirementName) ? RequirementName = RequirementName + "%" : RequirementName;
                    var result = await connection.QueryAsync<Requirement>(RequirementQueries.GetAllFilterRequirement, new { Name = RequirementName, UpdatedBy = updatedBy, Status = isactive, UpdatedDate = updatedDate });
                    var recordsPerPage = 10;
                    var skipRecords = (pageNumber - 1) * recordsPerPage;
                    if (pageNumber > 0)
                    {
                        var totalRecords = result.Count();
                        var requirementList = result.Skip(skipRecords).Take(recordsPerPage).ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var requirements = new GridData<Requirement>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = requirementList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<Requirement>>(requirements);
                    }
                    else
                    {
                        var totalRecords = result.Count();
                        var requirementList = result.ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var requirements = new GridData<Requirement>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = requirementList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<Requirement>>(requirements);
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
