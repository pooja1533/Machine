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
    public class ActivityDetailsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IActivityDetailRepository activityDetailRepository;
        private readonly ILogger<ActivityDetailsController> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        public ActivityDetailsController(IMapper _mapper, IActivityDetailRepository _activityDetailRepository, ILogger<ActivityDetailsController> _logger, IHttpContextAccessor _httpContextAccessor)
        {
            mapper = _mapper;
            activityDetailRepository = _activityDetailRepository;
            logger = _logger;
            httpContextAccessor = _httpContextAccessor;
        }
        [HttpGet("GetAllActivityDetails/{userId}")]
        public async Task<ApiResponse<List<ActivityDetailsViewModel>>> GetAllActivityDetails(string userId)
        {
            try
            {
                var apiResponse = new ApiResponse<List<ActivityDetailsViewModel>>();
                var activity = await activityDetailRepository.GetAllActivityDetails(userId);
                var data = mapper.Map<List<ActivityDetails>, List<ActivityDetailsViewModel>>(activity);
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
        [HttpPost("PostActivityDetails")]
        public async Task<ApiResponse<string>> PostActivityDetails(ActivityDetailsViewModel activityDetailsViewModel)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var activitydata = mapper.Map<ActivityDetailsViewModel, ActivityDetails>(activityDetailsViewModel);
                activitydata.CreatedByUserId= httpContextAccessor.HttpContext.Session.GetString("UserId");
                activitydata.CreatedDate = DateTime.UtcNow;
                bool data = await activityDetailRepository.PostActivityDetail(activitydata);
                apiResponse.Result = "activityDetails added successfully";
                apiResponse.Success = data;
                return apiResponse;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;
            }
        }
        [HttpGet("GetActivityDetails/{id}")]
        public async Task<ApiResponse<ActivityDetailsViewModel>> GetActivityDetails(long id)
        {
            try
            {
                var apiResponse = new ApiResponse<ActivityDetailsViewModel>();
                var activity = await activityDetailRepository.GetActivityDetails(id);
                var data = mapper.Map<ActivityDetails, ActivityDetailsViewModel>(activity);
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
        [HttpDelete("DeleteActivityDetails/{Id}")]
        public async Task<ApiResponse<string>> DeleteActivityDetails(long Id)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var role = await activityDetailRepository.DeleteActivityDetails(Id);
                apiResponse.Success = true;
                apiResponse.Message = "Activity deleted Successfully";
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
