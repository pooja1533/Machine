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
        [HttpGet("GetActiveActivity")]
        public async Task<ApiResponse<List<ActivityViewModel>>> GetActiveActivity()
        {
            try
            {
                var apiResponse = new ApiResponse<List<ActivityViewModel>>();
                var activity = await activityRepository.GetActiveActivity();
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
