using AutoMapper;
using Hutech.Application.Interfaces;
using Hutech.Core.Entities;
using Hutech.Models;
using Imputabiliteafro.Api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.PortableExecutable;

namespace Hutech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IMachineRepository machineRepository;
        private readonly ILogger<MachineController> logger;
        public MachineController(IMapper _mapper, IMachineRepository _machineRepository, ILogger<MachineController> _logger)
        {
            mapper = _mapper;
            machineRepository = _machineRepository;
            logger = _logger;
        }
        [HttpPost("PostComment")]
        public async Task<ApiResponse<string>> PostComment(MachineCommentViewModel machineCommentViewModel)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var machinedata = mapper.Map<MachineCommentViewModel, MachineComment>(machineCommentViewModel);
                bool result = await machineRepository.PostComment(machinedata);
                apiResponse.Success = result;
                apiResponse.Message = "Machine added successfully";
                return apiResponse;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;

            }
        }
        [HttpPost("PostMachine")]
        public async Task<ApiResponse<string>> PostMachine(MachineViewModel machine)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var machinedata = mapper.Map<MachineViewModel, MachineDetail>(machine);
                bool result = await machineRepository.PostMachine(machinedata);
                apiResponse.Success = result;
                apiResponse.Message = "Machine added successfully";
                return apiResponse;
            }
            catch (Exception ex) {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;

            }
        }
        [HttpGet("GetMachine")]
        public async Task<ApiResponse<List<MachineViewModel>>> GetMachine()
        {
            try
            {
                var apiResponse = new ApiResponse<List<MachineViewModel>>();
                var machines = await machineRepository.GetMachine();
                var data = mapper.Map<List<MachineDetail>, List<MachineViewModel>>(machines);
                apiResponse.Result = data;
                apiResponse.Message = "Get Machines";
                return apiResponse;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;

            }
        }
        [HttpDelete("DeleteMachine/{Id}")]
        public async Task<ApiResponse<string>> DeleteMachine(long Id)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var machines = await machineRepository.DeleteMachine(Id);
                apiResponse.Message = "delete machine successfully";
                apiResponse.Success = true;
                return apiResponse;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;

            }
        }
        [HttpGet("GetMachineById/{Id}")]
        public async Task<ApiResponse<MachineViewModel>> GetMachineById(long Id)
        {
            try
            {
                var apiResponse = new ApiResponse<MachineViewModel>();
                var machines = await machineRepository.GetMachineById(Id);
                var data = mapper.Map<MachineDetail,MachineViewModel>(machines);
                apiResponse.Result = data;
                apiResponse.Message = "Get Machines";
                return apiResponse;
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure in API.{ex.Message}");
                throw ex;

            }
        }
        [HttpPut("UpdateMachine")]
        public async Task<ApiResponse<MachineViewModel>> UpdateMachine(MachineViewModel machineDetail)
        {
            try
            {
                var apiResponse = new ApiResponse<MachineViewModel>();
                var data = mapper.Map<MachineViewModel, MachineDetail>(machineDetail);
                var machines = await machineRepository.UpdateMachine(data);
                apiResponse.Message = "Machine Updated Successfully";
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
