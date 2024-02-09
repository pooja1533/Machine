using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Hutech.Application.Interfaces;
using Hutech.Core.Entities;
using Hutech.Infrastructure.Repository;
using Hutech.Models;
using Imputabiliteafro.Api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace Hutech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IGroupRepository groupRepository;
        private readonly ILogger<GroupController> logger;
        private readonly IAuditRepository auditRepository;
        public GroupController(IMapper _mapper, IGroupRepository _groupRepository, ILogger<GroupController> _logger, IAuditRepository _auditRepository)
        {
            mapper = _mapper;
            groupRepository = _groupRepository;
            logger = _logger;
            auditRepository = _auditRepository;
        }
        [HttpPost("PostGroup")]
        public async Task<ApiResponse<string>> PostGroup(GroupViewModel groupViewModel)
        {
            try
            {
                //string dataa = null;
                //var length = dataa.Length;
                var apiResponse = new ApiResponse<string>();
                var activitydata = mapper.Map<GroupViewModel, Group>(groupViewModel);
                bool data = await groupRepository.PostGroup(activitydata);
                apiResponse.Result = "group added successfully";
                apiResponse.Success = true;
                return apiResponse;
            }
            catch (Exception ex)
            {
                var id = RouteData.Values["AuditId"];
                logger.LogInformation($"Exception Occure in API.{ex.Message}"+"{@AuditId}",id);
                long auditId=System.Convert.ToInt64(id);
                auditRepository.AddExceptionDetails(auditId,ex.Message);
                var apiResponse = new ApiResponse<string>();
                apiResponse.Success = false;
                apiResponse.AuditId = auditId;
                return apiResponse;
            }
        }
        
        [HttpGet("GetAllGroup")]
        public async Task<ApiResponse<List<GroupViewModel>>> GetAllGroup()
        {
            var apiResponse = new ApiResponse<List<GroupViewModel>>();
            try
            {
                var group = await groupRepository.GetGroup();
                var data = mapper.Map<List<Group>, List<GroupViewModel>>(group);
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
        [HttpGet("GetAllActiveGroup")]
        public async Task<ApiResponse<List<GroupViewModel>>> GetAllActiveGroup()
        {
            var apiResponse = new ApiResponse<List<GroupViewModel>>();
            try
            {
                var group = await groupRepository.GetAllActiveGroup();
                var data = mapper.Map<List<Group>, List<GroupViewModel>>(group);
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
        [HttpGet("GetGroupDetail/{id}")]
        public async Task<ApiResponse<GroupViewModel>> GetGroupDetail(long id)
        {
            var apiResponse = new ApiResponse<GroupViewModel>();
            try
            {
                var group = await groupRepository.GetGroupDetail(id);
                var data = mapper.Map<Group, GroupViewModel>(group);
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
        [HttpDelete("DeleteGroup/{Id}")]
        public async Task<ApiResponse<string>> DeleteGroup(long Id)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var role = await groupRepository.DeleteGroup(Id);
                apiResponse.Success = true;
                apiResponse.Message = "Group deleted Successfully";
                return apiResponse;
            }
            catch (Exception ex)
            {
                var id = RouteData.Values["AuditId"];
                logger.LogInformation($"Exception Occure in API.{ex.Message}" + "{@AuditId}", id);
                long auditId = System.Convert.ToInt64(id);
                auditRepository.AddExceptionDetails(auditId, ex.Message);
                apiResponse.Success = false;
                apiResponse.Result = id.ToString();
                apiResponse.AuditId = auditId;
                return apiResponse;
            }
        }
        [HttpPut("PutGroup")]
        public async Task<ApiResponse<string>> PutGroup(GroupViewModel model)
        {
            try
            {
                //string dataa = null;
                //var length = dataa.Length;
                var apiResponse = new ApiResponse<string>();
                var data = mapper.Map<GroupViewModel, Group>(model);
                var role = await groupRepository.PutGroup(data);
                apiResponse.Success = true;
                apiResponse.Message = "Update Group Successfully";
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
                apiResponse.Result = id.ToString();
                apiResponse.AuditId= auditId;
                //throw new Exception(apiResponse.Result);
                return apiResponse;
            }
        }
    }
}
