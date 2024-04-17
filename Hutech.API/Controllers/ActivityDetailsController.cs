using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Hutech.Application.Interfaces;
using Hutech.Core.Entities;
using Hutech.Infrastructure.Repository;
using Hutech.Models;
using Imputabiliteafro.Api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Diagnostics.Metrics;

namespace Hutech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityDetailsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IActivityDetailRepository activityDetailRepository;
        private readonly ILogger<ActivityDetailsController> logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IDocumentRepository documentRepository;
        private readonly IConfiguration configuration;
        private readonly IAuditRepository auditRepository;
        public ActivityDetailsController(IMapper _mapper, IActivityDetailRepository _activityDetailRepository, ILogger<ActivityDetailsController> _logger, IHttpContextAccessor _httpContextAccessor, IConfiguration _configuration, IDocumentRepository _documentRepository, IAuditRepository _auditRepository)
        {
            mapper = _mapper;
            activityDetailRepository = _activityDetailRepository;
            logger = _logger;
            httpContextAccessor = _httpContextAccessor;
            configuration = _configuration;
            documentRepository = _documentRepository;
            auditRepository = _auditRepository;
        }
        [HttpPost("GetAllFilterActivityDetail")]
        public async Task<ApiResponse<List<ActivityDetailsViewModel>>> GetAllFilterActivityDetail(ActivityDetailModel activityModel)
        {
            var apiResponse = new ApiResponse<List<ActivityDetailsViewModel>>();
            try
            {
                string? instrumentIdName = activityModel.InstrumentIdName;
                string? instrumentName=activityModel.InstrumentName;
                string? instrumentSerial = activityModel.InstrumentSerial;
                string? model=activityModel.Model;
                string? location=activityModel.Location;
                string? department=activityModel.Department;
                int pageNumber = activityModel.PageNumber;
                string? updatedBy = activityModel.UpdatedBy;
                string? status = activityModel.Status;
                DateTime? updatedDate = activityModel.UpdatedDate;
                string formattedDate = updatedDate?.ToString("yyyy-MM-dd");
                string LoggedInUser = activityModel.UserId;

                var activityDetail = await activityDetailRepository.GetAllFilterActivityDetail(instrumentIdName,instrumentName,instrumentSerial,model,location,department, pageNumber, updatedBy, status, formattedDate, LoggedInUser);
                var data = mapper.Map<List<ActivityDetails>, List<ActivityDetailsViewModel>>(activityDetail.Value.GridRecords);
                apiResponse.Success = true;
                apiResponse.Result = data;
                apiResponse.CurrentPage = activityDetail.Value.CurrentPage;
                apiResponse.TotalPage = activityDetail.Value.TotalPages;
                apiResponse.TotalRecords = activityDetail.Value.TotalRecords;
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
        [HttpGet("GetAllActivityDetails/{userId}/{pageNumber}")]
        public async Task<ApiResponse<List<ActivityDetailsViewModel>>> GetAllActivityDetails(string userId,int pageNumber)
        {
            var apiResponse = new ApiResponse<List<ActivityDetailsViewModel>>();
            try
            {
                var activity = await activityDetailRepository.GetAllActivityDetails(userId,pageNumber);
                var data = mapper.Map<List<ActivityDetails>, List<ActivityDetailsViewModel>>(activity.Value.GridRecords);
                apiResponse.Success = true;
                apiResponse.Result = data;
                apiResponse.CurrentPage = activity.Value.CurrentPage;
                apiResponse.TotalPage = activity.Value.TotalPages;
                apiResponse.TotalRecords = activity.Value.TotalRecords;
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
        [HttpPost("PostActivityDetails")]
        public async Task<ApiResponse<string>> PostActivityDetails(ActivityDetailsViewModel activityDetailsViewModel)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var activitydata = mapper.Map<ActivityDetailsViewModel, ActivityDetails>(activityDetailsViewModel);
                activitydata.CreatedByUserId= httpContextAccessor.HttpContext.Session.GetString("UserId");
                activitydata.CreatedDate = DateTime.UtcNow;
                activitydata.ModifiedDate=DateTime.UtcNow;
                bool data = await activityDetailRepository.PostActivityDetail(activitydata);
                apiResponse.Result = "activityDetails added successfully";
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
        [HttpGet("GetActivityDetails/{id}")]
        public async Task<ApiResponse<ActivityDetailsViewModel>> GetActivityDetails(long id)
        {
            var apiResponse = new ApiResponse<ActivityDetailsViewModel>();
            try
            {
                var activity = await activityDetailRepository.GetActivityDetails(id);
                var data = mapper.Map<ActivityDetails, ActivityDetailsViewModel>(activity);
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
        [HttpDelete("DeleteActivityDetails/{Id}")]
        public async Task<ApiResponse<string>> DeleteActivityDetails(long Id)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var role = await activityDetailRepository.DeleteActivityDetails(Id);
                apiResponse.Success = true;
                apiResponse.Message = "Activity deleted Successfully";
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
        [HttpPost("UploadActivityDetailsDocument")]
        public async Task<ApiResponse<string>> UploadActivityDetailsDocument([FromForm] ActivityDetailsDocumentViewModel activitydetailsDocumentViewModel)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var name = configuration.GetSection("ActivityDetailDocument").Value;
                string path = string.Empty;
                List<DocumentViewModel> documents = new List<DocumentViewModel>();
                foreach (var files in activitydetailsDocumentViewModel.Files)
                {
                    if (files.ContentType.Contains("image"))
                    {
                        path = name + "Image/";
                    }
                    else
                    {
                        path = name + "Document/";

                    }
                    DocumentViewModel document = new DocumentViewModel();
                    document.FileName = Path.GetFileNameWithoutExtension(files.FileName);
                    document.FileExtension = Path.GetExtension(files.FileName);
                    document.IsDeleted = false;
                    document.CreatedDate = DateTime.UtcNow;
                    document.CreatedBy = HttpContext.Session.GetString("UserId");
                    document.Path = path + files.FileName;
                    ; documents.Add(document);

                }
                var instrumentdata = mapper.Map<List<DocumentViewModel>, List<Document>>(documents);

                if (activitydetailsDocumentViewModel.Id > 0)
                {
                    var data = await documentRepository.UpdateDocument(instrumentdata, activitydetailsDocumentViewModel.Id);
                    foreach (var item in data)
                    {
                        ActivityDetailDocumentMapping activityDetailDocumentMapping = new ActivityDetailDocumentMapping()
                        {
                            ActivityDetailId = activitydetailsDocumentViewModel.Id,
                            DocumentId = item,
                            IsDeleted = false,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = instrumentdata.First().CreatedBy
                        };
                        bool result = await activityDetailRepository.AddActivityDetailDocumentMapping(activityDetailDocumentMapping);
                    }
                    apiResponse.Result = "activity detail document uploaded successfully";
                }
                else
                {
                    var data = await documentRepository.UploadDocument(instrumentdata);
                    var activityDetailId = await activityDetailRepository.GetLastInsertedActivityDetailId();
                    foreach (var item in data)
                    {
                        ActivityDetailDocumentMapping activityDetailDocument = new ActivityDetailDocumentMapping()
                        {
                            ActivityDetailId = activityDetailId,
                            DocumentId = item,
                            IsDeleted = false,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = instrumentdata.First().CreatedBy
                        };
                        bool result = await activityDetailRepository.AddActivityDetailDocumentMapping(activityDetailDocument);
                    }
                    apiResponse.Result = "activity Detail document added successfully";
                }
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
        [HttpDelete("DeleteDocument/{documentId}/{activityDetailId}")]
        public async Task<ApiResponse<string>> DeleteDocument(long documentId, long activityDetailId)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var role = await activityDetailRepository.DeleteDocument(documentId, activityDetailId);
                apiResponse.Success = true;
                apiResponse.Message = "activity detail document deleted Successfully";
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
