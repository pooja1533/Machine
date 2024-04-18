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
    public class ActivityDetailRepository : IActivityDetailRepository
    {
        public IConfiguration configuration;
        public ActivityDetailRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public async Task<ExecutionResult<GridData<ActivityDetails>>> GetAllFilterActivityDetail(string? InstrumentIdName,string InstrumentName,string InstrumentSerial,string Model,string Location,string Department, int pageNumber, string? updatedBy, string? status, string? updatedDate, string LoggedInUser)
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
                    InstrumentSerial = !string.IsNullOrEmpty(InstrumentSerial) ? InstrumentSerial = InstrumentSerial + "%" : InstrumentSerial;
                    InstrumentName = !string.IsNullOrEmpty(InstrumentName) ? InstrumentName = InstrumentName + "%" : InstrumentName;
                    InstrumentIdName = !string.IsNullOrEmpty(InstrumentIdName) ? InstrumentIdName = InstrumentIdName + "%" : InstrumentIdName;
                    Model = !string.IsNullOrEmpty(Model) ? Model=Model+"%" : Model;
                    Location = !string.IsNullOrEmpty(Location) ? Location = Location + "%" : Location;
                    Department = !string.IsNullOrEmpty(Department) ? Department = Department + "%" : Department;
                    var result = await connection.QueryAsync<ActivityDetails>(ActivityDetailsQueries.GetAllFilterActivityDetail, new { InstrumentIdName = InstrumentIdName,InstrumentName=InstrumentName,InstrumentSerial=InstrumentSerial,Model=Model,Location=Location,Department=Department, UpdatedBy = updatedBy, Status = isactive, UpdatedDate = updatedDate });
                    var recordsPerPage = 10;
                    var skipRecords = (pageNumber - 1) * recordsPerPage;
                    if (pageNumber > 0)
                    {
                        var totalRecords = result.Count();
                        var activitydetailsList = result.Skip(skipRecords).Take(recordsPerPage).ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var activitydetails = new GridData<ActivityDetails>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = activitydetailsList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<ActivityDetails>>(activitydetails);
                    }
                    else
                    {
                        var totalRecords = result.Count();
                        var activitydetailsList = result.ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var activitydetails = new GridData<ActivityDetails>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = activitydetailsList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<ActivityDetails>>(activitydetails);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ExecutionResult<GridData<ActivityDetails>>> GetAllActivityDetails(string userId, int pageNumber)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var recordsPerPage = 10;
                    var skipRecords = (pageNumber - 1) * recordsPerPage;
                    if (!string.IsNullOrEmpty(userId) && userId!="0")
                    {
                        var result = await connection.QueryAsync<ActivityDetails>(ActivityDetailsQueries.GetAllActivityDetails, new { LoggedInUser = userId });
                        if (pageNumber > 0)
                        {
                            var totalRecords = result.Count();
                            var activitydetailsList = result.Skip(skipRecords).Take(recordsPerPage).ToList();
                            var totalPages = ((double)totalRecords / (double)recordsPerPage);
                            var activitydetails = new GridData<ActivityDetails>()
                            {
                                CurrentPage = pageNumber,
                                TotalRecords = totalRecords,
                                GridRecords = activitydetailsList,
                                TotalPages = (int)Math.Ceiling(totalPages)
                            };
                            return new ExecutionResult<GridData<ActivityDetails>>(activitydetails);
                        }
                        else
                        {
                            var totalRecords = result.Count();
                            var activityDetailsList = result.ToList();
                            var totalPages = ((double)totalRecords / (double)recordsPerPage);
                            var activityDeatils = new GridData<ActivityDetails>()
                            {
                                CurrentPage = pageNumber,
                                TotalRecords = totalRecords,
                                GridRecords = activityDetailsList,
                                TotalPages = (int)Math.Ceiling(totalPages)
                            };
                            return new ExecutionResult<GridData<ActivityDetails>>(activityDeatils);
                        }
                    }
                    else
                    {
                        var result = await connection.QueryAsync<ActivityDetails>(ActivityDetailsQueries.GetAllUsersActivityDetails);
                        if (pageNumber > 0)
                        {
                            var totalRecords = result.Count();
                            var activitydetailsList = result.Skip(skipRecords).Take(recordsPerPage).ToList();
                            var totalPages = ((double)totalRecords / (double)recordsPerPage);
                            var activitydetails = new GridData<ActivityDetails>()
                            {
                                CurrentPage = pageNumber,
                                TotalRecords = totalRecords,
                                GridRecords = activitydetailsList,
                                TotalPages = (int)Math.Ceiling(totalPages)
                            };
                            return new ExecutionResult<GridData<ActivityDetails>>(activitydetails);
                        }
                        else
                        {
                            var totalRecords = result.Count();
                            var activityDetailsList = result.ToList();
                            var totalPages = ((double)totalRecords / (double)recordsPerPage);
                            var activityDeatils = new GridData<ActivityDetails>()
                            {
                                CurrentPage = pageNumber,
                                TotalRecords = totalRecords,
                                GridRecords = activityDetailsList,
                                TotalPages = (int)Math.Ceiling(totalPages)
                            };
                            return new ExecutionResult<GridData<ActivityDetails>>(activityDeatils);
                        }
                    }
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
        public async Task<bool> AddActivityDetailDocumentMapping(ActivityDetailDocumentMapping activityDetailDocumentMapping)
        {
            try
            {
                bool result = false;
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var activitydetailDocumentMappingresult = await connection.QueryAsync<string>(ActivityDetailsQueries.AddActivityDetailDocumentMapping, activityDetailDocumentMapping);
                    if (activitydetailDocumentMappingresult != null)
                    {
                        result = true;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<long> GetLastInsertedActivityDetailId()
        {
            try
            {
                long lastinstrumentId = 0;
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var instrumentDocumentMappingresult = await connection.QueryAsync<long>(ActivityDetailsQueries.GetLastInsertedActivityDetailId);
                    lastinstrumentId = instrumentDocumentMappingresult.First();
                }
                return lastinstrumentId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> DeleteDocument(long documentId, long activityDetailId)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<ActivityDetailDocumentMapping>(ActivityDetailsQueries.DeleteActivityDetailDocument, new { DocumentId = documentId, ActivityDetailId = activityDetailId });
                    var data = await connection.QueryAsync<Document>(DocumentQueries.DeleteDocument, new { Id = documentId });
                    return result.ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
