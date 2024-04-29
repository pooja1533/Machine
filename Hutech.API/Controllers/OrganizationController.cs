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
    public class OrganizationController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IOrganizationRepository organizationRepository;
        private readonly ILogger<OrganizationController> logger;
        private readonly IAuditRepository auditRepository;
        public OrganizationController(IMapper _mapper, IOrganizationRepository _organizationRepository, ILogger<OrganizationController> _logger, IAuditRepository _auditRepository)
        {
            mapper = _mapper;
            organizationRepository = _organizationRepository;
            logger = _logger;
            auditRepository = _auditRepository;
        }
        [HttpPost("PostOrganization")]
        public async Task<ApiResponse<string>> PostOrganization(OrganizationViewModel organizationViewModel)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var organizationdata = mapper.Map<OrganizationViewModel, Organization>(organizationViewModel);
                bool data = await organizationRepository.AddOrganization(organizationdata);
                apiResponse.Result = "Organization added successfully";
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
        [HttpGet("GetOrganization/{pageNumber}")]
        public async Task<ApiResponse<List<OrganizationViewModel>>> GetOrganization(int pageNumber)
        {
            var apiResponse = new ApiResponse<List<OrganizationViewModel>>();
            try
            {
                var organization = await organizationRepository.GetOrganization(pageNumber);
                var data = mapper.Map<List<Organization>, List<OrganizationViewModel>>(organization.Value.GridRecords);
                apiResponse.Success = true;
                apiResponse.Result = data;
                apiResponse.CurrentPage = organization.Value.CurrentPage;
                apiResponse.TotalPage = organization.Value.TotalPages;
                apiResponse.TotalRecords = organization.Value.TotalRecords;
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
        [HttpDelete("DeleteOrganization/{Id}")]
        public async Task<ApiResponse<string>> DeleteOrganization(long Id)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var role = await organizationRepository.DeleteOrganization(Id);
                apiResponse.Success = true;
                apiResponse.Message = "Organization Deleted Successfully";
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
        [HttpGet("GetOrganizationDetail/{id}")]
        public async Task<ApiResponse<OrganizationViewModel>> GetOrganizationDetail(long id)
        {
            var apiResponse = new ApiResponse<OrganizationViewModel>();
            try
            {
                var organization = await organizationRepository.GetOrganizationDetail(id);
                var data = mapper.Map<Organization, OrganizationViewModel>(organization);
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
