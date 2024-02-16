using AutoMapper;
using Hutech.Application.Interfaces;
using Hutech.Core.Entities;
using Hutech.Infrastructure.Repository;
using Hutech.Models;
using Imputabiliteafro.Api.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hutech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly ILogger<UserController> logger;
        private readonly IAuditRepository auditRepository;
        public UserController(IUserRepository _userRepository, IMapper _mapper, ILogger<UserController> _logger, IAuditRepository _auditRepository)
        {
            userRepository = _userRepository;
            mapper = _mapper;
            logger = _logger;
            auditRepository= _auditRepository;
        }
        [HttpGet("GetUsers")]
        public async Task<ApiResponse<List<UserViewModel>>> GetUsers()
        {
            try
            {
                logger.LogInformation($"API call For get all Users {DateTime.Now}");
                var ApiResponse = new ApiResponse<List<UserViewModel>>();

                var users = await userRepository.GetUsers();
                var data = mapper.Map<List<AspNetUsers>, List<UserViewModel>>(users);
                ApiResponse.Success = true;
                ApiResponse.Result = data;
                logger.LogInformation($"API End for get all Users {DateTime.Now}");
                return ApiResponse;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;
            }
        }
        [HttpGet("GetAllusers/{UserRole}/{UserId}")]
        public async Task<ApiResponse<List<UserViewModel>>> GetAllusers(string UserRole,string UserId)
        {
            var ApiResponse = new ApiResponse<List<UserViewModel>>();
            try
            {
                logger.LogInformation($"API call For get all Users {DateTime.Now}");
                var users = await userRepository.GetAllUSers(UserRole, UserId);
                var data = mapper.Map<List<AspNetUsers>, List<UserViewModel>>(users);
                ApiResponse.Success = true;
                ApiResponse.Result = data;
                return ApiResponse;
            }
            catch(Exception ex)
            {
                var id = RouteData.Values["AuditId"];
                logger.LogInformation($"Exception Occure in API.{ex.Message}" + "{@AuditId}", id);
                long auditId = System.Convert.ToInt64(id);
                auditRepository.AddExceptionDetails(auditId, ex.Message);
                ApiResponse.Success = false;
                ApiResponse.AuditId = auditId;
                return ApiResponse;
            }
        }
        [HttpDelete("DeleteUser/{Id}")]
        public async Task<ApiResponse<string>> DeleteUser(string Id)
        {
            var ApiResponse = new ApiResponse<string>();
            try
            {
                logger.LogInformation($"API call For delete User {DateTime.Now}");
                var users = await userRepository.DeleteUser(Id);
                ApiResponse.Success = true;
                ApiResponse.Result = "User Deleted Successfully";
                logger.LogInformation($"API End for delete user {DateTime.Now}");
                return ApiResponse;
            }
            catch (Exception ex)
            {
                var id = RouteData.Values["AuditId"];
                logger.LogInformation($"Exception Occure in API.{ex.Message}" + "{@AuditId}", id);
                long auditId = System.Convert.ToInt64(id);
                auditRepository.AddExceptionDetails(auditId, ex.Message);
                ApiResponse.Success = false;
                ApiResponse.AuditId = auditId;
                return ApiResponse;
            }
        }
        [HttpGet("GetUserById/{Id}")]
        public async Task<ApiResponse<UserViewModel>> GetUserById(string Id)
        {
            var ApiResponse = new ApiResponse<UserViewModel>();
            try
            {
                var users = await userRepository.GetUserById(Id);
                var data = mapper.Map<AspNetUsers, UserViewModel>(users);
                ApiResponse.Success = true;
                ApiResponse.Result = data;
                return ApiResponse;
            }
            catch (Exception ex)
            {
                var id = RouteData.Values["AuditId"];
                logger.LogInformation($"Exception Occure in API.{ex.Message}" + "{@AuditId}", id);
                long auditId = System.Convert.ToInt64(id);
                auditRepository.AddExceptionDetails(auditId, ex.Message);
                ApiResponse.Success = false;
                ApiResponse.AuditId = auditId;
                return ApiResponse;
            }
        }
        [HttpPut("UpdateUser")]
        public async Task<ApiResponse<string>> UpdateUser(UserViewModel userViewModel)
        {
            var ApiResponse = new ApiResponse<string>();
            try
            {
                var data = mapper.Map<UserViewModel, AspNetUsers>(userViewModel);
                var users = await userRepository.UpdateUser(data);
                ApiResponse.Success = true;
                ApiResponse.Message = "User updated successfully";
                return ApiResponse;

            }
            catch (Exception ex)
            {
                var id = RouteData.Values["AuditId"];
                logger.LogInformation($"Exception Occure in API.{ex.Message}" + "{@AuditId}", id);
                long auditId = System.Convert.ToInt64(id);
                auditRepository.AddExceptionDetails(auditId, ex.Message);
                ApiResponse.Success = false;
                ApiResponse.AuditId = auditId;
                return ApiResponse;
            }
        }
    }
}
