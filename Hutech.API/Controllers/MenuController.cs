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
    public class MenuController : ControllerBase
    {
        private readonly ILogger<MenuController> logger;
        private readonly IAuditRepository auditRepository;
        //private readonly AddLog addLog;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
        private readonly IMenuRepository menuRepository;
        public MenuController(ILogger<MenuController> _logger, IHttpContextAccessor _httpContextAccessor, IMapper _mapper, IAuditRepository _auditrepsotory, IMenuRepository _menuRepository)
        {
            logger = _logger;
            //addLog = _addLog;
            httpContextAccessor = _httpContextAccessor;
            mapper = _mapper;
            auditRepository = _auditrepsotory;
            menuRepository = _menuRepository;
        }
        [HttpGet("GetLoggedInUserMenuPermission/{loggedinUserId}")]
        public async Task<ApiResponse<List<MenuViewModel>>> GetLoggedInUserMenuPermission(Guid loggedInUserId)
        {
            var apiResponse = new ApiResponse<List<MenuViewModel>>();
            try
            {
                var menu = await menuRepository.GetLoggedInUserMenuPermission(loggedInUserId);
                var data = mapper.Map<List<Menu>, List<MenuViewModel>>(menu);
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
    }
}
