using AutoMapper;
using Hutech.Application.Interfaces;
using Hutech.Core.Entities;
using Hutech.Infrastructure.Repository;
using Hutech.Models;
using Imputabiliteafro.Api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Cryptography;

namespace Hutech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class DepartmentController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IDepartmentRepository departmentRepository;
        private readonly ILogger<DepartmentController> logger;
        private readonly IAuditRepository auditRepository;
        public DepartmentController(IMapper _mapper, IDepartmentRepository _departmentRepository, ILogger<DepartmentController> _logger, IAuditRepository _auditRepository)
        {
            mapper = _mapper;
            departmentRepository = _departmentRepository;
            logger = _logger;
            auditRepository = _auditRepository;
        }
        [HttpPost("PostDepartment")]
        public async Task<ApiResponse<string>> PostDepartment(DepartmentViewModel departmentViewModel)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var departmentdata = mapper.Map<DepartmentViewModel, Department>(departmentViewModel);
                bool data = await departmentRepository.PostDepartment(departmentdata);
                apiResponse.Result = "department added successfully";
                apiResponse.Success = true;
                return apiResponse;
            }
            catch(Exception ex)
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
        
        [HttpGet("GetDepartment")]
        public async Task<ApiResponse<List<DepartmentViewModel>>> GetDepartment()
        {
            var apiResponse = new ApiResponse<List<DepartmentViewModel>>();
            try
            {
                var department = await departmentRepository.GetDepartment();
                var data = mapper.Map<List<Department>, List<DepartmentViewModel>>(department);
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
        [HttpGet("GetActiveDepartment")]
        public async Task<ApiResponse<List<DepartmentViewModel>>> GetActiveDepartment()
        {
            var apiResponse = new ApiResponse<List<DepartmentViewModel>>();
            try
            {
                var department = await departmentRepository.GetActiveDepartment();
                var data = mapper.Map<List<Department>, List<DepartmentViewModel>>(department);
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
        [HttpGet("GetDepartmentDetail/{id}")]
        public async Task<ApiResponse<DepartmentViewModel>> GetDepartmentDetail(long id)
        {
            var apiResponse = new ApiResponse<DepartmentViewModel>();
            try
            {
                var department = await departmentRepository.GetDepartmentDetail(id);
                var data = mapper.Map<Department, DepartmentViewModel>(department);
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
        [HttpDelete("DeleteDepartment/{Id}")]
        public async Task<ApiResponse<string>> DeleteDepartment(long Id)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var role = await departmentRepository.DeleteDepartment(Id);
                apiResponse.Success = true;
                apiResponse.Message = "department deleted Successfully";
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
        [HttpPut("PutDepartment")]
        public async Task<ApiResponse<string>> PutDepartment(DepartmentViewModel model)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var data = mapper.Map<DepartmentViewModel, Department>(model);
                var role = await departmentRepository.PutDepartment(data);
                apiResponse.Success = true;
                apiResponse.Message = "Update department Successfully";
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
