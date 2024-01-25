using AutoMapper;
using Hutech.Application.Interfaces;
using Hutech.Core.Entities;
using Hutech.Models;
using Imputabiliteafro.Api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hutech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstrumentActivityController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IInstrumentActivityRepository activityRepository;
        private readonly ILogger<InstrumentActivityController> logger;
        public InstrumentActivityController(IMapper _mapper, IInstrumentActivityRepository _activityRepository, ILogger<InstrumentActivityController> _logger)
        {
            mapper = _mapper;
            activityRepository = _activityRepository;
            logger = _logger;
        }
        [HttpPost("PostInstrumentActivity")]
        public async Task<ApiResponse<string>> PostInstrumentActivity(InstrumentActivityViewModel instrumentActivityViewModel)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var activitydata = mapper.Map<InstrumentActivityViewModel, InstrumentActivity>(instrumentActivityViewModel);
                bool data = await activityRepository.PostInstrumentActivity(activitydata);
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
        [HttpGet("GetActiveInstrumentActivity")]
        public async Task<ApiResponse<List<InstrumentActivityViewModel>>> GetActiveInstrumentActivity()
        {
            try
            {
                var apiResponse = new ApiResponse<List<InstrumentActivityViewModel>>();
                var activity = await activityRepository.GetActiveInstrumentActivity();
                var data = mapper.Map<List<InstrumentActivity>, List<InstrumentActivityViewModel>>(activity);
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
        [HttpGet("GetAllInstrumentActivity")]
        public async Task<ApiResponse<List<InstrumentActivityViewModel>>> GetAllInstrumentActivity()
        {
            try
            {
                var apiResponse = new ApiResponse<List<InstrumentActivityViewModel>>();
                var activity = await activityRepository.GetInstrumentActivity();
                var data = mapper.Map<List<InstrumentActivity>, List<InstrumentActivityViewModel>>(activity);
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
        
       [HttpDelete("DeleteInstrumentActivity/{Id}")]
        public async Task<ApiResponse<string>> DeleteInstrumentActivity(long Id)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var role = await activityRepository.DeleteInstrumentActivity(Id);
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
        [HttpGet("GetInstrumentActivityDetailData/{id}")]
        public async Task<ApiResponse<InstrumentActivityViewModel>> GetInstrumentActivityDetailData(long id)
        {
            try
            {
                var apiResponse = new ApiResponse<InstrumentActivityViewModel>();
                var activity = await activityRepository.GetInstrumentActivityDetailData(id);
                var data = mapper.Map<InstrumentActivity, InstrumentActivityViewModel>(activity);
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
        [HttpGet("GetInstrumentActivity/{id}")]
        public async Task<ApiResponse<InstrumentActivityViewModel>> GetInstrumentActivity(long id)
        {
            try
            {
                var apiResponse = new ApiResponse<InstrumentActivityViewModel>();
                var activity = await activityRepository.GetInstrumentActivityDetail(id);
                var data = mapper.Map<InstrumentActivity, InstrumentActivityViewModel>(activity);
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
        [HttpPut("PutInstrumentActivity")]
        public async Task<ApiResponse<string>> PutInstrumentActivity(InstrumentActivityViewModel model)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var data = mapper.Map<InstrumentActivityViewModel, InstrumentActivity>(model);
                var role = await activityRepository.PutInstrumentActivity(data);
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
