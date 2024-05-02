using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using Hutech.Core.Entities;
using Hutech.Models;
using Hutech.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Hutech.Controllers
{
    [Authorize]
    public class InstrumentIdController : Controller
    {
        public IConfiguration configuration { get; set; }
        private readonly ILogger<InstrumentIdController> logger;
        private readonly LanguageService languageService;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnv;
        public InstrumentIdController(IConfiguration _configuration, ILogger<InstrumentIdController> _logger, LanguageService _languageService, Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnv)
        {
            configuration = _configuration;
            logger = _logger;
            languageService = _languageService;
            hostingEnv = _hostingEnv;
        }
        public async Task<IActionResult> AddInstrumentIdAsync()
        {
            InstrumentIdDocumentViewModel instrumentIdDocument = new InstrumentIdDocumentViewModel();
            var token = Request.Cookies["jwtCookie"];
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();

                token = token.Replace("Bearer ", "");
            }
            string apiUrl = configuration["Baseurl"];
            InstrumentIdViewModel instrumentId = new InstrumentIdViewModel();
            try
            {
                List<InstrumentViewModel> instrument = new List<InstrumentViewModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("Instrument/GetActiveInstrument");

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        instrument = root["result"].ToObject<List<InstrumentViewModel>>();
                    }
                }
                var data = instrument.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();

                //department
                List<DepartmentViewModel> departments = new List<DepartmentViewModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("Department/GetActiveDepartment");

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        departments = root["result"].ToObject<List<DepartmentViewModel>>();
                    }
                }
                var department = departments.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();
                //Location
                List<LocationViewModel> locations = new List<LocationViewModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("Location/GetActiveLocation");

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        locations = root["result"].ToObject<List<LocationViewModel>>();
                    }
                }
                var location = locations.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();

                //Team

                List<TeamViewModel> teams = new List<TeamViewModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("Team/GetActiveTeam");

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        teams = root["result"].ToObject<List<TeamViewModel>>();
                    }
                }
                var team = teams.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();
                instrumentIdDocument.Teams = team;
                instrumentIdDocument.Instruments = data;
                instrumentIdDocument.Departments = department;
                instrumentIdDocument.Locations = location;
                instrumentIdDocument.InstallationDate = DateTime.UtcNow;
                ViewBag.AllowedFileExtension = HttpContext.Session.GetString("FileType");
                ViewBag.AlloweFileSize = HttpContext.Session.GetString("FileSize");
            }

            catch (Exception ex)
            {

            }
            return View(instrumentIdDocument);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddInstrumentId(InstrumentIdDocumentViewModel instrumentIdDocumentViewModel)
        {
            ViewBag.AllowedFileExtension = HttpContext.Session.GetString("FileType");
            ViewBag.AlloweFileSize = HttpContext.Session.GetString("FileSize");
            var token = Request.Cookies["jwtCookie"];
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();

                token = token.Replace("Bearer ", "");
            }
            var InstrumentId = new InstrumentIdViewModel()
            {
                Id = instrumentIdDocumentViewModel.Id,
                InstrumentsId = instrumentIdDocumentViewModel.InstrumentsId,
                Model = instrumentIdDocumentViewModel.Model,
                InstrumentSerial = instrumentIdDocumentViewModel.InstrumentSerial,
                InstrumentId = instrumentIdDocumentViewModel.InstrumentId,
                LocationId = instrumentIdDocumentViewModel.LocationId,
                DepartmentId = instrumentIdDocumentViewModel.DepartmentId,
                TeamId = instrumentIdDocumentViewModel.TeamId,
                TeamLocation = instrumentIdDocumentViewModel.TeamLocation,
                IsActive = instrumentIdDocumentViewModel.IsActive,
                IsDeleted = instrumentIdDocumentViewModel.IsDeleted,
                LocationName = instrumentIdDocumentViewModel.LocationName,
                InstrumentName = instrumentIdDocumentViewModel.InstrumentName,
                TeamName = instrumentIdDocumentViewModel.TeamName,
                CreatedByUserId = instrumentIdDocumentViewModel.CreatedByUserId,
                ModifiedByUserId = instrumentIdDocumentViewModel.ModifiedByUserId,
                DatecreatedUtc = instrumentIdDocumentViewModel.DatecreatedUtc,
                DateModifiedUtc = instrumentIdDocumentViewModel.DateModifiedUtc,
                FullName = instrumentIdDocumentViewModel.FullName,
                Role = instrumentIdDocumentViewModel?.Role,
                InstallationDate = instrumentIdDocumentViewModel.InstallationDate,
            };
            var validation = new InstrumentIdDocumentViewModelValidator();
            var result = validation.Validate(instrumentIdDocumentViewModel);
            string apiUrl = configuration["Baseurl"];


            List<InstrumentViewModel> instrument = new List<InstrumentViewModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                HttpResponseMessage Res = await client.GetAsync("Instrument/GetActiveInstrument");

                if (Res.IsSuccessStatusCode)
                {
                    var content = await Res.Content.ReadAsStringAsync();
                    JObject root = JObject.Parse(content);
                    instrument = root["result"].ToObject<List<InstrumentViewModel>>();
                }
            }
            var instruments = instrument.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            List<LocationViewModel> locations = new List<LocationViewModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                HttpResponseMessage Res = await client.GetAsync("Location/GetActiveLocation");

                if (Res.IsSuccessStatusCode)
                {
                    var content = await Res.Content.ReadAsStringAsync();
                    JObject root = JObject.Parse(content);
                    locations = root["result"].ToObject<List<LocationViewModel>>();
                }
            }
            var location = locations.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            List<TeamViewModel> teams = new List<TeamViewModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                HttpResponseMessage Res = await client.GetAsync("Team/GetActiveTeam");

                if (Res.IsSuccessStatusCode)
                {
                    var content = await Res.Content.ReadAsStringAsync();
                    JObject root = JObject.Parse(content);
                    teams = root["result"].ToObject<List<TeamViewModel>>();
                }
            }
            var team = teams.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            //department
            List<DepartmentViewModel> departments = new List<DepartmentViewModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                HttpResponseMessage Res = await client.GetAsync("Department/GetActiveDepartment");

                if (Res.IsSuccessStatusCode)
                {
                    var content = await Res.Content.ReadAsStringAsync();
                    JObject root = JObject.Parse(content);
                    departments = root["result"].ToObject<List<DepartmentViewModel>>();
                }
            }
            var department = departments.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            if (!result.IsValid)
            {
                instrumentIdDocumentViewModel.Locations = location;
                instrumentIdDocumentViewModel.Teams = team;
                instrumentIdDocumentViewModel.Instruments = instruments;
                instrumentIdDocumentViewModel.Departments = department;
                return View(instrumentIdDocumentViewModel);
            }
            else
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        InstrumentId.DatecreatedUtc = DateTime.UtcNow;
                        InstrumentId.DateModifiedUtc = DateTime.UtcNow;
                        InstrumentId.CreatedByUserId = HttpContext.Session.GetString("UserId");
                        var json = JsonConvert.SerializeObject(InstrumentId);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                        HttpResponseMessage Res = await client.PostAsync("InstrumentId/PostInstrumentId", stringContent);

                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                            JObject rootData = JObject.Parse(content);
                            if (instrumentIdDocumentViewModel.Files != null)
                            {
                                if (instrumentIdDocumentViewModel.Files.Count > 0)
                                {
                                    var FileDic = "Documents/InstrumentId/Image/";
                                    string FilePath = Path.Combine(hostingEnv.ContentRootPath, FileDic);

                                    var DocumentDic = "Documents/InstrumentId/Document";
                                    string FileDocumentPath = Path.Combine(hostingEnv.ContentRootPath, DocumentDic);

                                    if (!Directory.Exists(FilePath))
                                        Directory.CreateDirectory(FilePath);

                                    if (!Directory.Exists(FileDocumentPath))
                                        Directory.CreateDirectory(FileDocumentPath);

                                    if (instrumentIdDocumentViewModel.Files != null)
                                    {
                                        foreach (var data in instrumentIdDocumentViewModel.Files)
                                        {
                                            if (data.ContentType.Contains("image"))
                                            {
                                                var imgFilePath = Path.Combine(FilePath, data.FileName);
                                                using (FileStream fs = System.IO.File.Create(imgFilePath))
                                                {
                                                    data.CopyTo(fs);
                                                }
                                            }
                                            else
                                            {
                                                var imgFilePath = Path.Combine(FileDocumentPath, data.FileName);
                                                using (FileStream fs = System.IO.File.Create(imgFilePath))
                                                {
                                                    data.CopyTo(fs);
                                                }
                                            }
                                        }
                                    }
                                    var formData = new Dictionary<string, string>()
                                    {
                            { "IsActive", instrumentIdDocumentViewModel.IsActive.ToString()},
                            { "IsDeleted", instrumentIdDocumentViewModel.IsDeleted.ToString()},
                            { "InstrumentsId", instrumentIdDocumentViewModel.InstrumentsId},
                            {"Model",instrumentIdDocumentViewModel.Model },
                            {"InstrumentSerial" ,instrumentIdDocumentViewModel.InstrumentSerial},
                            {"TeamLocation" ,instrumentIdDocumentViewModel.TeamLocation},
                                };
                                    var formContent = new MultipartFormDataContent();

                                    foreach (var keyValuePair in formData)
                                    {
                                        formContent.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                                    }

                                    foreach (var file in instrumentIdDocumentViewModel.Files)
                                    {
                                        formContent.Add(new StreamContent(file.OpenReadStream())
                                        {
                                            Headers = { ContentLength = file.Length, ContentType = new MediaTypeHeaderValue(file.ContentType) }
                                        },
                                        "Files",
                                        file.FileName);
                                    }
                                    HttpResponseMessage response = await client.PostAsync("InstrumentId/UploadInstrumentIdDocument/", formContent);
                                    if (response.IsSuccessStatusCode)
                                    {
                                        var contentData = await response.Content.ReadAsStringAsync();
                                        JObject root = JObject.Parse(contentData);
                                    }
                                }
                            }
                            var resultData = rootData["success"].ToString();
                            if (resultData == "False" || resultData == "false")
                            {
                                var Id = rootData["auditId"].ToString();
                                string message = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                                TempData["message"] = message;
                                TempData["RedirectURl"] = "/InstrumentId/AddInstrumentId/";
                            }
                            else
                            {
                                string message = languageService.Getkey("InstrumentId Added Successfully");
                                TempData["message"] = message;
                                TempData["RedirectURl"] = "/InstrumentId/GetAllInstrumentId/";
                            }
                        }
                    }
                    return RedirectToAction("GetAllInstrumentId");

                }

                catch (Exception ex)
                {
                    logger.LogInformation($"Exception Occure.{ex.Message}");
                    throw ex;
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetAllInstrumentId([FromBody] InstrumentIdModel instrumentIdModel)
        {
            instrumentIdModel.PageNumber = 1;
            int pageNumber = 1;
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                int totalRecords = 0;
                int totalPage = 0;
                List<InstrumentIdViewModel> instrumentIdViewModels = new List<InstrumentIdViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var json = JsonConvert.SerializeObject(instrumentIdModel);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    HttpResponseMessage Res = await client.PostAsync("InstrumentId/GetAllFilterInstrumentId", stringContent);

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var Id = root["auditId"].ToString();
                            string message = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                            TempData["message"] = message;
                        }
                        else
                        {
                            instrumentIdViewModels = root["result"].ToObject<List<InstrumentIdViewModel>>();
                            pageNumber = (int)root["currentPage"];
                            totalRecords = (int)root["totalRecords"];
                            totalPage = (int)root["totalPage"];
                        }
                    }
                }
                var data = new InstrumentIdsViewModel()
                {
                    CurrentPage = pageNumber,
                    instrumentIdViewModels = instrumentIdViewModels,
                    TotalPages = totalPage,
                    TotalRecords = totalRecords
                };
                data.InstrumentIdName = !string.IsNullOrEmpty(instrumentIdModel.InstrumentIdName) ? instrumentIdModel.InstrumentIdName : "";
                data.Model = !string.IsNullOrEmpty(instrumentIdModel.Model) ? instrumentIdModel.Model : "";
                data.InstrumentName = !string.IsNullOrEmpty(instrumentIdModel.InstrumentName) ? instrumentIdModel.InstrumentName : "";
                data.InstrumentSerial = !string.IsNullOrEmpty(instrumentIdModel.InstrumentSerial) ? instrumentIdModel.InstrumentSerial : "";
                data.InstrumentLocation = !string.IsNullOrEmpty(instrumentIdModel.InstrumentLocation) ? instrumentIdModel.InstrumentLocation : "";
                data.TeamName = !string.IsNullOrEmpty(instrumentIdModel.TeamName) ? instrumentIdModel.TeamName : "";
                data.UpdatedBy = !string.IsNullOrEmpty(instrumentIdModel.UpdatedBy) ? instrumentIdModel.UpdatedBy : "";
                //data.Status = locationModel.status;
                data.UpdatedDate = instrumentIdModel.UpdatedDate;
                var items = new List<SelectListItem>();
                foreach (int value in Enum.GetValues(typeof(StatusViewModel)))
                {
                    items.Add(new SelectListItem
                    {
                        Text = Enum.GetName(typeof(StatusViewModel), value),
                        Value = value.ToString(),
                    });
                }
                data.Status = items;
                data.SelectedStatus = System.Convert.ToInt32(instrumentIdModel.Status);
                string requestedWithHeader = Request.Headers["X-Requested-With"];
                data.InstrumentIdName = !string.IsNullOrEmpty(instrumentIdModel.InstrumentIdName) ? instrumentIdModel.InstrumentIdName : "";
                data.Model = !string.IsNullOrEmpty(instrumentIdModel.Model) ? instrumentIdModel.Model : "";
                data.InstrumentName = !string.IsNullOrEmpty(instrumentIdModel.InstrumentName) ? instrumentIdModel.InstrumentName : "";
                data.InstrumentSerial = !string.IsNullOrEmpty(instrumentIdModel.InstrumentSerial) ? instrumentIdModel.InstrumentSerial : "";
                data.InstrumentLocation = !string.IsNullOrEmpty(instrumentIdModel.InstrumentLocation) ? instrumentIdModel.InstrumentLocation : "";
                data.TeamName = !string.IsNullOrEmpty(instrumentIdModel.TeamName) ? instrumentIdModel.TeamName : "";
                data.UpdatedBy = !string.IsNullOrEmpty(instrumentIdModel.UpdatedBy) ? instrumentIdModel.UpdatedBy : "";
                data.UpdatedDate = instrumentIdModel.UpdatedDate;
                return View("GetAllInstrumentId", data);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> GetAllInstrumentId(int pageNumber = 1, string? instrumentIdName = null, string? model = null, string? instrumentName = null, string? instrumentSerial = null, string? instrumentLocation = null, string? teamName = null, string? updatedBy = null, string? updatedDate = null, string? Status = null)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                int totalRecords = 0;
                int totalPage = 0;
                InstrumentIdsViewModel instrumentIdsViewModel = new InstrumentIdsViewModel();
                List<InstrumentIdViewModel> instrumentIds = new List<InstrumentIdViewModel>();
                string apiUrl = configuration["Baseurl"];
                InstrumentIdModel instrumentIdModel = new InstrumentIdModel();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    instrumentIdModel.InstrumentIdName = instrumentIdName;
                    instrumentIdModel.Model = model;
                    instrumentIdModel.InstrumentName = instrumentName;
                    instrumentIdModel.InstrumentSerial = instrumentSerial;
                    instrumentIdModel.InstrumentLocation = instrumentLocation;
                    instrumentIdModel.TeamName = teamName;
                    instrumentIdModel.UpdatedBy = updatedBy;
                    instrumentIdModel.PageNumber = pageNumber;
                    if (!string.IsNullOrEmpty(updatedDate))
                        instrumentIdModel.UpdatedDate = DateTime.Parse(updatedDate);
                    instrumentIdModel.Status = Status;
                    var json = JsonConvert.SerializeObject(instrumentIdModel);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                    HttpResponseMessage Res = await client.PostAsync("InstrumentId/GetAllFilterInstrumentId", stringContent);
                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var Id = root["auditId"].ToString();
                            string message = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                            TempData["message"] = message;
                        }
                        else
                        {
                            instrumentIds = root["result"].ToObject<List<InstrumentIdViewModel>>();
                            pageNumber = (int)root["currentPage"];
                            totalRecords = (int)root["totalRecords"];
                            totalPage = (int)root["totalPage"];
                        }
                    }
                }
                var data = new InstrumentIdsViewModel()
                {
                    CurrentPage = pageNumber,
                    instrumentIdViewModels = instrumentIds,
                    TotalPages = totalPage,
                    TotalRecords = totalRecords
                };
                var items = new List<SelectListItem>();
                foreach (int value in Enum.GetValues(typeof(StatusViewModel)))
                {
                    items.Add(new SelectListItem
                    {
                        Text = Enum.GetName(typeof(StatusViewModel), value),
                        Value = value.ToString()
                    });
                }
                data.Status = items;
                if (Status == null)
                    data.SelectedStatus = (int)StatusViewModel.Active;
                else if (!string.IsNullOrEmpty(Status))
                    data.SelectedStatus = System.Convert.ToInt32(Status);
                data.InstrumentIdName = !string.IsNullOrEmpty(instrumentIdModel.InstrumentIdName) ? instrumentIdModel.InstrumentIdName : "";
                data.Model = !string.IsNullOrEmpty(instrumentIdModel.Model) ? instrumentIdModel.Model : "";
                data.InstrumentName = !string.IsNullOrEmpty(instrumentIdModel.InstrumentName) ? instrumentIdModel.InstrumentName : "";
                data.InstrumentSerial = !string.IsNullOrEmpty(instrumentIdModel.InstrumentSerial) ? instrumentIdModel.InstrumentSerial : "";
                data.InstrumentLocation = !string.IsNullOrEmpty(instrumentIdModel.InstrumentLocation) ? instrumentIdModel.InstrumentLocation : "";
                data.TeamName = !string.IsNullOrEmpty(instrumentIdModel.TeamName) ? instrumentIdModel.TeamName : "";
                data.UpdatedBy = !string.IsNullOrEmpty(instrumentIdModel.UpdatedBy) ? instrumentIdModel.UpdatedBy : "";
                data.UpdatedDate = instrumentIdModel.UpdatedDate;
                return View(data);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> DeleteInstrumentId(long id)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.DeleteAsync(string.Format("InstrumentId/DeleteInstrumentId/{0}", id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var Id = root["auditId"].ToString();
                            string message = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                            TempData["message"] = message;

                        }
                        else
                        {
                            string message = languageService.Getkey("InstrumentId Deleted Successfully");
                            TempData["message"] = message;
                            TempData["RedirectURl"] = "/InstrumentId/GetAllInstrumentId/";
                        }
                        //var message = root["value"].ToString();
                    }
                }
                return RedirectToAction("GetAllInstrumentId");
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> EditInstrumentId(long id)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                string apiUrl = configuration["Baseurl"];
                InstrumentIdViewModel instrumentViewModel = new InstrumentIdViewModel();
                List<InstrumentViewModel> instrument = new List<InstrumentViewModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("Instrument/GetActiveInstrument");

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        instrument = root["result"].ToObject<List<InstrumentViewModel>>();
                    }
                }
                var data = instrument.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();


                //Location
                List<LocationViewModel> locations = new List<LocationViewModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("Location/GetActiveLocation");

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        locations = root["result"].ToObject<List<LocationViewModel>>();
                    }
                }
                var location = locations.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();

                //Team

                List<TeamViewModel> teams = new List<TeamViewModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("Team/GetActiveTeam");

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        teams = root["result"].ToObject<List<TeamViewModel>>();
                    }
                }
                var team = teams.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();
                //Department

                List<DepartmentViewModel> departments = new List<DepartmentViewModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("Department/GetActiveDepartment");

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        departments = root["result"].ToObject<List<DepartmentViewModel>>();
                    }
                }
                var department = departments.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.GetAsync(string.Format("InstrumentId/GetInstrumentIdDetail/{0}", id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var Id = root["auditId"].ToString();
                            string message = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                            TempData["message"] = message;
                            TempData["RedirectURl"] = "/InstrumentId/AddInstrumentId/";
                        }
                        else
                        {
                            instrumentViewModel = root["result"].ToObject<InstrumentIdViewModel>();
                        }
                    }
                }
                InstrumentIdDocumentViewModel instrumentIdDocumentViewModel = new InstrumentIdDocumentViewModel();
                instrumentIdDocumentViewModel.Teams = team;
                instrumentIdDocumentViewModel.Instruments = data;
                instrumentIdDocumentViewModel.Locations = location;
                instrumentIdDocumentViewModel.Departments = department;
                instrumentIdDocumentViewModel.Id = instrumentViewModel.Id;
                instrumentIdDocumentViewModel.InstrumentId = instrumentViewModel.InstrumentId;
                instrumentIdDocumentViewModel.InstrumentsId = instrumentViewModel.InstrumentsId;
                instrumentIdDocumentViewModel.InstrumentSerial = instrumentViewModel.InstrumentSerial;
                instrumentIdDocumentViewModel.LocationId = instrumentViewModel.LocationId;
                instrumentIdDocumentViewModel.DepartmentId = instrumentViewModel.DepartmentId;
                instrumentIdDocumentViewModel.TeamId = instrumentViewModel.TeamId;
                instrumentIdDocumentViewModel.TeamLocation = instrumentViewModel.TeamLocation;
                instrumentIdDocumentViewModel.LocationName = instrumentViewModel.LocationName;
                instrumentIdDocumentViewModel.TeamName = instrumentViewModel.TeamName;
                instrumentIdDocumentViewModel.InstrumentName = instrumentViewModel.InstrumentName;
                instrumentIdDocumentViewModel.CreatedByUserId = instrumentViewModel.CreatedByUserId;
                instrumentIdDocumentViewModel.ModifiedByUserId = instrumentViewModel.ModifiedByUserId;
                instrumentIdDocumentViewModel.DatecreatedUtc = instrumentViewModel.DatecreatedUtc;
                instrumentIdDocumentViewModel.DateModifiedUtc = instrumentViewModel.DateModifiedUtc;
                instrumentIdDocumentViewModel.FullName = instrumentViewModel.FullName;
                instrumentIdDocumentViewModel.Role = instrumentViewModel.Role;
                instrumentIdDocumentViewModel.InstallationDate = instrumentViewModel.InstallationDate;
                instrumentIdDocumentViewModel.Model = instrumentViewModel.Model;
                instrumentIdDocumentViewModel.IsActive = instrumentViewModel.IsActive;
                instrumentIdDocumentViewModel.IsDeleted=instrumentViewModel.IsDeleted;
                var documentData = (string[])null;
                var documentIdsData = (string[])null;
                if (!string.IsNullOrEmpty(instrumentViewModel.Path))
                {
                    var documents = instrumentViewModel.Path.Split(",");
                    documentData = documents;
                }
                if (!string.IsNullOrEmpty(instrumentViewModel.DocumentId))
                {
                    var documentIds = instrumentViewModel.DocumentId.Split(",");
                    documentIdsData = documentIds;
                }
                if (documentData != null)
                {
                    for (int i = 0; i < documentData.Length; i++)
                    {
                        var filePath = documentData[i];
                        var fileName = documentData[i].Split("/").Last();
                        FileViewModel file = new FileViewModel();
                        file.FileName = fileName;
                        file.FilePath = filePath;
                        file.Id = System.Convert.ToInt64(documentIdsData[i]);
                        instrumentIdDocumentViewModel.UplodedFile.Add(file);
                    }
                }
                ViewBag.AllowedFileExtension = HttpContext.Session.GetString("FileType");
                ViewBag.AlloweFileSize = HttpContext.Session.GetString("FileSize");
                return View(instrumentIdDocumentViewModel);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInstrumentId(InstrumentIdDocumentViewModel instrumentIdDocumentViewModel)
        {
            ViewBag.AllowedFileExtension = HttpContext.Session.GetString("FileType");
            ViewBag.AlloweFileSize = HttpContext.Session.GetString("FileSize");
            string apiUrl = configuration["Baseurl"];
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                var InstrumentId = new InstrumentIdViewModel()
                {
                    Id = instrumentIdDocumentViewModel.Id,
                    InstrumentsId = instrumentIdDocumentViewModel.InstrumentsId,
                    Model = instrumentIdDocumentViewModel.Model,
                    InstrumentSerial = instrumentIdDocumentViewModel.InstrumentSerial,
                    InstrumentId = instrumentIdDocumentViewModel.InstrumentId,
                    LocationId = instrumentIdDocumentViewModel.LocationId,
                    DepartmentId = instrumentIdDocumentViewModel.DepartmentId,
                    TeamId = instrumentIdDocumentViewModel.TeamId,
                    TeamLocation = instrumentIdDocumentViewModel.TeamLocation,
                    IsActive = instrumentIdDocumentViewModel.IsActive,
                    IsDeleted = instrumentIdDocumentViewModel.IsDeleted,
                    LocationName = instrumentIdDocumentViewModel.LocationName,
                    InstrumentName = instrumentIdDocumentViewModel.InstrumentName,
                    TeamName = instrumentIdDocumentViewModel.TeamName,
                    CreatedByUserId = instrumentIdDocumentViewModel.CreatedByUserId,
                    ModifiedByUserId = instrumentIdDocumentViewModel.ModifiedByUserId,
                    DatecreatedUtc = instrumentIdDocumentViewModel.DatecreatedUtc,
                    DateModifiedUtc = instrumentIdDocumentViewModel.DateModifiedUtc,
                    FullName = instrumentIdDocumentViewModel.FullName,
                    Role = instrumentIdDocumentViewModel?.Role,
                    InstallationDate = instrumentIdDocumentViewModel.InstallationDate,
                };
                var validation = new InstrumentIdDocumentViewModelValidator();
                var result = validation.Validate(instrumentIdDocumentViewModel);
                if (!result.IsValid)
                {
                    List<InstrumentViewModel> instrument = new List<InstrumentViewModel>();
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                        HttpResponseMessage Res = await client.GetAsync("Instrument/GetActiveInstrument");

                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            instrument = root["result"].ToObject<List<InstrumentViewModel>>();
                        }
                    }
                    var data = instrument.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }).ToList();


                    //Location
                    List<LocationViewModel> locations = new List<LocationViewModel>();
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                        HttpResponseMessage Res = await client.GetAsync("Location/GetActiveLocation");

                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            locations = root["result"].ToObject<List<LocationViewModel>>();
                        }
                    }
                    var location = locations.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }).ToList();
                    //Department
                    List<DepartmentViewModel> departments = new List<DepartmentViewModel>();
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                        HttpResponseMessage Res = await client.GetAsync("Department/GetActiveDepartment");

                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            departments = root["result"].ToObject<List<DepartmentViewModel>>();
                        }
                    }
                    var department = departments.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }).ToList();

                    //Team

                    List<TeamViewModel> teams = new List<TeamViewModel>();
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                        HttpResponseMessage Res = await client.GetAsync("Team/GetActiveTeam");

                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            teams = root["result"].ToObject<List<TeamViewModel>>();
                        }
                    }
                    var team = teams.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }).ToList();
                    instrumentIdDocumentViewModel.Teams = team;
                    instrumentIdDocumentViewModel.Instruments = data;
                    instrumentIdDocumentViewModel.Locations = location;
                    instrumentIdDocumentViewModel.Departments = department;
                    return View(instrumentIdDocumentViewModel);
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        InstrumentId.DateModifiedUtc = DateTime.UtcNow;
                        InstrumentId.ModifiedByUserId = HttpContext.Session.GetString("UserId");
                        var json = JsonConvert.SerializeObject(InstrumentId);
                        var stringcontenet = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpResponseMessage response = await client.PutAsync("InstrumentId/PutInstrumentId", stringcontenet);

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            if (instrumentIdDocumentViewModel.Files != null)
                            {
                                if (instrumentIdDocumentViewModel.Files.Count > 0)
                                {
                                    var FileDic = "Documents/InstrumentId/Image/";
                                    string FilePath = Path.Combine(hostingEnv.ContentRootPath, FileDic);

                                    var DocumentDic = "Documents/InstrumentId/Document";
                                    string FileDocumentPath = Path.Combine(hostingEnv.ContentRootPath, DocumentDic);

                                    if (!Directory.Exists(FilePath))
                                        Directory.CreateDirectory(FilePath);

                                    if (!Directory.Exists(FileDocumentPath))
                                        Directory.CreateDirectory(FileDocumentPath);

                                    if (instrumentIdDocumentViewModel.Files != null)
                                    {
                                        foreach (var data in instrumentIdDocumentViewModel.Files)
                                        {
                                            if (data.ContentType.Contains("image"))
                                            {
                                                var imgFilePath = Path.Combine(FilePath, data.FileName);
                                                using (FileStream fs = System.IO.File.Create(imgFilePath))
                                                {
                                                    data.CopyTo(fs);
                                                }
                                            }
                                            else
                                            {
                                                var imgFilePath = Path.Combine(FileDocumentPath, data.FileName);
                                                using (FileStream fs = System.IO.File.Create(imgFilePath))
                                                {
                                                    data.CopyTo(fs);
                                                }
                                            }
                                        }
                                    }
                                    var formData = new Dictionary<string, string>()
                                    {
                            { "IsActive", instrumentIdDocumentViewModel.IsActive.ToString()},
                            { "IsDeleted", instrumentIdDocumentViewModel.IsDeleted.ToString()},
                            { "InstrumentsId", instrumentIdDocumentViewModel.InstrumentsId},
                            {"Model",instrumentIdDocumentViewModel.Model },
                            {"InstrumentSerial" ,instrumentIdDocumentViewModel.InstrumentSerial},
                            {"TeamLocation" ,instrumentIdDocumentViewModel.TeamLocation},
                                        {"Id",instrumentIdDocumentViewModel.Id.ToString() }
                                };
                                    var formContent = new MultipartFormDataContent();

                                    foreach (var keyValuePair in formData)
                                    {
                                        formContent.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                                    }

                                    foreach (var file in instrumentIdDocumentViewModel.Files)
                                    {
                                        formContent.Add(new StreamContent(file.OpenReadStream())
                                        {
                                            Headers = { ContentLength = file.Length, ContentType = new MediaTypeHeaderValue(file.ContentType) }
                                        },
                                        "Files",
                                        file.FileName);
                                    }
                                    HttpResponseMessage responseForDocumentUpload = await client.PostAsync("InstrumentId/UploadInstrumentIdDocument/", formContent);
                                    if (responseForDocumentUpload.IsSuccessStatusCode)
                                    {
                                        var contentData = await response.Content.ReadAsStringAsync();
                                        JObject rootResult = JObject.Parse(contentData);
                                    }
                                }
                            }
                            JObject root = JObject.Parse(content);
                            var resultData = root["success"].ToString();
                            if (resultData == "False" || resultData == "false")
                            {
                                var Id = root["auditId"].ToString();
                                string message = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                                TempData["message"] = message;
                                TempData["RedirectURl"] = "/InstrumentId/EditInstrumentId/";
                                return RedirectToAction("InstrumentId", new { id = instrumentIdDocumentViewModel.Id });

                            }
                            else
                            {
                                instrumentIdDocumentViewModel = root["result"].ToObject<InstrumentIdDocumentViewModel>();
                                string message = languageService.Getkey("InstrumentId Updated Successfully");
                                TempData["message"] = message;
                                TempData["RedirectURl"] = "/InstrumentId/GetAllInstrumentId/";
                            }
                        }
                    }
                    return RedirectToAction("GetAllInstrumentId");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }

        public ActionResult Download(string fileName)
        {
            string path = string.Empty;
            var name = fileName.Split("/").Last();
            if (fileName.Contains("Image"))
            {
                path = Path.Combine(hostingEnv.ContentRootPath, "Documents/InstrumentId/Image/" + name);
            }
            else
            {
                path = Path.Combine(hostingEnv.ContentRootPath, "Documents/InstrumentId/Document/" + name);

            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, name);
        }
        public async Task<IActionResult> DeleteDocument(long documentId, long instrumentId)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.DeleteAsync(string.Format("InstrumentId/DeleteDocument/{0}/{1}", documentId, instrumentId));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var Id = root["auditId"].ToString();
                            string message = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                            TempData["message"] = message;

                        }
                        else
                        {
                            string message = languageService.Getkey("Document Deleted Successfully");
                            TempData["message"] = message;
                        }
                    }
                }
                return RedirectToAction("GetAllInstrumentId");
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
    }
}



