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
    public class TeamRepository : ITeamRepository
    {
        public IConfiguration configuration;
        public TeamRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public async Task<string> DeleteTeam(long Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Team>(TeamQueries.DeleteTeam, new { Id = Id });
                    return result.ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Team>> GetTeam()
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Team>(TeamQueries.GetTeam);
                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Team>> GetActiveTeam()
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Team>(TeamQueries.GetActiveTeam);
                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Team> GetTeamDetail(long Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Team>(TeamQueries.GetTeamDetail, new { Id = Id });
                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> PostTeam(Team team)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    team.IsActive = true;
                    var result = await connection.QueryAsync<string>(TeamQueries.PostTeam, team);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<string> UpdateTeam(Team team)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Team>(TeamQueries.UpdateTeam, new { Id = team.Id, Name = team.Name, LocationId = team.LocationId, IsActive = team.IsActive, DateModifiedUtc=team.DateModifiedUtc, ModifiedByUserId=team.ModifiedByUserId,DepartmentId=team.DepartmentId });
                    return result.ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ExecutionResult<GridData<Team>>> GetAllFilterTeam(string? TeamName, int pageNumber, string? updatedBy, string? status, string? updatedDate,string? LocationName,string? DepartmentName)
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
                    LocationName = !string.IsNullOrEmpty(LocationName) ? LocationName + "%" : LocationName;
                    TeamName = !string.IsNullOrEmpty(TeamName) ? TeamName = TeamName + "%" : TeamName;
                    DepartmentName=!string.IsNullOrEmpty(DepartmentName)? DepartmentName+"%" : DepartmentName;
                    var result = await connection.QueryAsync<Team>(TeamQueries.GetAllFilterTeam, new { Name = TeamName, UpdatedBy = updatedBy, Status = isactive, UpdatedDate = updatedDate, LocationName=LocationName , DepartmentName = DepartmentName });
                    var recordsPerPage = 10;
                    var skipRecords = (pageNumber - 1) * recordsPerPage;
                    if (pageNumber > 0)
                    {
                        var totalRecords = result.Count();
                        var teamList = result.Skip(skipRecords).Take(recordsPerPage).ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var teams = new GridData<Team>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = teamList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<Team>>(teams);
                    }
                    else
                    {
                        var totalRecords = result.Count();
                        var teamList = result.ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var teams = new GridData<Team>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = teamList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<Team>>(teams);
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
