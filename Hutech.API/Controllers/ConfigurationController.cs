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
    public class ConfigurationController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IConfigurationRepository configurationRepository;
        private readonly ILogger<ConfigurationController> logger;
        private readonly IAuditRepository auditRepository;
        public ConfigurationController(IMapper _mapper, IConfigurationRepository _configurationRepository, ILogger<ConfigurationController> _logger, IAuditRepository _auditRepository)
        {
            mapper = _mapper;
            configurationRepository = _configurationRepository;
            logger = _logger;
            auditRepository = _auditRepository;
        }

        [HttpGet("GetAllConfiguration")]
        public async Task<ApiResponse<List<ConfigurationViewModel>>> GetAllConfiguration()
        {
            var apiResponse = new ApiResponse<List<ConfigurationViewModel>>();
            try
            {
                var configures = await configurationRepository.GetAllConfiguration();
                var data = mapper.Map<List<Configure>, List<ConfigurationViewModel>>(configures);
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
        [HttpPost("PostConfiguration")]
        public async Task<ApiResponse<string>> PostConfiguration(ConfigurationViewModel configurationViewModel)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                //string dataa = null;
                //var length = dataa.Length;
                var activitydata = mapper.Map<ConfigurationViewModel, Configure>(configurationViewModel);
                bool data = await configurationRepository.PostConfiguration(activitydata);
                apiResponse.Result = "configuration added successfully";
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
        [HttpGet("GetConfigurationDetail/{id}")]
        public async Task<ApiResponse<ConfigurationViewModel>> GetConfigurationDetail(long id)
        {
            var apiResponse = new ApiResponse<ConfigurationViewModel>();
            try
            {
                var configure = await configurationRepository.GetConfigurationDetail(id);
                var data = mapper.Map<Configure, ConfigurationViewModel>(configure);
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
