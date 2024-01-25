using AutoMapper;
using Hutech.Application.Interfaces;
using Hutech.Core.Entities;
using Hutech.Infrastructure.Repository;
using Hutech.Models;
using Imputabiliteafro.Api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hutech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ILocationRepository locationRepository;
        private readonly ILogger<LocationController> logger;
        public LocationController(IMapper _mapper,ILocationRepository _locationRepository, ILogger<LocationController> _logger)
        {
            mapper = _mapper;
            locationRepository = _locationRepository;
            logger = _logger;
        }
        [HttpPost("PostLocation")]
        public async Task<ApiResponse<string>> PostLocation(LocationViewModel locationViewModel)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var locationdata = mapper.Map<LocationViewModel, Location>(locationViewModel);
                bool data = await locationRepository.PostLocation(locationdata);
                apiResponse.Result = "Location added successfully";
                apiResponse.Success = data;
                return apiResponse;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;
            }
        }
        [HttpGet("GetActiveLocation")]
        public async Task<ApiResponse<List<LocationViewModel>>> GetActiveLocation()
        {
            try
            {
                var apiResponse = new ApiResponse<List<LocationViewModel>>();
                var location = await locationRepository.GetActiveLocation();
                var data = mapper.Map<List<Location>, List<LocationViewModel>>(location);
                apiResponse.Success = true;
                apiResponse.Result = data;
                return apiResponse;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;
            }
        }
        [HttpGet("GetLocation")]
        public async Task<ApiResponse<List<LocationViewModel>>> GetLocation()
        {
            try
            {
                var apiResponse = new ApiResponse<List<LocationViewModel>>();
                var location = await locationRepository.GetLocation();
                var data = mapper.Map<List<Location>, List<LocationViewModel>>(location);
                apiResponse.Success = true;
                apiResponse.Result = data;
                return apiResponse;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;
            }
        }
        [HttpGet("GetLocationDetail/{id}")]
        public async Task<ApiResponse<LocationViewModel>> GetLocationDetail(long id)
        {
            try
            {
                var apiResponse = new ApiResponse<LocationViewModel>();
                var location = await locationRepository.GetLocationDetail(id);
                var data = mapper.Map<Location, LocationViewModel>(location);
                apiResponse.Success = true;
                apiResponse.Result = data;
                return apiResponse;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;
            }
        }
        [HttpDelete("DeleteLocation/{Id}")]
        public async Task<ApiResponse<string>> DeleteLocation(long Id)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var role = await locationRepository.DeleteLocation(Id);
                apiResponse.Success = true;
                apiResponse.Message = "location deleted Successfully";
                return apiResponse;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;
            }
        }
        [HttpPut("PutLocation")]
        public async Task<ApiResponse<string>> PutLocation(LocationViewModel model)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var data = mapper.Map<LocationViewModel, Location>(model);
                var role = await locationRepository.PutLocation(data);
                apiResponse.Success = true;
                apiResponse.Message = "Update location Successfully";
                return apiResponse;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;
            }
        }
    }
}
