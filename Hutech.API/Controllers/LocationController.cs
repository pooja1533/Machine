using AutoMapper;
using Hutech.Application.Interfaces;
using Hutech.Core.Entities;
using Hutech.Infrastructure.Repository;
using Hutech.Models;
using Imputabiliteafro.Api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Hutech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ILocationRepository locationRepository;
        private readonly ILogger<LocationController> logger;
        private readonly IAuditRepository auditRepository;
        public LocationController(IMapper _mapper,ILocationRepository _locationRepository, ILogger<LocationController> _logger, IAuditRepository _auditRepository)
        {
            mapper = _mapper;
            locationRepository = _locationRepository;
            logger = _logger;
            auditRepository = _auditRepository;
        }
        [HttpPost("GetAllFilterLocation")]
        public async Task<ApiResponse<List<LocationViewModel>>> GetAllFilterLocation(LocationModel locationModel)
        {
            var apiResponse = new ApiResponse<List<LocationViewModel>>();
            try
            {
                string? locationName = locationModel.locationName;
                int pageNumber = locationModel.pageNumber;
                string? updatedBy=locationModel.updatedBy;
                string? status=locationModel.status;
                DateTime? updatedDate=locationModel.updatedDate;
                string formattedDate = updatedDate?.ToString("yyyy-MM-dd");

                var location = await locationRepository.GetAllFilterLocation(locationName,pageNumber,updatedBy,status, formattedDate);
                var data = mapper.Map<List<Location>, List<LocationViewModel>>(location.Value.GridRecords);
                apiResponse.Success = true;
                apiResponse.Result = data;
                apiResponse.CurrentPage = location.Value.CurrentPage;
                apiResponse.TotalPage = location.Value.TotalPages;
                apiResponse.TotalRecords = location.Value.TotalRecords;
                return apiResponse;
            }
            catch (Exception ex)
            {
                var id = RouteData.Values["AuditId"];
                logger.LogInformation($"Exception Occure in API.{ex.Message}" + "{@AuditId}", id);
                long auditId = System.Convert.ToInt64(id);
                auditRepository.AddExceptionDetails(auditId, ex.Message);
                apiResponse.Success = false;
                apiResponse.AuditId = auditId;
                return apiResponse;
            }

        }
        [HttpPost("PostLocation")]
        public async Task<ApiResponse<string>> PostLocation(LocationViewModel locationViewModel)
        {
            try
            {
                //string dataa = null;
                //var length = dataa.Length;
                var apiResponse = new ApiResponse<string>();
                var locationdata = mapper.Map<LocationViewModel, Location>(locationViewModel);
                bool data = await locationRepository.PostLocation(locationdata);
                apiResponse.Result = "Location added successfully";
                apiResponse.Success = true;
                return apiResponse;
            }
            catch (Exception ex)
            {
                var id = RouteData.Values["AuditId"];
                logger.LogInformation($"Exception Occure in API.{ex.Message}" + "{@AuditId}", id);
                long auditId = System.Convert.ToInt64(id);
                auditRepository.AddExceptionDetails(auditId, ex.Message);
                var apiResponse = new ApiResponse<string>();
                apiResponse.Success = false;
                apiResponse.AuditId = auditId;
                return apiResponse;
            }
        }
        [HttpGet("GetActiveLocation")]
        public async Task<ApiResponse<List<LocationViewModel>>> GetActiveLocation()
        {
            var apiResponse = new ApiResponse<List<LocationViewModel>>();
            try
            {
                var location = await locationRepository.GetActiveLocation();
                var data = mapper.Map<List<Location>, List<LocationViewModel>>(location);
                apiResponse.Success = true;
                apiResponse.Result = data;
                return apiResponse;
            }
            catch (Exception ex)
            {
                var id = RouteData.Values["AuditId"];
                logger.LogInformation($"Exception Occure in API.{ex.Message}" + "{@AuditId}", id);
                long auditId = System.Convert.ToInt64(id);
                auditRepository.AddExceptionDetails(auditId, ex.Message);
                apiResponse.Success = false;
                apiResponse.AuditId = auditId;
                return apiResponse;
            }
        }
        [HttpGet("GetLocation/{pageNumber}")]
        public async Task<ApiResponse<List<LocationViewModel>>> GetLocation(int pageNumber)
        {
            var apiResponse = new ApiResponse<List<LocationViewModel>>();
            try
            {
                var location = await locationRepository.GetLocation(pageNumber);
                var data = mapper.Map<List<Location>, List<LocationViewModel>>(location.Value.GridRecords);
                apiResponse.Success = true;
                apiResponse.Result = data;
                apiResponse.CurrentPage = location.Value.CurrentPage;
                apiResponse.TotalPage = location.Value.TotalPages;
                apiResponse.TotalRecords = location.Value.TotalRecords;
                return apiResponse;
            }
            catch (Exception ex)
            {
                var id = RouteData.Values["AuditId"];
                logger.LogInformation($"Exception Occure in API.{ex.Message}" + "{@AuditId}", id);
                long auditId = System.Convert.ToInt64(id);
                auditRepository.AddExceptionDetails(auditId, ex.Message);
                apiResponse.Success = false;
                apiResponse.AuditId = auditId;
                return apiResponse;
            }
        }
        [HttpGet("GetLocationDetail/{id}")]
        public async Task<ApiResponse<LocationViewModel>> GetLocationDetail(long id)
        {
            var apiResponse = new ApiResponse<LocationViewModel>();
            try
            {
                var location = await locationRepository.GetLocationDetail(id);
                var data = mapper.Map<Location, LocationViewModel>(location);
                apiResponse.Success = true;
                apiResponse.Result = data;
                return apiResponse;
            }
            catch (Exception ex)
            {
                var Id = RouteData.Values["AuditId"];
                logger.LogInformation($"Exception Occure in API.{ex.Message}" + "{@AuditId}", Id);
                long auditId = System.Convert.ToInt64(Id);
                auditRepository.AddExceptionDetails(auditId, ex.Message);
                apiResponse.Success = false;
                apiResponse.AuditId = auditId;
                return apiResponse;
            }
        }
        [HttpDelete("DeleteLocation/{Id}")]
        public async Task<ApiResponse<string>> DeleteLocation(long Id)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var role = await locationRepository.DeleteLocation(Id);
                apiResponse.Success = true;
                apiResponse.Message = "location deleted Successfully";
                return apiResponse;
            }
            catch (Exception ex)
            {
                var id = RouteData.Values["AuditId"];
                logger.LogInformation($"Exception Occure in API.{ex.Message}" + "{@AuditId}", id);
                long auditId = System.Convert.ToInt64(id);
                auditRepository.AddExceptionDetails(auditId, ex.Message);
                apiResponse.Success = false;
                apiResponse.AuditId = auditId;
                return apiResponse;
            }
        }
        [HttpPut("PutLocation")]
        public async Task<ApiResponse<string>> PutLocation(LocationViewModel model)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var data = mapper.Map<LocationViewModel, Location>(model);
                var role = await locationRepository.PutLocation(data);
                apiResponse.Success = true;
                apiResponse.Message = "Update location Successfully";
                return apiResponse;
            }
            catch (Exception ex)
            {
                var id = RouteData.Values["AuditId"];
                logger.LogInformation($"Exception Occure in API.{ex.Message}" + "{@AuditId}", id);
                long auditId = System.Convert.ToInt64(id);
                auditRepository.AddExceptionDetails(auditId, ex.Message);
                apiResponse.Success = false;
                apiResponse.AuditId = auditId;
                return apiResponse;
            }
        }
    }
}
