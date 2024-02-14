using DocumentFormat.OpenXml.Office2010.Excel;
using Hutech.Models;
using Hutech.Resources;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;

namespace Hutech.Controllers
{
    public class ActivityDetailsController : Controller
    {
        public IConfiguration configuration { get; set; }
        public ILogger<ActivityDetailsController> logger { get; set; }
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnv;
        private readonly LanguageService languageService;
        public ActivityDetailsController(IConfiguration _configuration, ILogger<ActivityDetailsController> _logger, Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnv, LanguageService _languageService)
        {
            configuration = _configuration;
            logger = _logger;
            hostingEnv = _hostingEnv;
            languageService = _languageService;
        }
        //public async Task<IActionResult> GetActivityDetailsForUser(string userId)
        //{
        //    var token = Request.Cookies["jwtCookie"];
        //    if (!string.IsNullOrEmpty(token))
        //    {
        //        var handler = new JwtSecurityTokenHandler();

        //        token = token.Replace("Bearer ", "");
        //    }
        //    ActivityDetailsViewModel model = new ActivityDetailsViewModel();
        //    List<ActivityDetailsViewModel> activityDetails = new List<ActivityDetailsViewModel>();
        //    List<UserViewModel> users = new List<UserViewModel>();
        //    string apiUrl = configuration["Baseurl"];
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(apiUrl);
        //        client.DefaultRequestHeaders.Clear();

        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //        HttpResponseMessage Res = await client.GetAsync(string.Format("ActivityDetails/GetAllActivityDetails/{0}", userId));

        //        if (Res.IsSuccessStatusCode)
        //        {
        //            var content = await Res.Content.ReadAsStringAsync();
        //            JObject root = JObject.Parse(content);
        //            activityDetails = root["result"].ToObject<List<ActivityDetailsViewModel>>();
        //        }
        //    }
        //    return Json(activityDetails);
        //}
        public async Task<IActionResult> GetAllActivityDetails(string userId,int pageNumber=1)
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
                ActivityDetailsViewModel model = new ActivityDetailsViewModel();
                List<ActivityDetailsViewModel> activityDetails = new List<ActivityDetailsViewModel>();
                List<UserViewModel> users=new List<UserViewModel>();
                var loggedinuserId = HttpContext.Session.GetString("UserId").ToString();
                var email = HttpContext.Session.GetString("LoogedInUser".ToString());

                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    if (string.IsNullOrEmpty(userId))
                    {
                        userId = HttpContext.Session.GetString("SelectedUserId");
                        if(string.IsNullOrEmpty(userId))
                            userId = HttpContext.Session.GetString("UserId");
                        HttpContext.Session.SetString("SelectedUserId", userId.ToString());
                    }
                    else if (new Guid(userId) == new Guid())
                    {
                        userId = "0";
                        HttpContext.Session.SetString("SelectedUserId", userId.ToString());
                    }
                    HttpResponseMessage Res = await client.GetAsync(string.Format("ActivityDetails/GetAllActivityDetails/{0}/{1}", userId,pageNumber));

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var Id = root["auditId"].ToString();
                            TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                        }
                        else
                        {
                            activityDetails = root["result"].ToObject<List<ActivityDetailsViewModel>>();
                            pageNumber = (int)root["currentPage"];
                            totalRecords = (int)root["totalRecords"];
                            totalPage = (int)root["totalPage"];
                        }
                    }
                }
                //using (var client = new HttpClient())
                //{
                //    client.BaseAddress = new Uri(apiUrl);
                //    client.DefaultRequestHeaders.Clear();

                //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                //    HttpResponseMessage User = await client.GetAsync("User/GetUsers");

                //    if (User.IsSuccessStatusCode)
                //    {
                //        var content = await User.Content.ReadAsStringAsync();
                //        JObject root = JObject.Parse(content);
                //        users = root["result"].ToObject<List<UserViewModel>>();
                //    }
                //}
                //var data = users.Select(x => new SelectListItem
                //{
                //    Text = x.UserName.ToString(),
                //    Value = x.Id.ToString()
                //}).ToList();
                List<UserViewModel> userViewModels=new List<UserViewModel>();
                UserViewModel modelData=new UserViewModel();
                UserViewModel alluser = new UserViewModel();
                modelData.Id =new Guid(loggedinuserId);
                modelData.UserName = email;
                userViewModels.Add(modelData);
                alluser.Id = new Guid();
                alluser.UserName = "Select All";
                userViewModels.Add(alluser);
                var data = userViewModels.Select(x => new SelectListItem
                {
                    Text = x.UserName.ToString(),
                    Value = x.Id.ToString()
                }).ToList();

                //model.ActivityDetails = activityDetails;
                //model.User = data;
                //model.UserId= HttpContext.Session.GetString("SelectedUserId").ToString();
                var activityDetailsViewModel = new GridData<ActivityDetailsViewModel>()
                {
                    CurrentPage = pageNumber,
                    GridRecords = activityDetails,
                    TotalPages = totalPage,
                    TotalRecords = totalRecords
                };
                ActivityDetailsViewModel viewModel = new ActivityDetailsViewModel();
                viewModel.CurrentPage = pageNumber;
                viewModel.TotalPages = totalPage;
                viewModel.TotalRecords = totalRecords;
                viewModel.ActivityDetails = activityDetails;
                viewModel.User = data;
                viewModel.UserId = HttpContext.Session.GetString("SelectedUserId").ToString();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> AddActivityDetails()
        {
            ActivityDetailsDocumentViewModel activityDetailsViewModel = new ActivityDetailsDocumentViewModel();
            var token = Request.Cookies["jwtCookie"];
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();

                token = token.Replace("Bearer ", "");
            }
            string apiUrl = configuration["Baseurl"];
            List<InstrumentIdViewModel> InstrumenIds= new List<InstrumentIdViewModel>();
            try
            {
                List<InstrumentIdViewModel> instrumentId = new List<InstrumentIdViewModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("InstrumentId/GetActiveInstrumentId");

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        instrumentId = root["result"].ToObject<List<InstrumentIdViewModel>>();
                    }
                }
                var data = instrumentId.Select(x => new SelectListItem
                {
                    Text = x.InstrumentsId.ToString(),
                    Value = x.Id.ToString()
                }).ToList();

                List<InstrumentActivityViewModel> instrumentActivityViewModels = new List<InstrumentActivityViewModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("InstrumentActivity/GetActiveInstrumentActivity");

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        instrumentActivityViewModels = root["result"].ToObject<List<InstrumentActivityViewModel>>();
                    }
                }
                var instrumentActivities = instrumentActivityViewModels.Select(x => new SelectListItem
                {
                    Text = x.InstrumentActivityName.ToString(),
                    Value = x.Id.ToString()
                }).ToList();
                activityDetailsViewModel.InstrumentsActivity = instrumentActivities;
                activityDetailsViewModel.InstrumentNameId = data;
                activityDetailsViewModel.PerformedDate = DateTime.UtcNow;
            }
            catch (Exception ex)
            {

            }
            ViewBag.AllowedFileExtension = HttpContext.Session.GetString("FileType");
            ViewBag.AlloweFileSize = HttpContext.Session.GetString("FileSize");
            return View(activityDetailsViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddActivityDetails(ActivityDetailsDocumentViewModel activityDetailsViewModel)
        {
            try
            {
                string apiUrl = configuration["Baseurl"];
                ViewBag.AllowedFileExtension = HttpContext.Session.GetString("FileType");
                ViewBag.AlloweFileSize = HttpContext.Session.GetString("FileSize");
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                var activityDetail = new ActivityDetailsViewModel()
                {
                    Id = activityDetailsViewModel.Id,
                    InstrumentId = activityDetailsViewModel.InstrumentId,
                    IsActive = true,
                    IsDeleted = false,
                    InstrumentName = activityDetailsViewModel.InstrumentName,
                    InstrumentSerial = activityDetailsViewModel.InstrumentSerial,
                    Model = activityDetailsViewModel.Model,
                    LocationName = activityDetailsViewModel.LocationName,
                    InstrumentActivityId = activityDetailsViewModel.InstrumentActivityId,
                    Days = activityDetailsViewModel.Days,
                    Frequency = activityDetailsViewModel.Frequency,
                    TeamName = activityDetailsViewModel.TeamName,
                    TeamLocation = activityDetailsViewModel.TeamLocation,
                    RequirementName = activityDetailsViewModel.RequirementName,
                    DeaprtmentName = activityDetailsViewModel.DeaprtmentName,
                    Remark = activityDetailsViewModel.Remark,
                    PerformedDate = activityDetailsViewModel.PerformedDate,
                };
                var validation = new ActivityDetailsValidator();
                var result = validation.Validate(activityDetail);
                if (!result.IsValid)
                {
                    List<InstrumentIdViewModel> instrumentId = new List<InstrumentIdViewModel>();
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                        HttpResponseMessage Res = await client.GetAsync("InstrumentId/GetActiveInstrumentId");

                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            instrumentId = root["result"].ToObject<List<InstrumentIdViewModel>>();
                        }
                    }
                    var data = instrumentId.Select(x => new SelectListItem
                    {
                        Text = x.InstrumentsId.ToString(),
                        Value = x.Id.ToString()
                    }).ToList();

                    List<InstrumentActivityViewModel> instrumentActivityViewModels = new List<InstrumentActivityViewModel>();
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                        HttpResponseMessage Res = await client.GetAsync("InstrumentActivity/GetActiveInstrumentActivity");

                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            instrumentActivityViewModels = root["result"].ToObject<List<InstrumentActivityViewModel>>();
                        }
                    }
                    var instrumentActivities = instrumentActivityViewModels.Select(x => new SelectListItem
                    {
                        Text = x.InstrumentActivityName.ToString(),
                        Value = x.Id.ToString()
                    }).ToList();
                    activityDetailsViewModel.InstrumentsActivity = instrumentActivities;
                    activityDetailsViewModel.InstrumentNameId = data;
                    activityDetailsViewModel.PerformedDate = DateTime.UtcNow;
                    if (activityDetailsViewModel.Id > 0)
                        return View("EditActivityDetails", activityDetailsViewModel);
                    else
                        return View(activityDetailsViewModel);
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    
                        var json = JsonConvert.SerializeObject(activityDetail);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                        HttpResponseMessage Res = await client.PostAsync("ActivityDetails/PostActivityDetails", stringContent);

                        if (Res.IsSuccessStatusCode)
                        {
                            var contentDataResult = await Res.Content.ReadAsStringAsync();
                        if (activityDetailsViewModel.Files != null)
                        {
                            if (activityDetailsViewModel.Files.Count > 0)
                            {
                                var FileDic = "Documents/ActivityDetails/Image/";
                                string FilePath = Path.Combine(hostingEnv.ContentRootPath, FileDic);

                                var DocumentDic = "Documents/ActivityDetails/Document";
                                string FileDocumentPath = Path.Combine(hostingEnv.ContentRootPath, DocumentDic);

                                if (!Directory.Exists(FilePath))
                                    Directory.CreateDirectory(FilePath);

                                if (!Directory.Exists(FileDocumentPath))
                                    Directory.CreateDirectory(FileDocumentPath);

                                if (activityDetailsViewModel.Files != null)
                                {
                                    foreach (var data in activityDetailsViewModel.Files)
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
                            { "IsActive", activityDetail.IsActive.ToString()},
                            { "IsDeleted", activityDetail.IsDeleted.ToString()},
                            { "InstrumentName", activityDetail.InstrumentName},
                            {"InstrumentSerial",activityDetail.InstrumentSerial },
                            {"Model" ,activityDetail.Model},
                            {"LocationName" ,activityDetail.LocationName},
                            { "TeamName",activityDetail.TeamName},
                            {"TeamLocation",activityDetail.TeamLocation },
                            {"Days",activityDetail.Days.ToString() },
                            { "Frequency", activityDetail.Frequency },
                            {"Remark",activityDetail.Remark },
                            {"PerformedDate" ,activityDetail.PerformedDate.ToString()},
                            {"myeumjson",activityDetail.myeumjson },
                                };
                                var formContent = new MultipartFormDataContent();

                                foreach (var keyValuePair in formData)
                                {
                                    formContent.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                                }

                                foreach (var file in activityDetailsViewModel.Files)
                                {
                                    formContent.Add(new StreamContent(file.OpenReadStream())
                                    {
                                        Headers = { ContentLength = file.Length, ContentType = new MediaTypeHeaderValue(file.ContentType) }
                                    },
                                    "Files",
                                    file.FileName);
                                }
                                HttpResponseMessage response = await client.PostAsync("ActivityDetails/UploadActivityDetailsDocument/", formContent);
                                if (response.IsSuccessStatusCode)
                                {
                                    var contentData = await response.Content.ReadAsStringAsync();
                                    JObject root = JObject.Parse(contentData);
                                }
                            }
                        }
                            JObject rootResult = JObject.Parse(contentDataResult);
                            var resultDataSuccess = rootResult["success"].ToString();
                            if (resultDataSuccess == "False" || resultDataSuccess == "false")
                            {
                                var Id = rootResult["auditId"].ToString();
                                TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                                TempData["RedirectURl"] = "/ActivityDetails/AddActivityDetails/";
                            }
                            else
                            {
                                TempData["message"] = languageService.Getkey("ActivityDetails Added Successfully");
                                TempData["RedirectURl"] = "/ActivityDetails/GetAllActivityDetails/";
                            }
                        }
                    }
                    return View(activityDetailsViewModel);
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> GetInstrumentActivityDetail(string id)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                ActivityDetailsViewModel activityDetailsViewModel = new ActivityDetailsViewModel();
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
                    HttpResponseMessage response = await client.GetAsync(string.Format("InstrumentActivity/GetInstrumentActivityDetailData/{0}", id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        activityDetailsViewModel = root["result"].ToObject<ActivityDetailsViewModel>();
                    }
                }
                return Json(activityDetailsViewModel);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> GetInstrumentIdDetail(string id)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                ActivityDetailsViewModel activityDetailsViewModel=new ActivityDetailsViewModel();
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
                    HttpResponseMessage response = await client.GetAsync(string.Format("InstrumentId/GetInstrumentIdDetail/{0}", id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        activityDetailsViewModel = root["result"].ToObject<ActivityDetailsViewModel>();
                    }
                }
                return Json(activityDetailsViewModel);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }

        public async Task<IActionResult> EditActivityDetails(long id)
        {
            try
            {
                ViewBag.AllowedFileExtension = HttpContext.Session.GetString("FileType");
                ViewBag.AlloweFileSize = HttpContext.Session.GetString("FileSize");
                string apiUrl = configuration["Baseurl"];
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                List<InstrumentIdViewModel> instrumentId = new List<InstrumentIdViewModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("InstrumentId/GetActiveInstrumentId");

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        instrumentId = root["result"].ToObject<List<InstrumentIdViewModel>>();
                    }
                }
                var data = instrumentId.Select(x => new SelectListItem
                {
                    Text = x.InstrumentsId.ToString(),
                    Value = x.Id.ToString()
                }).ToList();

                List<InstrumentActivityViewModel> instrumentActivityViewModels = new List<InstrumentActivityViewModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("InstrumentActivity/GetActiveInstrumentActivity");

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        instrumentActivityViewModels = root["result"].ToObject<List<InstrumentActivityViewModel>>();
                    }
                }
                var instrumentActivities = instrumentActivityViewModels.Select(x => new SelectListItem
                {
                    Text = x.InstrumentActivityName.ToString(),
                    Value = x.Id.ToString()
                }).ToList();
                

                ActivityDetailsViewModel activityViewModel = new ActivityDetailsViewModel();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.GetAsync(string.Format("ActivityDetails/GetActivityDetails/{0}", id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var Id = root["auditId"].ToString();
                            TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                            TempData["RedirectURl"] = "/ActivityDetails/AddActivityDetails/";
                        }
                        else
                        {
                            activityViewModel = root["result"].ToObject<ActivityDetailsViewModel>();
                        }
                    }
                }
                ActivityDetailsDocumentViewModel activityDetailsDocumentViewModel = new ActivityDetailsDocumentViewModel();
                activityDetailsDocumentViewModel.InstrumentsActivity = instrumentActivities;
                activityDetailsDocumentViewModel.InstrumentNameId = data;
                activityDetailsDocumentViewModel.InstrumentId = activityViewModel.InstrumentId;
                activityDetailsDocumentViewModel.InstrumentActivityId= activityViewModel.InstrumentActivityId;
                activityDetailsDocumentViewModel.User = activityViewModel.User;
                activityDetailsDocumentViewModel.InstrumentName= activityViewModel.InstrumentName;
                activityDetailsDocumentViewModel.InstrumentSerial = activityViewModel.InstrumentSerial;
                activityDetailsDocumentViewModel.Model= activityViewModel.Model;
                activityDetailsDocumentViewModel.LocationName = activityViewModel.LocationName;
                activityDetailsDocumentViewModel.TeamName   = activityViewModel.TeamName;
                activityDetailsDocumentViewModel.Days=activityViewModel.Days;
                activityDetailsDocumentViewModel.Frequency=activityViewModel.Frequency;
                activityDetailsDocumentViewModel.Remark=activityViewModel.Remark;
                activityDetailsDocumentViewModel.PerformedDate=activityViewModel.PerformedDate;
                activityDetailsDocumentViewModel.DeaprtmentName=activityViewModel.DeaprtmentName;
                activityDetailsDocumentViewModel.IsActive=activityViewModel.IsActive;
                activityDetailsDocumentViewModel.IsDeleted  =activityViewModel.IsDeleted;
                activityDetailsDocumentViewModel.myeumjson=activityViewModel.myeumjson;
                activityDetailsDocumentViewModel.ActivityDetails=activityViewModel.ActivityDetails;
                activityDetailsDocumentViewModel.UserId = activityViewModel.UserId;
                activityDetailsDocumentViewModel.Id=activityViewModel.Id;
                activityDetailsDocumentViewModel.TeamLocation = activityViewModel.TeamLocation;
                activityDetailsDocumentViewModel.RequirementName=activityViewModel.RequirementName;
                var documentData = (string[])null;
                var documentIdsData = (string[])null;
                if (!string.IsNullOrEmpty(activityViewModel.Path))
                {
                    var documents = activityViewModel.Path.Split(",");
                    documentData = documents;
                }
                if (!string.IsNullOrEmpty(activityViewModel.DocumentId))
                {
                    var documentIds = activityViewModel.DocumentId.Split(",");
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
                        activityDetailsDocumentViewModel.UplodedFile.Add(file);
                    }
                }
                return View(activityDetailsDocumentViewModel);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }

        public async Task<IActionResult> DeleteActivityDetails(long id)
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
                    HttpResponseMessage response = await client.DeleteAsync(string.Format("ActivityDetails/DeleteActivityDetails/{0}", id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var Id = root["auditId"].ToString();
                            TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;

                        }
                        else
                        {
                            TempData["message"] = languageService.Getkey("ActivityDetails Deleted Successfully");
                            TempData["RedirectURl"] = "/ActivityDetails/GetAllActivityDetails/";
                        }
                        //var message = root["value"].ToString();
                    }
                }
                return RedirectToAction("GetAllActivityDetails");
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
                path = Path.Combine(hostingEnv.ContentRootPath, "Documents/ActivityDetails/Image/" + name);
            }
            else
            {
                path = Path.Combine(hostingEnv.ContentRootPath, "Documents/ActivityDetails/Document/" + name);

            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, name);
        }
        public async Task<IActionResult> DeleteDocument(long documentId, long activityDetailId)
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
                    HttpResponseMessage response = await client.DeleteAsync(string.Format("ActivityDetails/DeleteDocument/{0}/{1}", documentId, activityDetailId));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var Id = root["auditId"].ToString();
                            TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;

                        }
                        else
                        {
                            TempData["message"] = languageService.Getkey("Document Deleted Successfully");
                        }
                    }
                }
                return RedirectToAction("GetAllActivityDetails");
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
    }
}
