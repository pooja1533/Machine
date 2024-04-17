﻿using AutoMapper;
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
        [HttpPost("GetAllFilterInstrumentActivity")]
        public async Task<ApiResponse<List<InstrumentActivityViewModel>>> GetAllFilterInstrumentActivity(InstrumentActivityModel instrumentActivityModel)
        {
            var apiResponse = new ApiResponse<List<InstrumentActivityViewModel>>();
            try
            {
                string? instrumentActivityId = instrumentActivityModel.InstrumentActivityId;
                string? activityName = instrumentActivityModel.ActivityName;
                string? instrumentName = instrumentActivityModel.InstrumentName;
                string? requirementName = instrumentActivityModel.RequirementName;
                string? departmentName = instrumentActivityModel.DepartmentName;
                int pageNumber = instrumentActivityModel.pageNumber;
                string? updatedBy = instrumentActivityModel.UpdatedBy;
                string? status = instrumentActivityModel.Status;
                DateTime? updatedDate = instrumentActivityModel.UpdatedDate;
                string formattedDate = updatedDate?.ToString("yyyy-MM-dd");

                var instrumentActivity = await activityRepository.GetAllFilterInstrumentActivity(instrumentActivityId,activityName,instrumentName,requirementName,departmentName, pageNumber, updatedBy, status, formattedDate);
                var data = mapper.Map<List<InstrumentActivity>, List<InstrumentActivityViewModel>>(instrumentActivity.Value.GridRecords);
                apiResponse.Success = true;
                apiResponse.Result = data;
                apiResponse.CurrentPage = instrumentActivity.Value.CurrentPage;
                apiResponse.TotalPage = instrumentActivity.Value.TotalPages;
                apiResponse.TotalRecords = instrumentActivity.Value.TotalRecords;
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
        [HttpGet("GetAllInstrumentActivity/{pageNumber}")]
        public async Task<ApiResponse<List<InstrumentActivityViewModel>>> GetAllInstrumentActivity(int pageNumber)
        {
            var apiResponse = new ApiResponse<List<InstrumentActivityViewModel>>();
            try
            {
                var activity = await activityRepository.GetInstrumentActivity(pageNumber);
                var data = mapper.Map<List<InstrumentActivity>, List<InstrumentActivityViewModel>>(activity.Value.GridRecords);
                apiResponse.Success = true;
                apiResponse.Result = data;
                apiResponse.CurrentPage = activity.Value.CurrentPage;
                apiResponse.TotalPage = activity.Value.TotalPages;
                apiResponse.TotalRecords = activity.Value.TotalRecords;
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
        public async void SendMail()
        {
            var toEmails = new List<string>() { "thakkarpooja153@gmail.com" };
            List<InstrumentActivityViewModel> list = new List<InstrumentActivityViewModel>();
            var data = await activityRepository.GetActiveInstrumentActivity();
            foreach (var item in data)
            {
                DateTime lastcommentDate = await activityRepository.GetLastPerformedDateForInstrumentActivity(item.Id);
                int days = item.Days;
                bool beforeAlerts = item.BeforeAlerts;
                var frequencyUnit = item.Frequency;
                int frequencyTime = item.FrequencyTime;
                DateTime nextPerformedDate = lastcommentDate!=default(DateTime)? lastcommentDate:item.CreatedDateTime;
                DateTime beforenextPerformedDate = default;
                DateTime afternextPerformedDate = default;
                lastcommentDate = lastcommentDate != default(DateTime) ? lastcommentDate : item.CreatedDateTime;
                if (frequencyUnit == Core.Entities.FrequencyEnum.FrequencyDay.ToString())
                {
                    nextPerformedDate = lastcommentDate.AddDays(frequencyTime);
                }
                else if (frequencyUnit == Core.Entities.FrequencyEnum.FrequencyMonth.ToString())
                {
                    nextPerformedDate = lastcommentDate.AddMonths(frequencyTime);
                }
                else if (frequencyUnit == Core.Entities.FrequencyEnum.FrequencyYear.ToString())
                {
                    nextPerformedDate = lastcommentDate.AddYears(frequencyTime);
                }
                if (days > 0)
                {
                    beforenextPerformedDate = nextPerformedDate.AddDays(-days);
                    afternextPerformedDate = nextPerformedDate.AddDays(days);
                }
                if (beforeAlerts)
                {
                    int beforeAlertDay = (int)item.BeforeAlertsTime;
                    beforenextPerformedDate = beforenextPerformedDate.AddDays(-beforeAlertDay);
                }
                DateTime startDate = beforenextPerformedDate;
                DateTime endDate = afternextPerformedDate;
                DateTime todayDate = DateTime.UtcNow;
                if (todayDate.Date >= startDate.Date && todayDate.Date <= endDate.Date)
                {
                    if (lastcommentDate.Date <= startDate.Date)
                    {
                         emailSenderService.SendEmail(new MessageServiceModel(toEmails, "HuTech – Today's CheckIn",
                        $"Dear {"Pooja"}, <br /> <br/> Service on your Instrument Activity {item.InstrumentActivityName} need to be done today. " +
                        $" <br /> - <br /> <br /> Regards <br /> <br /> Team: HuTech"));
                        //Console.WriteLine("Mail send");

                    }

                }

            }
            //[HttpGet("SendMail")]
            //public async void SendMail()
            //{
            //    var toEmails = new List<string>() { "thakkarpooja153@gmail.com" };
            //    List<InstrumentActivityViewModel> list = new List<InstrumentActivityViewModel>();
            //    var data = await activityRepository.GetActiveInstrumentActivity();
            //    foreach (var item in data)
            //    {
            //        DateTime lastcommentDate = await activityRepository.GetLastPerformedDateForInstrumentActivity(item.Id);
            //        int days = item.Days;
            //        bool beforeAlerts = item.BeforeAlerts;
            //        int? beforeAlertTime = item.BeforeAlertsTime;

            //        DateTime beforeDate = lastcommentDate!=default(DateTime)?lastcommentDate.AddDays(-days):item.CreatedDateTime.AddDays(-days);
            //        DateTime afterDate = lastcommentDate!=default(DateTime)?lastcommentDate.AddDays(days):item.CreatedDateTime.AddDays(days);
            //        if (beforeAlerts)
            //        {
            //            int beforeAlertDay = (int)item.BeforeAlertsTime;
            //            if (beforeAlertDay != null)
            //            {
            //                beforeDate = beforeDate.AddDays(-beforeAlertDay);
            //            }
            //        }
            //        DateTime todayDate = DateTime.UtcNow;
            //        //if (lastcommentDate.Date<=beforeDate.Date && lastcommentDate.Date >= afterDate.Date)
            //        if (lastcommentDate.Date <= beforeDate.Date)
            //        {
            //            emailSenderService.SendEmail(new MessageServiceModel(toEmails, "HuTech – Today's CheckIn",
            //        $"Dear {"Pooja"}, <br /> <br/> Service on your Instrument Activity {item.InstrumentActivityName} need to be done today. " +
            //        $" <br /> - <br /> <br /> Regards <br /> <br /> Team: HuTech"));
            //        }
            //    }

            //}
        }
    }
}
