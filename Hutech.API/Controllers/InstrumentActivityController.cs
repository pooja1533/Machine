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
    public class InstrumentActivityController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IInstrumentActivityRepository activityRepository;
        private readonly ILogger<InstrumentActivityController> logger;
        private readonly IEmailSenderService emailSenderService;
        private readonly IAuditRepository auditRepository;
        public InstrumentActivityController(IMapper _mapper, IInstrumentActivityRepository _activityRepository, ILogger<InstrumentActivityController> _logger, IEmailSenderService _emailSenderService, IAuditRepository _auditRepository)
        {
            mapper = _mapper;
            activityRepository = _activityRepository;
            logger = _logger;
            emailSenderService = _emailSenderService;
            auditRepository = _auditRepository;
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
        [HttpGet("GetActiveInstrumentActivity")]
        public async Task<ApiResponse<List<InstrumentActivityViewModel>>> GetActiveInstrumentActivity()
        {
            var apiResponse = new ApiResponse<List<InstrumentActivityViewModel>>();
            try
            {
                var activity = await activityRepository.GetActiveInstrumentActivity();
                var data = mapper.Map<List<InstrumentActivity>, List<InstrumentActivityViewModel>>(activity);
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
        [HttpGet("GetAllInstrumentActivity")]
        public async Task<ApiResponse<List<InstrumentActivityViewModel>>> GetAllInstrumentActivity()
        {
            var apiResponse = new ApiResponse<List<InstrumentActivityViewModel>>();
            try
            {
                var activity = await activityRepository.GetInstrumentActivity();
                var data = mapper.Map<List<InstrumentActivity>, List<InstrumentActivityViewModel>>(activity);
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
        
       [HttpDelete("DeleteInstrumentActivity/{Id}")]
        public async Task<ApiResponse<string>> DeleteInstrumentActivity(long Id)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var role = await activityRepository.DeleteInstrumentActivity(Id);
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
        [HttpGet("GetInstrumentActivityDetailData/{id}")]
        public async Task<ApiResponse<InstrumentActivityViewModel>> GetInstrumentActivityDetailData(long id)
        {
            var apiResponse = new ApiResponse<InstrumentActivityViewModel>();
            try
            {
                var activity = await activityRepository.GetInstrumentActivityDetailData(id);
                var data = mapper.Map<InstrumentActivity, InstrumentActivityViewModel>(activity);
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
        [HttpGet("GetInstrumentActivity/{id}")]
        public async Task<ApiResponse<InstrumentActivityViewModel>> GetInstrumentActivity(long id)
        {
            var apiResponse = new ApiResponse<InstrumentActivityViewModel>();
            try
            {
                var activity = await activityRepository.GetInstrumentActivityDetail(id);
                var data = mapper.Map<InstrumentActivity, InstrumentActivityViewModel>(activity);
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
        [HttpPut("PutInstrumentActivity")]
        public async Task<ApiResponse<string>> PutInstrumentActivity(InstrumentActivityViewModel model)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var data = mapper.Map<InstrumentActivityViewModel, InstrumentActivity>(model);
                var role = await activityRepository.PutInstrumentActivity(data);
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
        [HttpGet("SendMail")]
        public void SendMail()
        {
            var toEmails = new List<string>() { "aakashi0112@gmail.com" };

            emailSenderService.SendEmail(new MessageServiceModel(toEmails, "AFROVIGIL XPRT – Registration received",
                $"Dear {"Pooja"}, <br /> <br/> We have received your registration form and are currently reviewing it. " +
                $"<br />You will be notified about further action soon. <br /> <br /> - <br /> <br /> Regards <br /> <br /> Team: AFROVIGIL XPRT"));
        }
    }
}
