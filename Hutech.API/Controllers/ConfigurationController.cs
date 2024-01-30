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
        public ConfigurationController(IMapper _mapper, IConfigurationRepository _configurationRepository, ILogger<ConfigurationController> _logger)
        {
            mapper = _mapper;
            configurationRepository = _configurationRepository;
            logger = _logger;
        }

        [HttpGet("GetAllConfiguration")]
        public async Task<ApiResponse<List<ConfigurationViewModel>>> GetAllConfiguration()
        {
            try
            {
                var apiResponse = new ApiResponse<List<ConfigurationViewModel>>();
                var configures = await configurationRepository.GetAllConfiguration();
                var data = mapper.Map<List<Configure>, List<ConfigurationViewModel>>(configures);
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
        [HttpPost("PostConfiguration")]
        public async Task<ApiResponse<string>> PostConfiguration(ConfigurationViewModel configurationViewModel)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var activitydata = mapper.Map<ConfigurationViewModel, Configure>(configurationViewModel);
                bool data = await configurationRepository.PostConfiguration(activitydata);
                apiResponse.Result = "configuration added successfully";
                apiResponse.Success = data;
                return apiResponse;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;
            }
        }
        [HttpGet("GetConfigurationDetail/{id}")]
        public async Task<ApiResponse<ConfigurationViewModel>> GetConfigurationDetail(long id)
        {
            try
            {
                var apiResponse = new ApiResponse<ConfigurationViewModel>();
                var configure = await configurationRepository.GetConfigurationDetail(id);
                var data = mapper.Map<Configure, ConfigurationViewModel>(configure);
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
    }
}
