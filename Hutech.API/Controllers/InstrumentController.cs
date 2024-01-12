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
    public class InstrumentController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IInstrumentRepository instrumentRepository;
        private readonly ILogger<InstrumentController> logger;
        public InstrumentController(IMapper _mapper, IInstrumentRepository _instrumentRepository, ILogger<InstrumentController> _logger)
        {
            mapper = _mapper;
            instrumentRepository = _instrumentRepository;
            logger = _logger;
        }
        [HttpPost("PostInstrument")]
        public async Task<ApiResponse<string>> PostInstrumentt(InstrumentViewModel instrumentViewModel)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var instrumentdata = mapper.Map<InstrumentViewModel, Instrument>(instrumentViewModel);
                bool data = await instrumentRepository.PostInstrument(instrumentdata);
                apiResponse.Result = "instrumentdata added successfully";
                apiResponse.Success = data;
                return apiResponse;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;
            }
        }
        [HttpGet("GetInstrumentDetail/{id}")]
        public async Task<ApiResponse<InstrumentViewModel>> GetInstrumentDetail(long id)
        {
            try
            {
                var apiResponse = new ApiResponse<InstrumentViewModel>();
                var instrument = await instrumentRepository.GetInstrumentDetail(id);
                var data = mapper.Map<Instrument, InstrumentViewModel>(instrument);
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
        [HttpDelete("DeleteInstrument/{Id}")]
        public async Task<ApiResponse<string>> DeleteInstrument(long Id)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var role = await instrumentRepository.DeleteInstrument(Id);
                apiResponse.Success = true;
                apiResponse.Message = "instrument deleted Successfully";
                return apiResponse;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;
            }
        }
        [HttpPut("PutInstrument")]
        public async Task<ApiResponse<string>> PutInstrument(InstrumentViewModel model)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var data = mapper.Map<InstrumentViewModel, Instrument>(model);
                var role = await instrumentRepository.PutInstrument(data);
                apiResponse.Success = true;
                apiResponse.Message = "Update instrument Successfully";
                return apiResponse;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;
            }
        }
        [HttpGet("GetInstrument")]
        public async Task<ApiResponse<List<InstrumentViewModel>>> GetInstrument()
        {
            try
            {
                var apiResponse = new ApiResponse<List<InstrumentViewModel>>();
                var instrument = await instrumentRepository.GetInstrument();
                var data = mapper.Map<List<Instrument>, List<InstrumentViewModel>>(instrument);
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
    }
}
