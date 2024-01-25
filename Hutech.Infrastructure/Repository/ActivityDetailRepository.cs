using Dapper;
using Hutech.Application.Interfaces;
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
    public class ActivityDetailRepository : IActivityDetailRepository
    {
        public IConfiguration configuration;
        public ActivityDetailRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public async Task<List<ActivityDetails>> GetAllActivityDetails(string userId)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<ActivityDetails>(ActivityDetailsQueries.GetAllActivityDetails, new { LoggedInUser=userId});
                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> PostActivityDetail(ActivityDetails activityDetails)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    if (activityDetails.Id > 0)
                    {
                        var result = await connection.QueryAsync<string>(ActivityDetailsQueries.PutActivityDetail, activityDetails);

                    }
                    else
                    {
                        activityDetails.IsActive = true;
                        var result = await connection.QueryAsync<string>(ActivityDetailsQueries.PostActivityDetail, activityDetails);

                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> DeleteActivityDetails(long Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<ActivityDetails>(ActivityDetailsQueries.DeleteActivityDetails, new { Id = Id });
                    return result.ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ActivityDetails> GetActivityDetails(long Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<ActivityDetails>(ActivityDetailsQueries.GetActivityDetail, new { Id = Id });
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
