using AutoMapper;
using Hutech.Application.Interfaces;
using Hutech.Core.Entities;
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
        public RoleController(IRoleRepository _roleRepository, IMapper _mapper, ILogger<RoleController> _logger)
        {
            roleRepository = _roleRepository;
            mapper = _mapper;
            logger = _logger;
        }
        [HttpPost("PostRole")]
        public async Task<ApiResponse<string>> PostRole(RoleViewModel aspNetRole)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var roledata = mapper.Map<RoleViewModel, AspNetRole>(aspNetRole);
                bool data = await roleRepository.AddRole(roledata);
                apiResponse.Result = "Role added successfully";
                apiResponse.Success = data;
                return apiResponse;
            }
            catch(Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;
            }
        }
        [HttpGet("GetRoleAccordingToRole/{Role}")]
        public async Task<ApiResponse<List<RoleViewModel>>> GetRoleAccordingToRole(string role)
        {
            try
            {
                //logger.LogInformation($"Get Roles call at api level {DateTime.Now}");
                var apiResponse = new ApiResponse<List<RoleViewModel>>();
                var roledata = await roleRepository.GetRoleAccordingToRole(role);
                var data = mapper.Map<List<AspNetRole>, List<RoleViewModel>>(roledata);
                apiResponse.Success = true;
                apiResponse.Result = data;
                //logger.LogInformation($"Get Roles call return from  api level {DateTime.Now}");
                return apiResponse;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;
            }
        }
        [HttpGet("GetRoles")]
        public async Task<ApiResponse<List<RoleViewModel>>> GetRoles()
        {
            try
            {
                //logger.LogInformation($"Get Roles call at api level {DateTime.Now}");
                var apiResponse = new ApiResponse<List<RoleViewModel>>();
                var role = await roleRepository.GetAllRoles();
                var data = mapper.Map<List<AspNetRole>, List<RoleViewModel>>(role);
                apiResponse.Success = true;
                apiResponse.Result = data;
                //logger.LogInformation($"Get Roles call return from  api level {DateTime.Now}");
                return apiResponse;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;
            }
        }
        [HttpGet("GetRoleDetail/{id}")]
        public async Task<ApiResponse<RoleViewModel>> GetRoleDetail(Guid id)
        {
            try
            {
                var apiResponse = new ApiResponse<RoleViewModel>();
                var role = await roleRepository.GetRoleDetail(id);
                var data = mapper.Map<AspNetRole, RoleViewModel>(role);
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

        [HttpDelete("DeleteRole/{Id}")]
        public async Task<ApiResponse<string>> DeleteRole(Guid Id)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var role = await roleRepository.DeleteRole(Id);
                apiResponse.Success = true;
                apiResponse.Message = "Delete Role Successfully";
                return apiResponse;
            }
            catch(Exception ex) 
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;
            }
        }
        [HttpPut("PutRole")]
        public async Task<ApiResponse<string>> PutRole(RoleViewModel model)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var data = mapper.Map<RoleViewModel, AspNetRole>(model);
                var role = await roleRepository.UpdateRole(data);
                apiResponse.Success = true;
                apiResponse.Message = "Update Role Successfully";
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
