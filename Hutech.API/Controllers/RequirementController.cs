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
        private readonly IAuditRepository auditRepository;
        public RequirementController(IMapper _mapper, IRequirementRepository _requirementRepository, ILogger<RequirementController> _logger,IAuditRepository _auditRepository)
        {
            mapper = _mapper;
            requirementRepository = _requirementRepository;
            logger = _logger;
            auditRepository = _auditRepository;
        }
        [HttpPost("PostRequirement")]
        public async Task<ApiResponse<string>> PostRequirement(RequirementViewModel requirementViewModel)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var activitydata = mapper.Map<RequirementViewModel, Requirement>(requirementViewModel);
                bool data = await requirementRepository.PostRequirement(activitydata);
                apiResponse.Result = "Requirement added successfully";
                apiResponse.Success = true;
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
        
        [HttpGet("GetRequirement")]
        public async Task<ApiResponse<List<RequirementViewModel>>> GetRequirement()
        {
            var apiResponse = new ApiResponse<List<RequirementViewModel>>();
            try
            {
                var requirement = await requirementRepository.GetRequirement();
                var data = mapper.Map<List<Requirement>, List<RequirementViewModel>>(requirement);
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
        [HttpGet("GetActiveRequirement")]
        public async Task<ApiResponse<List<RequirementViewModel>>> GetActiveRequirement()
        {
            var apiResponse = new ApiResponse<List<RequirementViewModel>>();
            try
            {
                var requirement = await requirementRepository.GetActiveRequirement();
                var data = mapper.Map<List<Requirement>, List<RequirementViewModel>>(requirement);
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
        [HttpGet("GetRequirementDetail/{id}")]
        public async Task<ApiResponse<RequirementViewModel>> GetRequirementDetail(long id)
        {
            var apiResponse = new ApiResponse<RequirementViewModel>();
            try
            {
                var requirement = await requirementRepository.GetRequirementDetail(id);
                var data = mapper.Map<Requirement, RequirementViewModel>(requirement);
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
        [HttpDelete("DeleteRequirement/{Id}")]
        public async Task<ApiResponse<string>> DeleteRequirement(long Id)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var role = await requirementRepository.DeleteRequirement(Id);
                apiResponse.Success = true;
                apiResponse.Message = "Requirement deleted Successfully";
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
        [HttpPut("PutRequirement")]
        public async Task<ApiResponse<string>> PutRequirement(RequirementViewModel model)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var data = mapper.Map<RequirementViewModel, Requirement>(model);
                var role = await requirementRepository.PutRequirement(data);
                apiResponse.Success = true;
                apiResponse.Message = "Update Requirement Successfully";
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
