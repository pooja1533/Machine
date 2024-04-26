using Hutech.Models;
using Hutech.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;

namespace Hutech.Controllers
{
    [Authorize]
    public class InstrumentActivityController : Controller
    {
        public IConfiguration configuration { get; set; }
        private readonly ILogger<InstrumentActivityController> logger;
        private readonly LanguageService languageService;
        public InstrumentActivityController(IConfiguration _configuration, ILogger<InstrumentActivityController> _logger, LanguageService _languageService)
        {
            logger = _logger;
            configuration = _configuration;
            languageService = _languageService;
        }
        public async Task<IActionResult> AddInstrumentActivity()
        {
            InstrumentActivityViewModel instrumentActivityViewModel = new InstrumentActivityViewModel();
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                List<InstrumentViewModel> instrument = new List<InstrumentViewModel>();
                string apiUrl = configuration["Baseurl"];
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


                List<ActivityViewModel> activities = new List<ActivityViewModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("Activity/GetActiveActivity");

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        activities = root["result"].ToObject<List<ActivityViewModel>>();
                    }
                }
                var activity = activities.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();

                List<RequirementViewModel> requirements = new List<RequirementViewModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("Requirement/GetActiveRequirement");

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        requirements = root["result"].ToObject<List<RequirementViewModel>>();
                    }
                }
                var requirementdata = requirements.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();

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
                var departmentdata = departments.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();
                List<GroupViewModel> groups = new List<GroupViewModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var loggedinUserRole = HttpContext.Session.GetString("UserRole").ToString();
                    var loggedinuserId = HttpContext.Session.GetString("UserId").ToString();
                    HttpResponseMessage Res = await client.GetAsync(string.Format("Group/GetAllActiveGroup/"));

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        groups = root["result"].ToObject<List<GroupViewModel>>();
                    }
                }
                var userData = groups.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();
                instrumentActivityViewModel.EmailList = userData;
                instrumentActivityViewModel.Requirement = requirementdata;
                instrumentActivityViewModel.Instruments = data;
                instrumentActivityViewModel.Activities = activity;
                instrumentActivityViewModel.Department = departmentdata;
                return View(instrumentActivityViewModel);

            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddInstrumentActivity(InstrumentActivityViewModel instrumentActivityViewModel)
        {
            var token = Request.Cookies["jwtCookie"];
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();

                token = token.Replace("Bearer ", "");
            }
            var validation = new InstrumentActivityValidator();
            var result = validation.Validate(instrumentActivityViewModel);
            string apiUrl = configuration["Baseurl"];
            List<GroupViewModel> group = new List<GroupViewModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var loggedinUserRole = HttpContext.Session.GetString("UserRole").ToString();
                var loggedinuserId = HttpContext.Session.GetString("UserId").ToString();
                HttpResponseMessage Res = await client.GetAsync(string.Format("Group/GetAllActiveGroup/"));

                if (Res.IsSuccessStatusCode)
                {
                    var content = await Res.Content.ReadAsStringAsync();
                    JObject root = JObject.Parse(content);
                    group = root["result"].ToObject<List<GroupViewModel>>();
                }
            }
            var userData = group.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            //instrumentActivityViewModel.EmailList = userData;

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

            List<ActivityViewModel> activities = new List<ActivityViewModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                HttpResponseMessage Res = await client.GetAsync("Activity/GetActiveActivity");

                if (Res.IsSuccessStatusCode)
                {
                    var content = await Res.Content.ReadAsStringAsync();
                    JObject root = JObject.Parse(content);
                    activities = root["result"].ToObject<List<ActivityViewModel>>();
                }
            }
            var activity = activities.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            List<RequirementViewModel> requirements = new List<RequirementViewModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                HttpResponseMessage Res = await client.GetAsync("Requirement/GetActiveRequirement");

                if (Res.IsSuccessStatusCode)
                {
                    var content = await Res.Content.ReadAsStringAsync();
                    JObject root = JObject.Parse(content);
                    requirements = root["result"].ToObject<List<RequirementViewModel>>();
                }
            }
            var requirementdata = requirements.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

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
            var departmentdata = departments.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            instrumentActivityViewModel.CreatedDateTime= DateTime.UtcNow;
            if (!result.IsValid)
            {
                instrumentActivityViewModel.EmailList = userData;
                instrumentActivityViewModel.Requirement = requirementdata;
                instrumentActivityViewModel.Instruments = data;
                instrumentActivityViewModel.Activities = activity;
                instrumentActivityViewModel.Department = departmentdata;
                return View(instrumentActivityViewModel);
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
                        instrumentActivityViewModel.CreatedByUserId = HttpContext.Session.GetString("UserId");
                        instrumentActivityViewModel.CreatedDateTime = DateTime.UtcNow;
                        instrumentActivityViewModel.ModifiedDateTime = DateTime.UtcNow;
                        var json = JsonConvert.SerializeObject(instrumentActivityViewModel);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                        HttpResponseMessage Res = await client.PostAsync("InstrumentActivity/PostInstrumentActivity", stringContent);

                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            var resultData = root["success"].ToString();
                            if (resultData == "False" || resultData == "false")
                            {
                                var Id = root["auditId"].ToString();
                                string message= languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                                TempData["message"] = message;
                                TempData["RedirectURl"] = "/InstrumentActivity/AddInstrumentActivity/";
                            }
                            else
                            {
                                string message= languageService.Getkey("InstrumentActivity Added Successfully");
                                TempData["message"] = message;
                                TempData["RedirectURl"] = "/InstrumentActivity/GetAllInstrumentActivity/";
                            }
                        }
                    }
                    return RedirectToAction("GetAllInstrumentActivity");
                }

                catch (Exception ex)
                {
                    logger.LogInformation($"Exception Occure.{ex.Message}");
                    throw ex;
                }
            }
        }

        public async Task<IActionResult> DeleteInstrumentActivity(long id)
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
                    HttpResponseMessage response = await client.DeleteAsync(string.Format("InstrumentActivity/DeleteInstrumentActivity/{0}", id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var Id = root["auditId"].ToString();
                            string message= languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                            TempData["message"] = message;

                        }
                        else
                        {
                            string message= languageService.Getkey("InstrumentActivity Deleted Successfully");
                            TempData["message"] = message;
                            TempData["RedirectURl"] = "/InstrumentActivity/GetAllInstrumentActivity/";
                        }
                        //var message = root["value"].ToString();
                    }
                }
                return RedirectToAction("GetAllInstrumentActivity");
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetAllInstrumentActivity([FromBody] InstrumentActivityModel instrumentActivityModel)
        {
            instrumentActivityModel.pageNumber = 1;
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
                List<InstrumentActivityViewModel> instrumentActivityViewModels = new List<InstrumentActivityViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var json = JsonConvert.SerializeObject(instrumentActivityModel);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    HttpResponseMessage Res = await client.PostAsync("InstrumentActivity/GetAllFilterInstrumentActivity", stringContent);

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var Id = root["auditId"].ToString();
                            string message= languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                            TempData["message"] = message;
                        }
                        else
                        {
                            instrumentActivityViewModels = root["result"].ToObject<List<InstrumentActivityViewModel>>();
                            pageNumber = (int)root["currentPage"];
                            totalRecords = (int)root["totalRecords"];
                            totalPage = (int)root["totalPage"];
                        }
                    }
                }
                var data = new InstrumentActivitiesViewModel()
                {
                    CurrentPage = pageNumber,
                    instrumentActivityViewModels = instrumentActivityViewModels,
                    TotalPages = totalPage,
                    TotalRecords = totalRecords
                };
                data.InstrumentActivityId = !string.IsNullOrEmpty(instrumentActivityModel.InstrumentActivityId) ? instrumentActivityModel.InstrumentActivityId : "";
                data.ActivityName= !string.IsNullOrEmpty(instrumentActivityModel.ActivityName) ? instrumentActivityModel.ActivityName : "";
                data.InstrumentName = !string.IsNullOrEmpty(instrumentActivityModel.InstrumentName) ? instrumentActivityModel.InstrumentName : "";
                data.RequirementName = !string.IsNullOrEmpty(instrumentActivityModel.RequirementName) ? instrumentActivityModel.RequirementName : "";
                data.DepartmentName = !string.IsNullOrEmpty(instrumentActivityModel.DepartmentName) ? instrumentActivityModel.DepartmentName : "";
                data.UpdatedBy = !string.IsNullOrEmpty(instrumentActivityModel.UpdatedBy) ? instrumentActivityModel.UpdatedBy : "";
                data.UpdatedDate = instrumentActivityModel.UpdatedDate;
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
                data.SelectedStatus = System.Convert.ToInt32(instrumentActivityModel.Status);
                string requestedWithHeader = Request.Headers["X-Requested-With"];
                data.InstrumentActivityId = !string.IsNullOrEmpty(instrumentActivityModel.InstrumentActivityId) ? instrumentActivityModel.InstrumentActivityId : "";
                data.ActivityName = !string.IsNullOrEmpty(instrumentActivityModel.ActivityName) ? instrumentActivityModel.ActivityName : "";
                data.InstrumentName = !string.IsNullOrEmpty(instrumentActivityModel.InstrumentName) ? instrumentActivityModel.InstrumentName : "";
                data.RequirementName = !string.IsNullOrEmpty(instrumentActivityModel.RequirementName) ? instrumentActivityModel.RequirementName : "";
                data.DepartmentName = !string.IsNullOrEmpty(instrumentActivityModel.DepartmentName) ? instrumentActivityModel.DepartmentName : "";
                data.UpdatedBy = !string.IsNullOrEmpty(instrumentActivityModel.UpdatedBy) ? instrumentActivityModel.UpdatedBy : "";
                data.UpdatedDate = instrumentActivityModel.UpdatedDate;
                return View("GetAllInstrumentActivity", data);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> GetAllInstrumentActivity(int pageNumber = 1, string? instrumentActivityId = null,string? activityName=null,string? instrumentName=null,string? requirementName=null,string? departmentName=null, string? updatedBy = null, string? updatedDate = null, string? SelectedStatus = null)
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
                InstrumentActivitiesViewModel models=new InstrumentActivitiesViewModel();
                List<InstrumentActivityViewModel> instrumentactivities = new List<InstrumentActivityViewModel>();
                InstrumentActivityModel instrumentActivityModel=new InstrumentActivityModel();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    instrumentActivityModel.pageNumber = pageNumber;
                    instrumentActivityModel.InstrumentActivityId = instrumentActivityId;
                    instrumentActivityModel.InstrumentName = instrumentName;
                    instrumentActivityModel.UpdatedBy = updatedBy;
                    instrumentActivityModel.ActivityName = activityName;
                    instrumentActivityModel.RequirementName = requirementName;
                    instrumentActivityModel.DepartmentName = departmentName;
                    if (!string.IsNullOrEmpty(updatedDate))
                        instrumentActivityModel.UpdatedDate = DateTime.Parse(updatedDate);
                    instrumentActivityModel.Status = SelectedStatus;
                    var json = JsonConvert.SerializeObject(instrumentActivityModel);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    HttpResponseMessage Res = await client.PostAsync("InstrumentActivity/GetAllFilterInstrumentActivity", stringContent);

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var Id = root["auditId"].ToString();
                            string message= languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                            TempData["message"] = message;
                        }
                        else
                        {
                            instrumentactivities = root["result"].ToObject<List<InstrumentActivityViewModel>>();
                            pageNumber = (int)root["currentPage"];
                            totalRecords = (int)root["totalRecords"];
                            totalPage = (int)root["totalPage"];
                        }
                    }
                }
                var data = new InstrumentActivitiesViewModel()
                {
                    CurrentPage = pageNumber,
                    instrumentActivityViewModels = instrumentactivities,
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
                if (SelectedStatus == null)
                    data.SelectedStatus = (int)StatusViewModel.Active;
                else if (!string.IsNullOrEmpty(SelectedStatus))
                    data.SelectedStatus = System.Convert.ToInt32(SelectedStatus);
                data.InstrumentActivityId = !string.IsNullOrEmpty(instrumentActivityModel.InstrumentActivityId) ? instrumentActivityModel.InstrumentActivityId : "";
                data.ActivityName = !string.IsNullOrEmpty(instrumentActivityModel.ActivityName) ? instrumentActivityModel.ActivityName : "";
                data.InstrumentName = !string.IsNullOrEmpty(instrumentActivityModel.InstrumentName) ? instrumentActivityModel.InstrumentName : "";
                data.RequirementName = !string.IsNullOrEmpty(instrumentActivityModel.RequirementName) ? instrumentActivityModel.RequirementName : "";
                data.DepartmentName = !string.IsNullOrEmpty(instrumentActivityModel.DepartmentName) ? instrumentActivityModel.DepartmentName : "";
                data.UpdatedBy = !string.IsNullOrEmpty(instrumentActivityModel.UpdatedBy) ? instrumentActivityModel.UpdatedBy : "";
                data.UpdatedDate = instrumentActivityModel.UpdatedDate;
                return View(data);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> EditInstrumentActivity(long id)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                InstrumentActivityViewModel activityViewModel = new InstrumentActivityViewModel();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.GetAsync(string.Format("InstrumentActivity/GetInstrumentActivity/{0}", id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var Id = root["auditId"].ToString();
                            string message= languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                            TempData["message"] = message;
                            TempData["RedirectURl"] = "/InstrumentActivity/AddInstrumentActivity/";
                        }
                        else
                        {
                            activityViewModel = root["result"].ToObject<InstrumentActivityViewModel>();
                        }
                    }
                }

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


                List<ActivityViewModel> activities = new List<ActivityViewModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("Activity/GetActiveActivity");

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        activities = root["result"].ToObject<List<ActivityViewModel>>();
                    }
                }
                var activity = activities.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();

                List<RequirementViewModel> requirements = new List<RequirementViewModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("Requirement/GetActiveRequirement");

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        requirements = root["result"].ToObject<List<RequirementViewModel>>();
                    }
                }
                var requirementdata = requirements.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();

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
                var departmentdata = departments.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();
                List<GroupViewModel> groups = new List<GroupViewModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var loggedinUserRole = HttpContext.Session.GetString("UserRole").ToString();
                    var loggedinuserId = HttpContext.Session.GetString("UserId").ToString();
                    HttpResponseMessage Res = await client.GetAsync(string.Format("Group/GetAllActiveGroup/"));

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        groups = root["result"].ToObject<List<GroupViewModel>>();
                    }
                }
                var userData = groups.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();
                activityViewModel.EmailList = userData;
                activityViewModel.Requirement = requirementdata;
                activityViewModel.Instruments = data;
                activityViewModel.Activities = activity;
                activityViewModel.Department = departmentdata;
                activityViewModel.SelectedEmailListInt = !string.IsNullOrEmpty(activityViewModel.SelectedGroups) ? activityViewModel.SelectedGroups?.Split(",").ToList(): new List<string>(new string[] { });
                return View(activityViewModel);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInstrumentActivity(InstrumentActivityViewModel instrumentActivityViewModel)
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
                var validation = new InstrumentActivityValidator();
                var result = validation.Validate(instrumentActivityViewModel);
                if (!result.IsValid)
                {
                    List<GroupViewModel> group = new List<GroupViewModel>();
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                        var loggedinUserRole = HttpContext.Session.GetString("UserRole").ToString();
                        var loggedinuserId = HttpContext.Session.GetString("UserId").ToString();
                        HttpResponseMessage Res = await client.GetAsync(string.Format("Group/GetAllActiveGroup/"));

                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            group = root["result"].ToObject<List<GroupViewModel>>();
                        }
                    }
                    var userData = group.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }).ToList();
                    //instrumentActivityViewModel.EmailList = userData;

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

                    List<ActivityViewModel> activities = new List<ActivityViewModel>();
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                        HttpResponseMessage Res = await client.GetAsync("Activity/GetActiveActivity");

                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            activities = root["result"].ToObject<List<ActivityViewModel>>();
                        }
                    }
                    var activity = activities.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }).ToList();

                    List<RequirementViewModel> requirements = new List<RequirementViewModel>();
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                        HttpResponseMessage Res = await client.GetAsync("Requirement/GetActiveRequirement");

                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            requirements = root["result"].ToObject<List<RequirementViewModel>>();
                        }
                    }
                    var requirementdata = requirements.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }).ToList();

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
                    var departmentdata = departments.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }).ToList();
                    instrumentActivityViewModel.EmailList = userData;
                    instrumentActivityViewModel.Requirement = requirementdata;
                    instrumentActivityViewModel.Instruments = data;
                    instrumentActivityViewModel.Activities = activity;
                    instrumentActivityViewModel.Department = departmentdata;
                    return View(instrumentActivityViewModel);
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        instrumentActivityViewModel.ModifiedDateTime = DateTime.UtcNow;
                        instrumentActivityViewModel.ModifiedByUserId= HttpContext.Session.GetString("UserId");
                        var json = JsonConvert.SerializeObject(instrumentActivityViewModel);
                        var stringcontenet = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpResponseMessage response = await client.PutAsync("InstrumentActivity/PutInstrumentActivity", stringcontenet);

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            var resultData = root["success"].ToString();
                            if (resultData == "False" || resultData == "false")
                            {
                                var Id = root["auditId"].ToString();
                                string message= languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                                TempData["message"] = message;
                                TempData["RedirectURl"] = "/InstrumentActivity/EditInstrumentActivity/";
                                return RedirectToAction("EditInstrumentActivity", new { id = instrumentActivityViewModel.Id });

                            }
                            else
                            {
                                instrumentActivityViewModel = root["result"].ToObject<InstrumentActivityViewModel>();
                                string message= languageService.Getkey("InstrumentActivity Updated Successfully");
                                TempData["message"] = message;
                                TempData["RedirectURl"] = "/InstrumentActivity/GetAllInstrumentActivity/";
                            }
                        }
                    }
                    return RedirectToAction("GetAllInstrumentActivity");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }    
    }
}
