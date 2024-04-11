using Hutech.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Hutech.Core.Constants;
using System.IdentityModel.Tokens.Jwt;
using Hutech.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hutech.Controllers
{
    [Authorize]
    public class ActivityController : Controller
    {
        public IConfiguration configuration { get; set; }
        private readonly ILogger<ActivityController> logger;
        private readonly LanguageService languageService;
        public ActivityController(IConfiguration _configuration, ILogger<ActivityController> _logger, LanguageService _languageService)
        {
            configuration = _configuration;
            logger = _logger;
            languageService = _languageService;
        }
        [HttpPost]
        public async Task<IActionResult> GetAllActivity([FromBody] ActivityModel activityModel)
        {
            activityModel.PageNumber = 1;
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
                List<ActivityViewModel> activities = new List<ActivityViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var json = JsonConvert.SerializeObject(activityModel);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    HttpResponseMessage Res = await client.PostAsync("Activity/GetAllFilterActivity", stringContent);

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
                            activities = root["result"].ToObject<List<ActivityViewModel>>();
                            pageNumber = (int)root["currentPage"];
                            totalRecords = (int)root["totalRecords"];
                            totalPage = (int)root["totalPage"];
                        }
                    }
                }
                var data = new ActivitiesViewModel()
                {
                    CurrentPage = pageNumber,
                    ActivityViewModels = activities,
                    TotalPages = totalPage,
                    TotalRecords = totalRecords
                };
                data.ActivityName = !string.IsNullOrEmpty(activityModel.ActivityName) ? activityModel.ActivityName : "";
                data.UpdatedBy = !string.IsNullOrEmpty(activityModel.UpdatedBy) ? activityModel.UpdatedBy : "";
                //data.Status = locationModel.status;
                data.UpdatedDate = activityModel.UpdatedDate;
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
                data.SelectedStatus = System.Convert.ToInt32(activityModel.Status);
                string requestedWithHeader = Request.Headers["X-Requested-With"];
                return View("GetAllActivity", data);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> GetAllActivity(int pageNumber = 1, string? activityName = null, string? updatedBy = null, string? updatedDate = null, string? SelectedStatus = null)
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
                ActivitiesViewModel activitiesViewModel = new ActivitiesViewModel();
                List<ActivityViewModel> activities = new List<ActivityViewModel>();
                string apiUrl = configuration["Baseurl"];
                ActivityModel activityModel = new ActivityModel();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    activityModel.PageNumber = pageNumber;
                    activityModel.ActivityName = activityName;
                    activityModel.UpdatedBy = updatedBy;
                    if (!string.IsNullOrEmpty(updatedDate))
                        activityModel.UpdatedDate = DateTime.Parse(updatedDate);
                    activityModel.Status = SelectedStatus;
                    var json = JsonConvert.SerializeObject(activityModel);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    HttpResponseMessage Res = await client.PostAsync("Activity/GetAllFilterActivity", stringContent);

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
                            activities = root["result"].ToObject<List<ActivityViewModel>>();
                            pageNumber = (int)root["currentPage"];
                            totalRecords = (int)root["totalRecords"];
                            totalPage = (int)root["totalPage"];
                        }
                    }
                }
                var data = new ActivitiesViewModel()
                {
                    CurrentPage = pageNumber,
                    ActivityViewModels = activities,
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
                data.ActivityName = !string.IsNullOrEmpty(activityModel.ActivityName) ? activityModel.ActivityName : "";
                data.UpdatedBy = !string.IsNullOrEmpty(activityModel.UpdatedBy) ? activityModel.UpdatedBy : "";
                data.UpdatedDate = activityModel.UpdatedDate;
                return View(data);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public IActionResult AddActivity()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddActivity(ActivityViewModel activityViewModel)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                var validation = new ActivityValidator();
                var result = validation.Validate(activityViewModel);
                if (!result.IsValid)
                {
                    return View();
                }
                else
                {
                    string apiUrl = configuration["Baseurl"];
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        activityViewModel.IsDeleted = false;
                        activityViewModel.DatecreatedUtc = DateTime.UtcNow;
                        activityViewModel.DateModifiedUtc=DateTime.UtcNow;
                        activityViewModel.CreatedByUserId = HttpContext.Session.GetString("UserId");
                        var json = JsonConvert.SerializeObject(activityViewModel);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                        HttpResponseMessage Res = await client.PostAsync("Activity/PostActivity", stringContent);

                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            var resultData = root["success"].ToString();
                            if (resultData == "False" || resultData == "false")
                            {
                                var Id = root["auditId"].ToString();
                                TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                                TempData["RedirectURl"] = "/Activity/AddActivity/";
                            }
                            else
                            {
                                TempData["message"] = languageService.Getkey("Activity Added Successfully");
                                TempData["RedirectURl"] = "/Activity/GetAllActivity/";
                            }
                        }
                    }
                    return RedirectToAction("GetAllActivity");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> EditActivity(long id)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                ActivityViewModel activityViewModel = new ActivityViewModel();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.GetAsync(string.Format("Activity/GetActivityDetail/{0}", id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var Id = root["auditId"].ToString();
                            TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                            TempData["RedirectURl"] = "/Activity/AddActivity/";
                        }
                        else
                        {
                            activityViewModel = root["result"].ToObject<ActivityViewModel>();
                        }
                    }
                }
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
        public async Task<IActionResult> EditActivity(ActivityViewModel activityViewModel)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                var validation = new ActivityValidator();
                var result = validation.Validate(activityViewModel);
                if (!result.IsValid)
                {
                    return View();
                }
                else
                {
                    string apiUrl = configuration["Baseurl"];
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        activityViewModel.DateModifiedUtc = DateTime.UtcNow;
                        activityViewModel.ModifiedByUserId = HttpContext.Session.GetString("UserId");
                        var json = JsonConvert.SerializeObject(activityViewModel);
                        var stringcontenet = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpResponseMessage response = await client.PutAsync("Activity/PutActivity", stringcontenet);

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            var resultData = root["success"].ToString();
                            if (resultData == "False" || resultData == "false")
                            {
                                var Id = root["auditId"].ToString();
                                TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                                //TempData["RedirectURl"] = "/Activity/EditActivity/";
                                return RedirectToAction("EditActivity", new { id = activityViewModel.Id });

                            }
                            else
                            {
                                activityViewModel = root["result"].ToObject<ActivityViewModel>();
                                TempData["message"] = languageService.Getkey("Activity Updated Successfully");
                                TempData["RedirectURl"] = "/Activity/GetAllActivity/";
                            }
                        }
                    }
                    return RedirectToAction("GetAllActivity");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> DeleteActivity(long id)
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
                    HttpResponseMessage response = await client.DeleteAsync(string.Format("Activity/DeleteActivity/{0}", id));

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
                            TempData["message"] = languageService.Getkey("Activity Deleted Successfully");
                            TempData["RedirectURl"] = "/Activity/GetAllActivity/";
                        }
                        //var message = root["value"].ToString();
                    }
                }
                return RedirectToAction("GetAllActivity");
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
    }
}
