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
    public class ActivityRepository : IActivityRepository
    {
        public IConfiguration configuration;
        public ActivityRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public async Task<string> DeleteActivity(long Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Activity>(ActivityQueries.DeleteActivity, new { Id = Id });
                    return result.ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ExecutionResult<GridData<Activity>>> GetActivity(int pageNumber = 1)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Activity>(ActivityQueries.GetActivity);
                    var recordsPerPage = 10;
                    var skipRecords = (pageNumber - 1) * recordsPerPage;
                    if (pageNumber > 0)
                    {
                        var totalRecords = result.Count();
                        var activityList = result.Skip(skipRecords).Take(recordsPerPage).ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var activities = new GridData<Activity>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = activityList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<Activity>>(activities);
                    }
                    else
                    {
                        var totalRecords = result.Count();
                        var activityList = result.ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var activities = new GridData<Activity>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = activityList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<Activity>>(activities);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Activity>> GetActiveActivity()
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Activity>(ActivityQueries.GetActiveActivity);
                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<Activity> GetActivityDetail(long Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Activity>(ActivityQueries.GetActivityDetail, new { Id = Id });
                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> PostActivity(Activity activity)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    activity.IsActive = true;
                    var result = await connection.QueryAsync<string>(ActivityQueries.PostActivity, activity);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> PutActivity(Activity activity)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Activity>(ActivityQueries.UpdateActivity, new { Id = activity.Id, Name = activity.Name, IsActive = activity.IsActive, DateModifiedUtc=activity.DateModifiedUtc, ModifiedByUserId=activity.ModifiedByUserId });
                    return result.ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ExecutionResult<GridData<Activity>>> GetAllFilterActivity(string? ActivityName, int pageNumber, string? updatedBy, string? status, string? updatedDate)
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
                    ActivityName = !string.IsNullOrEmpty(ActivityName) ? ActivityName = ActivityName + "%" : ActivityName;
                    var result = await connection.QueryAsync<Activity>(ActivityQueries.GetAllFilterActivity, new { Name = ActivityName, UpdatedBy = updatedBy, Status = isactive, UpdatedDate = updatedDate });
                    var recordsPerPage = 10;
                    var skipRecords = (pageNumber - 1) * recordsPerPage;
                    if (pageNumber > 0)
                    {
                        var totalRecords = result.Count();
                        var activityList = result.Skip(skipRecords).Take(recordsPerPage).ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var activities = new GridData<Activity>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = activityList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<Activity>>(activities);
                    }
                    else
                    {
                        var totalRecords = result.Count();
                        var activityList = result.ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var activities = new GridData<Activity>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = activityList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<Activity>>(activities);
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
