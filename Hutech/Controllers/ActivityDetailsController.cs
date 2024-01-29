using Hutech.Models;
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
        public ActivityDetailsController(IConfiguration _configuration, ILogger<ActivityDetailsController> _logger)
        {
            configuration = _configuration;
            logger = _logger;
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
        public async Task<IActionResult> GetAllActivityDetails(string userId)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
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
                    HttpResponseMessage Res = await client.GetAsync(string.Format("ActivityDetails/GetAllActivityDetails/{0}", userId));

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        activityDetails = root["result"].ToObject<List<ActivityDetailsViewModel>>();
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

                model.ActivityDetails = activityDetails;
                model.User = data;
                model.UserId= HttpContext.Session.GetString("SelectedUserId").ToString();
                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> AddActivityDetails()
        {
            ActivityDetailsViewModel activityDetailsViewModel = new ActivityDetailsViewModel();
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
           return View(activityDetailsViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddActivityDetails(ActivityDetailsViewModel activityDetailsViewModel)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                //var validation = new ActivityValidator();
                //var result = validation.Validate(activityViewModel);
                //if (!result.IsValid)
                //{
                //    return View();
                //}
                //else
                //{
                string apiUrl = configuration["Baseurl"];
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    activityDetailsViewModel.IsDeleted = false;
                        var json = JsonConvert.SerializeObject(activityDetailsViewModel);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                        HttpResponseMessage Res = await client.PostAsync("ActivityDetails/PostActivityDetails", stringContent);

                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                        }
                    }
                    return RedirectToAction("GetAllActivityDetails");
                //}
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
                        activityViewModel = root["result"].ToObject<ActivityDetailsViewModel>();
                    }
                }
                activityViewModel.InstrumentsActivity = instrumentActivities;
                activityViewModel.InstrumentNameId = data;
                return View(activityViewModel);
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
    }
}
