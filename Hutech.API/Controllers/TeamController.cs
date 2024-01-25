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
    public class TeamController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ITeamRepository teamRepository;
        private readonly ILogger<TeamController> logger;
        public TeamController(IMapper _mapper, ITeamRepository _teamRepository, ILogger<TeamController> _logger)
        {
            mapper = _mapper;
            teamRepository = _teamRepository;
            logger = _logger;
        }
        [HttpPost("PostTeam")]
        public async Task<ApiResponse<string>> PostTeam(TeamViewModel teamViewModel)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var teamdata = mapper.Map<TeamViewModel, Team>(teamViewModel);
                bool data = await teamRepository.PostTeam(teamdata);
                apiResponse.Result = "Location added successfully";
                apiResponse.Success = data;
                return apiResponse;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;
            }
        }
        
        [HttpGet("GetActiveTeam")]
        public async Task<ApiResponse<List<TeamViewModel>>> GetActiveTeam()
        {
            try
            {
                var apiResponse = new ApiResponse<List<TeamViewModel>>();
                var team = await teamRepository.GetActiveTeam();
                var data = mapper.Map<List<Team>, List<TeamViewModel>>(team);
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
        [HttpGet("GetTeam")]
        public async Task<ApiResponse<List<TeamViewModel>>> GetTeam()
        {
            try
            {
                var apiResponse = new ApiResponse<List<TeamViewModel>>();
                var team = await teamRepository.GetTeam();
                var data = mapper.Map<List<Team>, List<TeamViewModel>>(team);
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
        [HttpGet("GetTeamDetail/{id}")]
        public async Task<ApiResponse<TeamViewModel>> GetTeamDetail(long id)
        {
            try
            {
                var apiResponse = new ApiResponse<TeamViewModel>();
                var team = await teamRepository.GetTeamDetail(id);
                var data = mapper.Map<Team, TeamViewModel>(team);
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
        [HttpPut("PutTeam")]
        public async Task<ApiResponse<string>> PutTeam(TeamViewModel model)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var data = mapper.Map<TeamViewModel, Team>(model);
                var role = await teamRepository.UpdateTeam(data);
                apiResponse.Success = true;
                apiResponse.Message = "Update Team Successfully";
                return apiResponse;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;
            }
        }
        [HttpDelete("DeleteTeam/{Id}")]
        public async Task<ApiResponse<string>> DeleteTeam(long Id)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var role = await teamRepository.DeleteTeam(Id);
                apiResponse.Success = true;
                apiResponse.Message = "Delete Team Successfully";
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
