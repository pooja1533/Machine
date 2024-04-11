using AutoMapper;
using Hutech.API.Helpers;
using Hutech.API.Model;
using Hutech.Application.Interfaces;
using Hutech.Core.Entities;
using Hutech.Infrastructure.Repository;
using Hutech.Models;
using Imputabiliteafro.Api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Globalization;
using System.Security.AccessControl;
using System.Text;
using AuditModel = Hutech.Core.Entities.AuditModels;

namespace Hutech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IActivityRepository activityRepository;
        private readonly ILogger<ActivityController> logger;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuditRepository _auditRepository;
        public ActivityController(IMapper _mapper, IActivityRepository _activityRepository, ILogger<ActivityController> _logger, IHttpContextAccessor httpContextAccessor,IAuditRepository auditRepository)
        {
            mapper = _mapper;
            activityRepository = _activityRepository;
            logger = _logger;
            _auditRepository = auditRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        private void AuditActivity()
        {
            var objaudit = new AuditModel();
            //objaudit.RoleId = Convert.ToString(HttpContext.Session.GetInt32(AllSessionKeys.RoleId));
            objaudit.ModuleName = "Portal";
            objaudit.ActionName = "Login";
            objaudit.Area = "";
            objaudit.LoggedInAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            //if (_httpContextAccessor.HttpContext != null)
            //    objaudit.IpAddress = Convert.ToString(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            //objaudit.UserId = Convert.ToString(HttpContext.Session.GetInt32(AllSessionKeys.UserId));
            objaudit.PageAccessed = "";
            objaudit.UrlReferrer = "";
            //objaudit.SessionId = HttpContext.Session.Id;
            //_auditRepository.InsertAuditLogs(objaudit);
        }
        [HttpPost("PostActivity")]
        public async Task<ApiResponse<string>> PostActivity(ActivityViewModel activityViewModel)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var activitydata = mapper.Map<ActivityViewModel, Activity>(activityViewModel);
                bool data = await activityRepository.PostActivity(activitydata);
                //AuditActivity();
                apiResponse.Result = "activity added successfully";
                apiResponse.Success = true;
                return apiResponse;
            }
            catch (Exception ex)
            {
                var id = RouteData.Values["AuditId"];
                logger.LogInformation($"Exception Occure in API.{ex.Message}" + "{@AuditId}", id);
                long auditId = System.Convert.ToInt64(id);
                _auditRepository.AddExceptionDetails(auditId, ex.Message);
                var apiResponse = new ApiResponse<string>();
                apiResponse.Success = false;
                apiResponse.AuditId = auditId;
                return apiResponse;
            }
        }

        [HttpGet("GetActivity/{pageNumber}")]
        public async Task<ApiResponse<List<ActivityViewModel>>> GetActivity(int pageNumber)
        {
            var apiResponse = new ApiResponse<List<ActivityViewModel>>();
            try
            {
                var activity = await activityRepository.GetActivity(pageNumber);
                var data = mapper.Map<List<Activity>, List<ActivityViewModel>>(activity.Value.GridRecords);
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
                _auditRepository.AddExceptionDetails(auditId, ex.Message);
                apiResponse.Success = false;
                apiResponse.AuditId = auditId;
                return apiResponse;
            }
        }
        [HttpGet("GetActiveActivity")]
        public async Task<ApiResponse<List<ActivityViewModel>>> GetActiveActivity()
        {
            var apiResponse = new ApiResponse<List<ActivityViewModel>>();
            try
            {
                var activity = await activityRepository.GetActiveActivity();
                var data = mapper.Map<List<Activity>, List<ActivityViewModel>>(activity);
                apiResponse.Success = true;
                apiResponse.Result = data;
                return apiResponse;
            }
            catch (Exception ex)
            {
                var id = RouteData.Values["AuditId"];
                logger.LogInformation($"Exception Occure in API.{ex.Message}" + "{@AuditId}", id);
                long auditId = System.Convert.ToInt64(id);
                _auditRepository.AddExceptionDetails(auditId, ex.Message);
                apiResponse.Success = false;
                apiResponse.AuditId = auditId;
                return apiResponse;
            }
        }
        [HttpGet("GetActivityDetail/{id}")]
        public async Task<ApiResponse<ActivityViewModel>> GetActivityDetail(long id)
        {
            var apiResponse = new ApiResponse<ActivityViewModel>();
            try
            {
                var activity = await activityRepository.GetActivityDetail(id);
                var data = mapper.Map<Activity, ActivityViewModel>(activity);
                apiResponse.Success = true;
                apiResponse.Result = data;
                return apiResponse;
            }
            catch (Exception ex)
            {
                var Id = RouteData.Values["AuditId"];
                logger.LogInformation($"Exception Occure in API.{ex.Message}" + "{@AuditId}", Id);
                long auditId = System.Convert.ToInt64(Id);
                _auditRepository.AddExceptionDetails(auditId, ex.Message);
                apiResponse.Success = false;
                apiResponse.AuditId = auditId;
                return apiResponse;
            }
        }
        [HttpDelete("DeleteActivity/{Id}")]
        public async Task<ApiResponse<string>> DeleteActivity(long Id)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var role = await activityRepository.DeleteActivity(Id);
                apiResponse.Success = true;
                apiResponse.Message = "Activity deleted Successfully";
                return apiResponse;
            }
            catch (Exception ex)
            {
                var id = RouteData.Values["AuditId"];
                logger.LogInformation($"Exception Occure in API.{ex.Message}" + "{@AuditId}", id);
                long auditId = System.Convert.ToInt64(id);
                _auditRepository.AddExceptionDetails(auditId, ex.Message);
                apiResponse.Success = false;
                apiResponse.AuditId = auditId;
                return apiResponse;
            }
        }
        [HttpPut("PutActivity")]
        public async Task<ApiResponse<string>> PutActivity(ActivityViewModel model)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var data = mapper.Map<ActivityViewModel, Activity>(model);
                var role = await activityRepository.PutActivity(data);
                apiResponse.Success = true;
                apiResponse.Message = "Update Activity Successfully";
                return apiResponse;
            }
            catch (Exception ex)
            {
                var id = RouteData.Values["AuditId"];
                logger.LogInformation($"Exception Occure in API.{ex.Message}" + "{@AuditId}", id);
                long auditId = System.Convert.ToInt64(id);
                _auditRepository.AddExceptionDetails(auditId, ex.Message);
                apiResponse.Success = false;
                apiResponse.AuditId = auditId;
                return apiResponse;
            }
        }

        [HttpPost("GetAllFilterActivity")]
        public async Task<ApiResponse<List<ActivityViewModel>>> GetAllFilterActivity(ActivityModel activityModel)
        {
            var apiResponse = new ApiResponse<List<ActivityViewModel>>();
            try
            {
                string? activityName = activityModel.ActivityName;
                int pageNumber = activityModel.PageNumber;
                string? updatedBy = activityModel.UpdatedBy;
                string? status = activityModel.Status;
                DateTime? updatedDate = activityModel.UpdatedDate;
                string formattedDate = updatedDate?.ToString("yyyy-MM-dd");

                var activities = await activityRepository.GetAllFilterActivity(activityName, pageNumber, updatedBy, status, formattedDate);
                var data = mapper.Map<List<Activity>, List<ActivityViewModel>>(activities.Value.GridRecords);
                apiResponse.Success = true;
                apiResponse.Result = data;
                apiResponse.CurrentPage = activities.Value.CurrentPage;
                apiResponse.TotalPage = activities.Value.TotalPages;
                apiResponse.TotalRecords = activities.Value.TotalRecords;
                return apiResponse;
            }
            catch (Exception ex)
            {
                var id = RouteData.Values["AuditId"];
                logger.LogInformation($"Exception Occure in API.{ex.Message}" + "{@AuditId}", id);
                long auditId = System.Convert.ToInt64(id);
                _auditRepository.AddExceptionDetails(auditId, ex.Message);
                apiResponse.Success = false;
                apiResponse.AuditId = auditId;
                return apiResponse;
            }

        }
    }
}
