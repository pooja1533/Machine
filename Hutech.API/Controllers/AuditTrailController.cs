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
    public class AuditTrailController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IAuditTrailRepository auditTrailRepository;
        private readonly ILogger<AuditTrailController> logger;
        public AuditTrailController(IMapper _mapper, IAuditTrailRepository _auditTrailRepository, ILogger<AuditTrailController> _logger)
        {
            mapper = _mapper;
            auditTrailRepository = _auditTrailRepository;
            logger = _logger;
        }
        [SkipAuditFilterAttribute]
        [HttpGet("GetAuditTrail/{startDate}/{endDate}/{keyword}/{pageNumber}")]
        public async Task<ApiResponse<List<AuditViewModel>>> GetAuditTrail(string startDate, string endDate,string keyword,int pageNumber)
        {
            try
            {
                var apiResponse = new ApiResponse<List<AuditViewModel>>();
                if(keyword== "null")
                    keyword=string.Empty;
                else
                    keyword = keyword + "%";
                var activity = await auditTrailRepository.GetAuditTrail(startDate, endDate,keyword,pageNumber);
                var data = mapper.Map<List<Audit>, List<AuditViewModel>>(activity.Value.GridRecords);
                apiResponse.Success = true;
                apiResponse.CurrentPage = activity.Value.CurrentPage;
                apiResponse.TotalPage = activity.Value.TotalPages;
                apiResponse.TotalRecords = activity.Value.TotalRecords;
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
