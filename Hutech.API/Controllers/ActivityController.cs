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
    public class ActivityController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IActivityRepository activityRepository;
        private readonly ILogger<ActivityController> logger;
        public ActivityController(IMapper _mapper, IActivityRepository _activityRepository, ILogger<ActivityController> _logger)
        {
            mapper = _mapper;
            activityRepository = _activityRepository;
            logger = _logger;
        }
        [HttpPost("PostActivity")]
        public async Task<ApiResponse<string>> PostActivity(ActivityViewModel activityViewModel)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var activitydata = mapper.Map<ActivityViewModel, Activity>(activityViewModel);
                bool data = await activityRepository.PostActivity(activitydata);
                apiResponse.Result = "activity added successfully";
                apiResponse.Success = data;
                return apiResponse;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;
            }
        }
        [HttpGet("GetActivity")]
        public async Task<ApiResponse<List<ActivityViewModel>>> GetActivity()
        {
            try
            {
                var apiResponse = new ApiResponse<List<ActivityViewModel>>();
                var activity = await activityRepository.GetActivity();
                var data = mapper.Map<List<Activity>, List<ActivityViewModel>>(activity);
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
        [HttpGet("GetActivityDetail/{id}")]
        public async Task<ApiResponse<ActivityViewModel>> GetActivityDetail(long id)
        {
            try
            {
                var apiResponse = new ApiResponse<ActivityViewModel>();
                var activity = await activityRepository.GetActivityDetail(id);
                var data = mapper.Map<Activity, ActivityViewModel>(activity);
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
        [HttpDelete("DeleteActivity/{Id}")]
        public async Task<ApiResponse<string>> DeleteActivity(long Id)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var role = await activityRepository.DeleteActivity(Id);
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
        [HttpPut("PutActivity")]
        public async Task<ApiResponse<string>> PutActivity(ActivityViewModel model)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var data = mapper.Map<ActivityViewModel, Activity>(model);
                var role = await activityRepository.PutActivity(data);
                apiResponse.Success = true;
                apiResponse.Message = "Update Activity Successfully";
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
