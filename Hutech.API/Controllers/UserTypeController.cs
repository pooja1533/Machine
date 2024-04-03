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
    public class UserTypeController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUserTypeRepository userTypeRepository;
        private readonly ILogger<UserTypeController> logger;
        private readonly IAuditRepository auditRepository;
        public UserTypeController(IMapper _mapper, IUserTypeRepository _userTypeRepository, ILogger<UserTypeController> _logger, IAuditRepository _auditRepository)
        {
            mapper = _mapper;
            userTypeRepository = _userTypeRepository;
            logger = _logger;
            auditRepository = _auditRepository;
        }
        [HttpPost("PostUserType")]
        public async Task<ApiResponse<string>> PostUserType(UserTypeViewModel userTypeViewModel)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var userTypedata = mapper.Map<UserTypeViewModel, UserType>(userTypeViewModel);
                bool data = await userTypeRepository.PostUserType(userTypedata);
                apiResponse.Result = "User Type added successfully";
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
        [HttpGet("GetActiveUserType")]
        public async Task<ApiResponse<List<UserTypeViewModel>>> GetActiveUserType()
        {
            var apiResponse = new ApiResponse<List<UserTypeViewModel>>();
            try
            {
                var userType = await userTypeRepository.GetActiveUserType();
                var data = mapper.Map<List<UserType>, List<UserTypeViewModel>>(userType);
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
        [HttpGet("GetUserType/{pageNumber}")]
        public async Task<ApiResponse<List<UserTypeViewModel>>> GetUserType(int pageNumber)
        {
            var apiResponse = new ApiResponse<List<UserTypeViewModel>>();
            try
            {
                var userType = await userTypeRepository.GetUserType(pageNumber);
                var data = mapper.Map<List<UserType>, List<UserTypeViewModel>>(userType.Value.GridRecords);
                apiResponse.Success = true;
                apiResponse.Result = data;
                apiResponse.CurrentPage = userType.Value.CurrentPage;
                apiResponse.TotalPage = userType.Value.TotalPages;
                apiResponse.TotalRecords = userType.Value.TotalRecords;
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
        [HttpDelete("DeleteUserType/{Id}")]
        public async Task<ApiResponse<string>> DeleteUserType(long Id)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var role = await userTypeRepository.DeleteUserType(Id);
                apiResponse.Success = true;
                apiResponse.Message = "User Type deleted Successfully";
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
        [HttpGet("GetUserTypeDetail/{id}")]
        public async Task<ApiResponse<UserTypeViewModel>> GetUserTypeDetail(long id)
        {
            var apiResponse = new ApiResponse<UserTypeViewModel>();
            try
            {
                var usertype = await userTypeRepository.GetUserTypeDetail(id);
                var data = mapper.Map<UserType, UserTypeViewModel>(usertype);
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
        
    }
}
