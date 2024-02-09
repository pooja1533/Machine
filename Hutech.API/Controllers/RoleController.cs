using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Hutech.Application.Interfaces;
using Hutech.Core.Entities;
using Hutech.Infrastructure.Repository;
using Hutech.Models;
using Imputabiliteafro.Api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Hutech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository roleRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RoleController> logger;
        private readonly IAuditRepository auditRepository;
        public RoleController(IRoleRepository _roleRepository, IMapper _mapper, ILogger<RoleController> _logger, IAuditRepository _auditRepository)
        {
            roleRepository = _roleRepository;
            mapper = _mapper;
            logger = _logger;
            auditRepository = _auditRepository;
        }
        [HttpPost("PostRole")]
        public async Task<ApiResponse<string>> PostRole(RoleViewModel aspNetRole)
        {
            try
            {
                //string dataa = null;
                //var length = dataa.Length;
                var apiResponse = new ApiResponse<string>();
                var roledata = mapper.Map<RoleViewModel, AspNetRole>(aspNetRole);
                bool data = await roleRepository.AddRole(roledata);
                apiResponse.Result = "Role added successfully";
                apiResponse.Success = true;
                return apiResponse;
            }
            catch(Exception ex)
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
        [HttpGet("GetRoleAccordingToRole/{Role}")]
        public async Task<ApiResponse<List<RoleViewModel>>> GetRoleAccordingToRole(string role)
        {
            var apiResponse = new ApiResponse<List<RoleViewModel>>();
            try
            {
                var roledata = await roleRepository.GetRoleAccordingToRole(role);
                var data = mapper.Map<List<AspNetRole>, List<RoleViewModel>>(roledata);
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
                apiResponse.AuditId = auditId;
                apiResponse.Success = false;
                return apiResponse;
            }
        }
        [HttpGet("GetRoles")]
        public async Task<ApiResponse<List<RoleViewModel>>> GetRoles()
        {
            var apiResponse = new ApiResponse<List<RoleViewModel>>();
            try
            {
                var role = await roleRepository.GetAllRoles();
                var data = mapper.Map<List<AspNetRole>, List<RoleViewModel>>(role);
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
                apiResponse.AuditId = auditId;
                apiResponse.Success = false;
                return apiResponse;
            }
        }
        [HttpGet("GetRoleDetail/{id}")]
        public async Task<ApiResponse<RoleViewModel>> GetRoleDetail(Guid id)
        {
            var apiResponse = new ApiResponse<RoleViewModel>();
            try
            {
                var role = await roleRepository.GetRoleDetail(id);
                var data = mapper.Map<AspNetRole, RoleViewModel>(role);
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
                apiResponse.AuditId = auditId;
                apiResponse.Success = false;
                return apiResponse;
            }
        }

        [HttpDelete("DeleteRole/{Id}")]
        public async Task<ApiResponse<string>> DeleteRole(Guid Id)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var role = await roleRepository.DeleteRole(Id);
                apiResponse.Success = true;
                apiResponse.Message = "Delete Role Successfully";
                return apiResponse;
            }
            catch(Exception ex) 
            {
                var id = RouteData.Values["AuditId"];
                logger.LogInformation($"Exception Occure in API.{ex.Message}" + "{@AuditId}", id);
                long auditId = System.Convert.ToInt64(id);
                auditRepository.AddExceptionDetails(auditId, ex.Message);
                apiResponse.AuditId = auditId;
                apiResponse.Success = false;
                return apiResponse;
            }
        }
        [HttpPut("PutRole")]
        public async Task<ApiResponse<string>> PutRole(RoleViewModel model)
        {
            try
            {
                //string dataa = null;
                //var length = dataa.Length;
                var apiResponse = new ApiResponse<string>();
                var data = mapper.Map<RoleViewModel, AspNetRole>(model);
                var role = await roleRepository.UpdateRole(data);
                apiResponse.Success = true;
                apiResponse.Message = "Update Role Successfully";
                return apiResponse;
            }
            catch (Exception ex)
            {
                var id = RouteData.Values["AuditId"];
                long auditId = System.Convert.ToInt64(id);
                logger.LogInformation($"Exception Occure in API.{ex.Message}" + "{@AuditId}", id);
                var apiResponse = new ApiResponse<string>();
                apiResponse.Success = false;
                apiResponse.Result = id.ToString();
                apiResponse.AuditId = auditId;
                return apiResponse;
            }
        }
    }
}
