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
    public class LocationRepository : ILocationRepository
    {
        public IConfiguration configuration;
        public LocationRepository (IConfiguration _configuration)
        {
            configuration = _configuration; 
        }
        public async Task<bool> PostLocation(Location location)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    location.IsActive = true;
                    var result = await connection.QueryAsync<string>(LocationQueries.PostLocation, location);
                    return true;
                }
            }
            catch (Exception ex) {
                throw ex;
            }

        }
        public async Task<ExecutionResult<GridData<Location>>> GetAllFilterLocation(string? LocationName,int pageNumber, string? updatedBy,string? status, string? updatedDate)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    updatedBy = !string.IsNullOrEmpty(updatedBy) ? updatedBy ="%"+ updatedBy + "%" : updatedBy;
                    bool isactive = status=="1"?true:false;
                    LocationName = !string.IsNullOrEmpty(LocationName) ? LocationName = LocationName + "%" : LocationName;
                    var result = await connection.QueryAsync<Location>(LocationQueries.GetAllFilterLocation, new { Name = LocationName,UpdatedBy=updatedBy,Status= isactive, UpdatedDate= updatedDate });
                    var recordsPerPage = 10;
                    var skipRecords = (pageNumber - 1) * recordsPerPage;
                    if (pageNumber > 0)
                    {
                        var totalRecords = result.Count();
                        var locationList = result.Skip(skipRecords).Take(recordsPerPage).ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var locations = new GridData<Location>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = locationList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<Location>>(locations);
                    }
                    else
                    {
                        var totalRecords = result.Count();
                        var locationList = result.ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var locations = new GridData<Location>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = locationList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<Location>>(locations);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ExecutionResult<GridData<Location>>> GetLocation(int pageNumber)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Location>(LocationQueries.GetLocation);
                    var recordsPerPage = 10;
                    var skipRecords = (pageNumber - 1) * recordsPerPage;
                    if (pageNumber > 0)
                    {
                        var totalRecords = result.Count();
                        var locationList = result.Skip(skipRecords).Take(recordsPerPage).ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var locations = new GridData<Location>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = locationList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<Location>>(locations);
                    }
                    else
                    {
                        var totalRecords = result.Count();
                        var locationList = result.ToList();
                        var totalPages = ((double)totalRecords / (double)recordsPerPage);
                        var locations = new GridData<Location>()
                        {
                            CurrentPage = pageNumber,
                            TotalRecords = totalRecords,
                            GridRecords = locationList,
                            TotalPages = (int)Math.Ceiling(totalPages)
                        };
                        return new ExecutionResult<GridData<Location>>(locations);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<Location>> GetActiveLocation()
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Location>(LocationQueries.GetActiveLocation);
                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Location> GetLocationDetail(long Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Location>(LocationQueries.GetLocationDetail, new { Id = Id });
                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> DeleteLocation(long Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Location>(LocationQueries.DeleteLocation, new { Id = Id });
                    return result.ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> PutLocation(Location location)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<AspNetRole>(LocationQueries.UpdateLocation, new { Id = location.Id, Name = location.Name,IsActive=location.IsActive, ModifiedByUserId =location.ModifiedByUserId, DateModifiedUtc =location.DateModifiedUtc});
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
