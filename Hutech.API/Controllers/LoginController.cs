using AutoMapper;
using Hutech.API.Helpers;
using Hutech.API.Model;
using Hutech.Application.Interfaces;
using Hutech.Core.Entities;
using Hutech.Infrastructure.Repository;
using Hutech.Models;
using Imputabiliteafro.Api.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web.Http.Filters;

namespace Hutech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> logger;
        private readonly IAuditRepository auditRepository;
        //private readonly AddLog addLog;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
        public LoginController(ILogger<LoginController> _logger, IHttpContextAccessor _httpContextAccessor, IMapper _mapper, IAuditRepository _auditrepsotory)
        {
            logger = _logger;
            //addLog = _addLog;
            httpContextAccessor = _httpContextAccessor;
            mapper = _mapper;
            auditRepository = _auditrepsotory;
        }
        [HttpGet("Login/{id}"), Authorize]
        public async Task<ApiResponse<bool>> Login(string id)
        {
            try
            {
                var apiResponse = new ApiResponse<bool>();
                //AuditModel objaudit = new AuditModel();
                //objaudit.ControllerName = "Login";
                //objaudit.ActionName = "Login";
                //objaudit.Area = "";
                //objaudit.LoggedInAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                //objaudit.UserId = httpContextAccessor.HttpContext.Session.GetString("UserId");
                //objaudit.PageAccessed = Convert.ToString(httpContextAccessor.HttpContext.Request.Path); // URL User Requested ;
                //objaudit.UrlReferrer = "";
                //objaudit.Message = "User LoggedIn";
                //objaudit.IpAddress = Convert.ToString(httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
                //objaudit.IsFirstLogin = "";
                //objaudit.LangId = "";
                //objaudit.RoleId = "";
                //objaudit.SessionId = "";
                //objaudit.LoggedOutAt = "";
                //var activitydata = mapper.Map<AuditModel, AuditModels>(objaudit);
                //var addLog = new AddLog();
                //auditRepository.InsertAuditLogs(activitydata);
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
