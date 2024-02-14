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
        private readonly IAuditRepository auditRepository;
        public InstrumentIdController(IMapper _mapper, IInstrumentIdRepository _instrumentIdRepository, ILogger<InstrumentIdController> _logger, IAuditRepository _auditRepository)
        {
            mapper = _mapper;
            instrumentIdRepository = _instrumentIdRepository;
            logger = _logger;
            auditRepository = _auditRepository;
        }
        [HttpGet("GetActiveInstrumentId")]
        public async Task<ApiResponse<List<InstrumentIdViewModel>>> GetActiveInstrumentId()
        {
            var apiResponse = new ApiResponse<List<InstrumentIdViewModel>>();
            try
            {
                var activity = await instrumentIdRepository.GetActiveInstrumentId();
                var data = mapper.Map<List<InstrumentsIds>, List<InstrumentIdViewModel>>(activity);
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
        [HttpGet("GetInstrumentId/{pageNumber}")]
        public async Task<ApiResponse<List<InstrumentIdViewModel>>> GetInstrumentId(int pageNumber)
        {
            var apiResponse = new ApiResponse<List<InstrumentIdViewModel>>();
            try
            {
                var activity = await instrumentIdRepository.GetInstrumentId(pageNumber);
                var data = mapper.Map<List<InstrumentsIds>, List<InstrumentIdViewModel>>(activity.Value.GridRecords);
                apiResponse.Success = true;
                apiResponse.CurrentPage = activity.Value.CurrentPage;
                apiResponse.TotalPage = activity.Value.TotalPages;
                apiResponse.TotalRecords = activity.Value.TotalRecords;
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
        [HttpPost("PostInstrumentId")]
        public async Task<ApiResponse<string>> PostInstrumentId(InstrumentIdViewModel instrumentIdViewModel)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var activitydata = mapper.Map<InstrumentIdViewModel, InstrumentsIds>(instrumentIdViewModel);
                bool data = await instrumentIdRepository.PostInstrumentId(activitydata);
                apiResponse.Result = "activity added successfully";
                apiResponse.Success = true;
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
        [HttpDelete("DeleteInstrumentId/{Id}")]
        public async Task<ApiResponse<string>> DeleteInstrumentId(long Id)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var role = await instrumentIdRepository.DeleteInstrumentId(Id);
                apiResponse.Success = true;
                apiResponse.Message = "Activity deleted Successfully";
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
        [HttpGet("GetInstrumentIdDetail/{id}")]
        public async Task<ApiResponse<InstrumentIdViewModel>> GetInstrumentIdDetail(long id)
        {
            var apiResponse = new ApiResponse<InstrumentIdViewModel>();
            try
            {
                var activity = await instrumentIdRepository.GetInstrumentIdDetail(id);
                var data = mapper.Map<InstrumentsIds, InstrumentIdViewModel>(activity);
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
        [HttpPut("PutInstrumentId")]
        public async Task<ApiResponse<string>> PutInstrumentId(InstrumentIdViewModel model)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var data = mapper.Map<InstrumentIdViewModel, InstrumentsIds>(model);
                var role = await instrumentIdRepository.PutInstrumentId(data);
                apiResponse.Success = true;
                apiResponse.Message = "Update Activity Successfully";
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
    }
}
