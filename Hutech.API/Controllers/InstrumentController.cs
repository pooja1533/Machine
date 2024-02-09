using AutoMapper;
using Hutech.Application.Interfaces;
using Hutech.Core.Entities;
using Hutech.Infrastructure.Repository;
using Hutech.Models;
using Hutech.Sql.Queries;
using Imputabiliteafro.Api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hutech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstrumentController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IInstrumentRepository instrumentRepository;
        private readonly IDocumentRepository documentRepository;
        private readonly ILogger<InstrumentController> logger;
        private readonly IConfiguration configuration;
        private readonly IAuditRepository auditRepository;
        public InstrumentController(IMapper _mapper, IInstrumentRepository _instrumentRepository,IDocumentRepository _documentRepository, ILogger<InstrumentController> _logger,IConfiguration _configuration,IAuditRepository _auditRepository)
        {
            mapper = _mapper;
            documentRepository = _documentRepository;
            instrumentRepository = _instrumentRepository;
            logger = _logger;
            configuration = _configuration;
            auditRepository = _auditRepository;
        }
        [HttpPost("UploadInstrumentDocument")]
        public async Task<ApiResponse<string>> UploadInstrumentDocument([FromForm] InstrumentDocumentViewModel instrumentDocumentViewModel)
        {
            try
            {
                var name = configuration.GetSection("InstrumentDocument").Value;
                string path = string.Empty;
                List <DocumentViewModel> documents = new List<DocumentViewModel>();
                foreach(var files in instrumentDocumentViewModel.Files)
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
;                    documents.Add(document);

                }
                var instrumentdata = mapper.Map<List<DocumentViewModel>, List<Document>>(documents);

                var apiResponse = new ApiResponse<string>();
                if (instrumentDocumentViewModel.Id > 0)
                {
                    var data = await documentRepository.UpdateDocument(instrumentdata, instrumentDocumentViewModel.Id);
                    foreach(var item in data)
                    {
                        InstrumentDocumentMapping instrumentDocument = new InstrumentDocumentMapping()
                        {
                            InstrumentId = instrumentDocumentViewModel.Id,
                            DocumentId = item,
                            IsDeleted = false,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = instrumentdata.First().CreatedBy
                        };
                        bool result = await instrumentRepository.AddInstrumentDocumentMapping(instrumentDocument);
                        //var instrumentDocumentMapping = await connection.QueryAsync<long>(InstrumentQueries.AddInstrumentDocumentMapping, instrumentDocument);
                    }
                    apiResponse.Result = "instrumentdata updated successfully";
                }
                else
                {
                    var data = await documentRepository.UploadDocument(instrumentdata);
                    var instrumentId = await instrumentRepository.GetLastInsertedInstrumentId();
                    foreach (var item in data)
                    {
                        InstrumentDocumentMapping instrumentDocument = new InstrumentDocumentMapping()
                        {
                            InstrumentId = instrumentId,
                            DocumentId = item,
                            IsDeleted = false,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = instrumentdata.First().CreatedBy
                        };
                        bool result = await instrumentRepository.AddInstrumentDocumentMapping(instrumentDocument);
                    }
                    apiResponse.Result = "instrumentdata added successfully";
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
                var apiResponse = new ApiResponse<string>();
                apiResponse.Success = false;
                apiResponse.AuditId = auditId;
                return apiResponse;
            }
        }
        [HttpPost("PostInstrument")]
        public async Task<ApiResponse<string>> PostInstrumentt(InstrumentViewModel instrumentViewModel)
        {
            try
            {
                var apiResponse = new ApiResponse<string>();
                var instrumentdata = mapper.Map<InstrumentViewModel, Instrument>(instrumentViewModel);
                bool data = await instrumentRepository.PostInstrument(instrumentdata);
                apiResponse.Result = "instrumentdata added successfully";
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
        [HttpGet("GetInstrumentDetail/{id}")]
        public async Task<ApiResponse<InstrumentViewModel>> GetInstrumentDetail(long id)
        {
            var apiResponse = new ApiResponse<InstrumentViewModel>();
            try
            {
                var instrument = await instrumentRepository.GetInstrumentDetail(id);
                var data = mapper.Map<Instrument, InstrumentViewModel>(instrument);
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
        [HttpDelete("DeleteExistingInstrumentDocument/{Id}")]
        public async Task<ApiResponse<string>> DeleteExistingInstrumentDocument(long Id)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var role = await instrumentRepository.DeleteExistingInstrumentDocument(Id);
                apiResponse.Success = true;
                apiResponse.Message = "instrument deleted Successfully";
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
        [HttpDelete("DeleteDocument/{documentId}/{instrumentId}")]
        public async Task<ApiResponse<string>> DeleteDocument (long documentId,long instrumentId)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var role = await instrumentRepository.DeleteDocument(documentId,instrumentId);
                apiResponse.Success = true;
                apiResponse.Message = "instrument document deleted Successfully";
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
        [HttpDelete("DeleteInstrument/{Id}")]
        public async Task<ApiResponse<string>> DeleteInstrument(long Id)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var role = await instrumentRepository.DeleteInstrument(Id);
                apiResponse.Success = true;
                apiResponse.Message = "instrument deleted Successfully";
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
        [HttpPut("PutInstrument")]
        public async Task<ApiResponse<string>> PutInstrument(InstrumentViewModel model)
        {
            var apiResponse = new ApiResponse<string>();
            try
            {
                var data = mapper.Map<InstrumentViewModel, Instrument>(model);
                var role = await instrumentRepository.PutInstrument(data);
                apiResponse.Success = true;
                apiResponse.Message = "Update instrument Successfully";
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
        
        [HttpGet("GetInstrument")]
        public async Task<ApiResponse<List<InstrumentViewModel>>> GetInstrument()
        {
            var apiResponse = new ApiResponse<List<InstrumentViewModel>>();
            try
            {
                var instrument = await instrumentRepository.GetInstrument();
                var data = mapper.Map<List<Instrument>, List<InstrumentViewModel>>(instrument);
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
        [HttpGet("GetActiveInstrument")]
        public async Task<ApiResponse<List<InstrumentViewModel>>> GetActiveInstrument()
        {
            var apiResponse = new ApiResponse<List<InstrumentViewModel>>();
            try
            {
                var instrument = await instrumentRepository.GetActiveInstrument();
                var data = mapper.Map<List<Instrument>, List<InstrumentViewModel>>(instrument);
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
    }
}
