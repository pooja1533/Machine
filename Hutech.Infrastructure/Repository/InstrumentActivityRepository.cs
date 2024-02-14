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
using static Dapper.SqlMapper;

namespace Hutech.Infrastructure.Repository
{
    public class InstrumentActivityRepository : IInstrumentActivityRepository
    {
        public IConfiguration configuration;
        public InstrumentActivityRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public async Task<List<InstrumentActivity>> GetActiveInstrumentActivity()
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<InstrumentActivity>(InstrumentActivityQueries.GetActiveInstrumentActivity);
                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ExecutionResult<GridData<InstrumentActivity>>> GetInstrumentActivity(int pageNumber = 1)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<InstrumentActivity>(InstrumentActivityQueries.GetInstrumentActivity);
                    var recordsPerPage = 10;
                    var skipRecords = (pageNumber - 1) * recordsPerPage;
                    if (pageNumber > 0)
                    {
                        var totalRecords = result.Count();
                        var instrumentActivitiesList = result.Skip(skipRecords).Take(recordsPerPage).ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var instrumentActivities = new GridData<InstrumentActivity>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = instrumentActivitiesList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<InstrumentActivity>>(instrumentActivities);
                    }
                    else
                    {
                        var totalRecords = result.Count();
                        var instrumentActivitiesList = result.ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var instrumentActivities = new GridData<InstrumentActivity>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = instrumentActivitiesList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<InstrumentActivity>>(instrumentActivities);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> DeleteInstrumentActivity(long Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    await connection.ExecuteAsync(InstrumentActivityQueries.DeleteExistingInstrumentActivityGroup, new { InstrumentActivityId = Id });
                    var result = await connection.QueryAsync<Activity>(InstrumentActivityQueries.DeleteInstrumentActivity, new { Id = Id });
                    return result.ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> PostInstrumentActivity(InstrumentActivity instrumentActivity)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    instrumentActivity.IsActive = true;
                    instrumentActivity.IsDeleted = false;
                    var result = await connection.ExecuteAsync(InstrumentActivityQueries.PostInstrumentActivity, instrumentActivity);
                    if (result == 1)
                    {
                        int lastinsertedId = await connection.QuerySingleOrDefaultAsync<int>(InstrumentActivityQueries.GetLastInsertedId);

                        if (instrumentActivity.SelectedEmailListInt.Count > 0)
                        {
                            foreach (var item in instrumentActivity.SelectedEmailListInt)
                            {
                                var adduser = await connection.ExecuteAsync(InstrumentActivityQueries.AddUserOfInstrumentActivity, new { InstrumentActivityId = lastinsertedId, GroupId = item, IsActive = true, IsDeleted = false });
                            }
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<InstrumentActivity> GetInstrumentActivityDetailData(long Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<InstrumentActivity>(InstrumentActivityQueries.GetInstrumentActivityDetailData, new { Id = Id });
                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<InstrumentActivity> GetInstrumentActivityDetail(long Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<InstrumentActivity>(InstrumentActivityQueries.GetInstrumentActivityDetail, new { Id = Id });
                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> PutInstrumentActivity(InstrumentActivity activity)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.ExecuteAsync(InstrumentActivityQueries.PutInstrumentActivity, activity);
                    await connection.ExecuteAsync(InstrumentActivityQueries.DeleteExistingInstrumentActivityForGroup, new { InstrumentActivityId = activity.Id });

                    foreach (var item in activity.SelectedEmailListInt)
                    {
                        await connection.ExecuteAsync(InstrumentActivityQueries.AddUserOfInstrumentActivity, new { InstrumentActivityId = activity.Id, GroupId = item, IsDeleted = 0, IsActive = 1 });
                    }

                    return result.ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<DateTime> GetLastPerformedDateForInstrumentActivity(long instrumentActivityId)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<DateTime>(InstrumentActivityQueries.GetLastPerformedDateForInstrumentActivity, new { Id = instrumentActivityId });
                    return System.Convert.ToDateTime(result.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
