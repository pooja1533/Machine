using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using Hutech.Models;
using Hutech.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Hutech.Controllers
{
    [Authorize]
    public class LocationController : Controller
    {
        public IConfiguration configuration { get; set; }
        private readonly ILogger<LocationController> logger;
        private readonly LanguageService languageService;
        public LocationController(IConfiguration _configuration, ILogger<LocationController> _logger, LanguageService _languageService)
        {
            configuration = _configuration;
            logger = _logger;
            languageService = _languageService;
        }
        [HttpPost]
        public async Task<IActionResult> GetAllLocation([FromBody]LocationModel locationModel)
        {
            locationModel.pageNumber = 1;
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
                List<LocationViewModel> locations = new List<LocationViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var json = JsonConvert.SerializeObject(locationModel);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    HttpResponseMessage Res = await client.PostAsync("Location/GetAllFilterLocation", stringContent);

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
                            locations = root["result"].ToObject<List<LocationViewModel>>();
                            pageNumber = (int)root["currentPage"];
                            totalRecords = (int)root["totalRecords"];
                            totalPage = (int)root["totalPage"];
                        }
                    }
                }
                var data = new GridData<LocationViewModel>()
                {
                    CurrentPage = pageNumber,
                    GridRecords = locations,
                    TotalPages = totalPage,
                    TotalRecords = totalRecords
                };
                ViewBag.LocationName = locationModel.locationName;
                ViewBag.UpdatedBy = locationModel.updatedBy;
                ViewBag.status = locationModel.status;
                //string date = string.Empty;
                //if (locationModel.updatedDate!= null)
                //{
                //    date= locationModel.updatedDate?.ToString("MM-dd-yyyy");
                //}
                ViewBag.updatedDate = locationModel.updatedDate;
                string requestedWithHeader = Request.Headers["X-Requested-With"];
                return View("GetAllLocation",data);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> GetAllLocation(int pageNumber = 1)
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
                List<LocationViewModel> locations = new List<LocationViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.GetAsync(string.Format("Location/GetLocation/{0}",pageNumber));

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
                            locations = root["result"].ToObject<List<LocationViewModel>>();
                            pageNumber = (int)root["currentPage"];
                            totalRecords = (int)root["totalRecords"];
                            totalPage = (int)root["totalPage"];
                        }
                    }
                }
                var data = new GridData<LocationViewModel>()
                {
                    CurrentPage = pageNumber,
                    GridRecords = locations,
                    TotalPages = totalPage,
                    TotalRecords = totalRecords
                };
                return View(data);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public IActionResult AddLocation()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLocation(LocationViewModel locationViewModel)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                var validation = new LocationValidator();
                var result = validation.Validate(locationViewModel);
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
                        locationViewModel.IsDeleted = false;
                        locationViewModel.DatecreatedUtc = DateTime.UtcNow;
                        locationViewModel.DateModifiedUtc= DateTime.UtcNow;
                        locationViewModel.CreatedByUserId = HttpContext.Session.GetString("UserId");
                        var json = JsonConvert.SerializeObject(locationViewModel);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                        HttpResponseMessage Res = await client.PostAsync("Location/PostLocation", stringContent);

                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            var resultData = root["success"].ToString();
                            if (resultData == "False" || resultData == "false")
                            {
                                var Id = root["auditId"].ToString();
                                TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                                TempData["RedirectURl"] = "/Location/AddLocation/";
                            }
                            else
                            {
                                TempData["message"] = languageService.Getkey("Location Added Successfully");
                                TempData["RedirectURl"] = "/Location/GetAllLocation/";
                            }
                        }
                    }
                    //return RedirectToAction("GetAllLocation");
                    return View();
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> EditLocation(long id)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                LocationViewModel locationViewModel = new LocationViewModel();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.GetAsync(string.Format("Location/GetLocationDetail/{0}", id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var Id = root["auditId"].ToString();
                            TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                            TempData["RedirectURl"] = "/Location/AddLocation/";
                        }
                        else
                        {
                            locationViewModel = root["result"].ToObject<LocationViewModel>();
                        }
                    }
                }
                return View(locationViewModel);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLocation(LocationViewModel locationViewModel)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                var validation = new LocationValidator();
                var result = validation.Validate(locationViewModel);
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
                        locationViewModel.DateModifiedUtc = DateTime.UtcNow;
                        locationViewModel.ModifiedByUserId = HttpContext.Session.GetString("UserId");
                        var json = JsonConvert.SerializeObject(locationViewModel);
                        var stringcontenet = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpResponseMessage response = await client.PutAsync("Location/PutLocation", stringcontenet);

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            var resultData = root["success"].ToString();
                            if (resultData == "False" || resultData == "false")
                            {
                                var Id = root["auditId"].ToString();
                                TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                                TempData["RedirectURl"] = "/Location/EditLocation/";
                                return RedirectToAction("EditLocation", new { id = locationViewModel.Id });

                            }
                            else
                            {
                                locationViewModel = root["result"].ToObject<LocationViewModel>();
                                TempData["message"] = languageService.Getkey("Location Updated Successfully");
                                TempData["RedirectURl"] = "/Location/GetAllLocation/";
                            }
                        }
                    }
                    return RedirectToAction("GetAllLocation");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }

        }
        public async Task<IActionResult> DeleteLocation(long id)
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
                    HttpResponseMessage response = await client.DeleteAsync(string.Format("Location/DeleteLocation/{0}", id));

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
                            TempData["message"] = languageService.Getkey("Location Deleted Successfully");
                            TempData["RedirectURl"] = "/Location/GetAllLocation/";
                        }
                        //var message = root["value"].ToString();
                    }
                }
                return RedirectToAction("GetAllLocation");
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;

            }
        }
    }
}
