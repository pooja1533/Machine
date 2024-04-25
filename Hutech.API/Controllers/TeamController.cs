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
        private readonly IAuditRepository auditRepository;
        public TeamController(IMapper _mapper, ITeamRepository _teamRepository, ILogger<TeamController> _logger, IAuditRepository _auditRepository)
        {
            mapper = _mapper;
            teamRepository = _teamRepository;
            logger = _logger;
            auditRepository = _auditRepository;
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
        
        [HttpGet("GetActiveTeam")]
        public async Task<ApiResponse<List<TeamViewModel>>> GetActiveTeam()
        {
            var apiResponse = new ApiResponse<List<TeamViewModel>>();
            try
            {
                var team = await teamRepository.GetActiveTeam();
                var data = mapper.Map<List<Team>, List<TeamViewModel>>(team);
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
        [HttpGet("GetTeam")]
        public async Task<ApiResponse<List<TeamViewModel>>> GetTeam()
        {
            var apiResponse = new ApiResponse<List<TeamViewModel>>();
            try
            {
                var team = await teamRepository.GetTeam();
                var data = mapper.Map<List<Team>, List<TeamViewModel>>(team);
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
        [HttpGet("GetTeamDetail/{id}")]
        public async Task<ApiResponse<TeamViewModel>> GetTeamDetail(long id)
        {
            var apiResponse = new ApiResponse<TeamViewModel>();
            try
            {
                var team = await teamRepository.GetTeamDetail(id);
                var data = mapper.Map<Team, TeamViewModel>(team);
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
        [HttpPut("PutTeam")]
        public async Task<ApiResponse<string>> PutTeam(TeamViewModel model)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var data = mapper.Map<TeamViewModel, Team>(model);
                var role = await teamRepository.UpdateTeam(data);
                apiResponse.Success = true;
                apiResponse.Message = "Update Team Successfully";
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
        [HttpDelete("DeleteTeam/{Id}")]
        public async Task<ApiResponse<string>> DeleteTeam(long Id)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var role = await teamRepository.DeleteTeam(Id);
                apiResponse.Success = true;
                apiResponse.Message = "Delete Team Successfully";
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
        [HttpPost("GetAllFilterTeam")]
        public async Task<ApiResponse<List<TeamViewModel>>> GetAllFilterTeam(TeamModel teamModel)
        {
            var apiResponse = new ApiResponse<List<TeamViewModel>>();
            try
            {
                string? teamName = teamModel.TeamName;
                int pageNumber = teamModel.PageNumber;
                string? updatedBy = teamModel.UpdatedBy;
                string? status = teamModel.Status;
                DateTime? updatedDate = teamModel.UpdatedDate;
                string formattedDate = updatedDate?.ToString("yyyy-MM-dd");
                string? locationName = teamModel.LocationName;
                string? departmentName=teamModel.DepartmentName;
                var team = await teamRepository.GetAllFilterTeam(teamName, pageNumber, updatedBy, status, formattedDate,locationName,departmentName);
                var data = mapper.Map<List<Team>, List<TeamViewModel>>(team.Value.GridRecords);
                apiResponse.Success = true;
                apiResponse.Result = data;
                apiResponse.CurrentPage = team.Value.CurrentPage;
                apiResponse.TotalPage = team.Value.TotalPages;
                apiResponse.TotalRecords = team.Value.TotalRecords;
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
