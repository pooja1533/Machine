using Hutech.Models;
using Hutech.Resources;
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
                                TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                                TempData["RedirectURl"] = "/InstrumentActivity/AddInstrumentActivity/";
                            }
                            else
                            {
                                TempData["message"] = languageService.Getkey("InstrumentActivity Added Successfully");
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
                            TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;

                        }
                        else
                        {
                            TempData["message"] = languageService.Getkey("InstrumentActivity Deleted Successfully");
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
        public async Task<IActionResult> GetAllInstrumentActivity()
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                List<InstrumentActivityViewModel> instrumentactivities = new List<InstrumentActivityViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("InstrumentActivity/GetAllInstrumentActivity");

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
                            instrumentactivities = root["result"].ToObject<List<InstrumentActivityViewModel>>();
                        }
                    }
                }
                return View(instrumentactivities);
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
                            TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
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
        public async Task<IActionResult> EditInstrumentActivity(InstrumentActivityViewModel activityViewModel)
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
                var result = validation.Validate(activityViewModel);
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
                    activityViewModel.EmailList = userData;
                    activityViewModel.Requirement = requirementdata;
                    activityViewModel.Instruments = data;
                    activityViewModel.Activities = activity;
                    activityViewModel.Department = departmentdata;
                    return View(activityViewModel);
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var json = JsonConvert.SerializeObject(activityViewModel);
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
                                TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                                TempData["RedirectURl"] = "/InstrumentActivity/EditInstrumentActivity/";
                                return RedirectToAction("EditInstrumentActivity", new { id = activityViewModel.Id });

                            }
                            else
                            {
                                activityViewModel = root["result"].ToObject<InstrumentActivityViewModel>();
                                TempData["message"] = languageService.Getkey("InstrumentActivity Updated Successfully");
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
