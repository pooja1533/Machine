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
    
    public class DepartmentController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IDepartmentRepository departmentRepository;
        private readonly ILogger<DepartmentController> logger;
        public DepartmentController(IMapper _mapper, IDepartmentRepository _departmentRepository, ILogger<DepartmentController> _logger)
        {
            mapper = _mapper;
            departmentRepository = _departmentRepository;
            logger = _logger;
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
                apiResponse.Success = data;
                return apiResponse;
            }
            catch(Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;
            }
        }
        [HttpGet("GetDepartment")]
        public async Task<ApiResponse<List<DepartmentViewModel>>> GetDepartment()
        {
            try
            {
                var apiResponse = new ApiResponse<List<DepartmentViewModel>>();
                var department = await departmentRepository.GetDepartment();
                var data = mapper.Map<List<Department>, List<DepartmentViewModel>>(department);
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
        [HttpGet("GetDepartmentDetail/{id}")]
        public async Task<ApiResponse<DepartmentViewModel>> GetDepartmentDetail(long id)
        {
            try
            {
                var apiResponse = new ApiResponse<DepartmentViewModel>();
                var department = await departmentRepository.GetDepartmentDetail(id);
                var data = mapper.Map<Department, DepartmentViewModel>(department);
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
        [HttpDelete("DeleteDepartment/{Id}")]
        public async Task<ApiResponse<string>> DeleteDepartment(long Id)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var role = await departmentRepository.DeleteDepartment(Id);
                apiResponse.Success = true;
                apiResponse.Message = "department deleted Successfully";
                return apiResponse;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;
            }
        }
        [HttpPut("PutDepartment")]
        public async Task<ApiResponse<string>> PutDepartment(DepartmentViewModel model)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var data = mapper.Map<DepartmentViewModel, Department>(model);
                var role = await departmentRepository.PutDepartment(data);
                apiResponse.Success = true;
                apiResponse.Message = "Update department Successfully";
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
