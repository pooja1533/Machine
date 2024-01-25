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
    public class GroupController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IGroupRepository groupRepository;
        private readonly ILogger<GroupController> logger;
        public GroupController(IMapper _mapper, IGroupRepository _groupRepository, ILogger<GroupController> _logger)
        {
            mapper = _mapper;
            groupRepository = _groupRepository;
            logger = _logger;
        }
        [HttpPost("PostGroup")]
        public async Task<ApiResponse<string>> PostGroup(GroupViewModel groupViewModel)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var activitydata = mapper.Map<GroupViewModel, Group>(groupViewModel);
                bool data = await groupRepository.PostGroup(activitydata);
                apiResponse.Result = "group added successfully";
                apiResponse.Success = data;
                return apiResponse;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;
            }
        }
        
        [HttpGet("GetAllGroup")]
        public async Task<ApiResponse<List<GroupViewModel>>> GetAllGroup()
        {
            try
            {
                var apiResponse = new ApiResponse<List<GroupViewModel>>();
                var group = await groupRepository.GetGroup();
                var data = mapper.Map<List<Group>, List<GroupViewModel>>(group);
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
        [HttpGet("GetAllActiveGroup")]
        public async Task<ApiResponse<List<GroupViewModel>>> GetAllActiveGroup()
        {
            try
            {
                var apiResponse = new ApiResponse<List<GroupViewModel>>();
                var group = await groupRepository.GetAllActiveGroup();
                var data = mapper.Map<List<Group>, List<GroupViewModel>>(group);
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
        [HttpGet("GetGroupDetail/{id}")]
        public async Task<ApiResponse<GroupViewModel>> GetGroupDetail(long id)
        {
            try
            {
                var apiResponse = new ApiResponse<GroupViewModel>();
                var group = await groupRepository.GetGroupDetail(id);
                var data = mapper.Map<Group, GroupViewModel>(group);
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
        [HttpDelete("DeleteGroup/{Id}")]
        public async Task<ApiResponse<string>> DeleteGroup(long Id)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var role = await groupRepository.DeleteGroup(Id);
                apiResponse.Success = true;
                apiResponse.Message = "Group deleted Successfully";
                return apiResponse;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;
            }
        }
        [HttpPut("PutGroup")]
        public async Task<ApiResponse<string>> PutGroup(GroupViewModel model)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var data = mapper.Map<GroupViewModel, Group>(model);
                var role = await groupRepository.PutGroup(data);
                apiResponse.Success = true;
                apiResponse.Message = "Update Group Successfully";
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
