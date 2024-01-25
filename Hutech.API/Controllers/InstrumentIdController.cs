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
    public class InstrumentIdController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IInstrumentIdRepository instrumentIdRepository;
        private readonly ILogger<InstrumentIdController> logger;
        public InstrumentIdController(IMapper _mapper, IInstrumentIdRepository _instrumentIdRepository, ILogger<InstrumentIdController> _logger)
        {
            mapper = _mapper;
            instrumentIdRepository = _instrumentIdRepository;
            logger = _logger;
        }
        [HttpGet("GetActiveInstrumentId")]
        public async Task<ApiResponse<List<InstrumentIdViewModel>>> GetActiveInstrumentId()
        {
            try
            {
                var apiResponse = new ApiResponse<List<InstrumentIdViewModel>>();
                var activity = await instrumentIdRepository.GetActiveInstrumentId();
                var data = mapper.Map<List<InstrumentsIds>, List<InstrumentIdViewModel>>(activity);
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
        [HttpGet("GetInstrumentId")]
        public async Task<ApiResponse<List<InstrumentIdViewModel>>> GetInstrumentId()
        {
            try
            {
                var apiResponse = new ApiResponse<List<InstrumentIdViewModel>>();
                var activity = await instrumentIdRepository.GetInstrumentId();
                var data = mapper.Map<List<InstrumentsIds>, List<InstrumentIdViewModel>>(activity);
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
        [HttpPost("PostInstrumentId")]
        public async Task<ApiResponse<string>> PostInstrumentId(InstrumentIdViewModel instrumentIdViewModel)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var activitydata = mapper.Map<InstrumentIdViewModel, InstrumentsIds>(instrumentIdViewModel);
                bool data = await instrumentIdRepository.PostInstrumentId(activitydata);
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
        [HttpDelete("DeleteInstrumentId/{Id}")]
        public async Task<ApiResponse<string>> DeleteInstrumentId(long Id)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var role = await instrumentIdRepository.DeleteInstrumentId(Id);
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
        [HttpGet("GetInstrumentIdDetail/{id}")]
        public async Task<ApiResponse<InstrumentIdViewModel>> GetInstrumentIdDetail(long id)
        {
            try
            {
                var apiResponse = new ApiResponse<InstrumentIdViewModel>();
                var activity = await instrumentIdRepository.GetInstrumentIdDetail(id);
                var data = mapper.Map<InstrumentsIds, InstrumentIdViewModel>(activity);
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
        [HttpPut("PutInstrumentId")]
        public async Task<ApiResponse<string>> PutInstrumentId(InstrumentIdViewModel model)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var data = mapper.Map<InstrumentIdViewModel, InstrumentsIds>(model);
                var role = await instrumentIdRepository.PutInstrumentId(data);
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
