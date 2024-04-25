using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Hutech.Application.Interfaces;
using Hutech.Core.Entities;
using Hutech.Infrastructure.Repository;
using Hutech.Models;
using Imputabiliteafro.Api.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using IdentityUser = Microsoft.AspNetCore.Identity.IdentityUser;

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
        [HttpPost("GetAllFilterUser")]
        public async Task<ApiResponse<List<UserViewModel>>> GetAllFilterUser(UserModal userModal)
        {
            var apiResponse = new ApiResponse<List<UserViewModel>>();
            try
            {
                string? fullName = userModal.FullName;
                int pageNumber = userModal.PageNumber;
                string? userName = userModal.UserName;
                string? status = userModal.status;
                string? email = userModal.EmailId;
                string? loggedInUserId = userModal.LoggedInUserId;
                string? employeeId= userModal.EmployeeId;
                int? userType =System.Convert.ToInt32(userModal.UserType);
                long departmentId = System.Convert.ToInt64(userModal.Department);
                long? locationId = System.Convert.ToInt64(userModal.Location);
                string? roleId = userModal.Role;
                var user = await userRepository.GetAllFilterUser(fullName, pageNumber, userName, status, email,loggedInUserId,employeeId, userType, departmentId,locationId, roleId);
                var data = mapper.Map<List<UserDetail>, List<UserViewModel>>(user.Value.GridRecords);
                apiResponse.Success = true;
                apiResponse.Result = data;
                apiResponse.CurrentPage = user.Value.CurrentPage;
                apiResponse.TotalPage = user.Value.TotalPages;
                apiResponse.TotalRecords = user.Value.TotalRecords;
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
                var data = mapper.Map<List<UserDetail>, List<UserViewModel>>(users);
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
                ApiResponse.Message = "User Deleted Successfully";
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
        public async Task<ApiResponse<UserViewModel>> GetUserById(long Id)
        {
            var ApiResponse = new ApiResponse<UserViewModel>();
            try
            {
                var users = await userRepository.GetUserById(Id);
                var data = mapper.Map<UserDetail, UserViewModel>(users);
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
        [HttpGet("GetUserDetail/{Id}")]
        public async Task<ApiResponse<UserViewModel>> GetUserDetail(long Id)
        {
            var ApiResponse = new ApiResponse<UserViewModel>();
            try
            {
                var users = await userRepository.GetUserDetail(Id);
                var data = mapper.Map<UserDetail, UserViewModel>(users);
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
        [HttpGet("ApproveUser/{UserId}")]
        public async Task<ApiResponse<string>> ApproveUser(long UserId)
        {
            var ApiResponse = new ApiResponse<string>();
            try
            {
                bool result = await userRepository.ApproveUser(UserId);
                ApiResponse.Success = result;
                ApiResponse.Message = "User Approved successfully";
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
        [HttpGet("RejectUser/{comment}/{userId}")]
        public async Task<ApiResponse<string>> RejectUser(string comment,long userId)
        {
            var ApiResponse = new ApiResponse<string>();
            try
            {
                bool result = await userRepository.RejectUser(comment,userId);
                ApiResponse.Success = result;
                ApiResponse.Message = "User Approved successfully";
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
        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
        [HttpPost("PostUser")]
        public async Task<ApiResponse<string>> PostUser(UserViewModel userViewModel)
        {
            try
            {

                var apiResponse = new ApiResponse<string>();
                var userdata = mapper.Map<UserViewModel, UserDetail>(userViewModel);
                bool data = await userRepository.PostUser(userdata);
                apiResponse.Result = "User added successfully";
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
    }
}
