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
    public class RequirementController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IRequirementRepository requirementRepository;
        private readonly ILogger<RequirementController> logger;
        public RequirementController(IMapper _mapper, IRequirementRepository _requirementRepository, ILogger<RequirementController> _logger)
        {
            mapper = _mapper;
            requirementRepository = _requirementRepository;
            logger = _logger;
        }
        [HttpPost("PostRequirement")]
        public async Task<ApiResponse<string>> PostRequirement(RequirementViewModel requirementViewModel)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var activitydata = mapper.Map<RequirementViewModel, Requirement>(requirementViewModel);
                bool data = await requirementRepository.PostRequirement(activitydata);
                apiResponse.Result = "Requirement added successfully";
                apiResponse.Success = data;
                return apiResponse;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;
            }
        }
        [HttpGet("GetRequirement")]
        public async Task<ApiResponse<List<RequirementViewModel>>> GetRequirement()
        {
            try
            {
                var apiResponse = new ApiResponse<List<RequirementViewModel>>();
                var requirement = await requirementRepository.GetRequirement();
                var data = mapper.Map<List<Requirement>, List<RequirementViewModel>>(requirement);
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
        [HttpGet("GetRequirementDetail/{id}")]
        public async Task<ApiResponse<RequirementViewModel>> GetRequirementDetail(long id)
        {
            try
            {
                var apiResponse = new ApiResponse<RequirementViewModel>();
                var requirement = await requirementRepository.GetRequirementDetail(id);
                var data = mapper.Map<Requirement, RequirementViewModel>(requirement);
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
        [HttpDelete("DeleteRequirement/{Id}")]
        public async Task<ApiResponse<string>> DeleteRequirement(long Id)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var role = await requirementRepository.DeleteRequirement(Id);
                apiResponse.Success = true;
                apiResponse.Message = "Requirement deleted Successfully";
                return apiResponse;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;
            }
        }
        [HttpPut("PutRequirement")]
        public async Task<ApiResponse<string>> PutRequirement(RequirementViewModel model)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var data = mapper.Map<RequirementViewModel, Requirement>(model);
                var role = await requirementRepository.PutRequirement(data);
                apiResponse.Success = true;
                apiResponse.Message = "Update Requirement Successfully";
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
