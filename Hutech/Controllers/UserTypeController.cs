using Hutech.Models;
using Hutech.Resources;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Hutech.Controllers
{
    public class UserTypeController : Controller
    {
        public IConfiguration configuration { get; set; }
        private readonly ILogger<UserTypeController> logger;
        private readonly LanguageService languageService;
        public UserTypeController(IConfiguration _configuration, ILogger<UserTypeController> _logger, LanguageService _languageService)
        {
            configuration = _configuration;
            logger = _logger;
            languageService = _languageService;
        }
        public async Task<IActionResult> GetAllUserType(int pageNumber=1)
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
                List<UserTypeViewModel> userTypes = new List<UserTypeViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.GetAsync(string.Format("UserType/GetUserType/{0}",pageNumber));

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
                            userTypes = root["result"].ToObject<List<UserTypeViewModel>>();
                            pageNumber = (int)root["currentPage"];
                            totalRecords = (int)root["totalRecords"];
                            totalPage = (int)root["totalPage"];
                        }
                    }
                }
                var data = new GridData<UserTypeViewModel>()
                {
                    CurrentPage = pageNumber,
                    GridRecords = userTypes,
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
        public IActionResult AddUserType()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserType(UserTypeViewModel model)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                var validation = new UserTypeValidator();
                var result = validation.Validate(model);
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
                        model.IsDeleted = false;
                        model.CreatedByUserId = HttpContext.Session.GetString("UserId");
                        var json = JsonConvert.SerializeObject(model);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                        HttpResponseMessage Res = await client.PostAsync("UserType/PostUserType", stringContent);

                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            var resultData = root["success"].ToString();
                            if (resultData == "False" || resultData == "false")
                            {
                                var Id = root["auditId"].ToString();
                                TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                                TempData["RedirectURl"] = "/UserType/AddUserType/";
                            }
                            else
                            {
                                TempData["message"] = languageService.Getkey("UserType Added Successfully");
                                TempData["RedirectURl"] = "/UserType/GetAllUserType/";
                            }
                        }
                    }
                    //return RedirectToAction("GetAllLocation");
                    return View();
                }
            }
            catch(Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;

            }
        }
        public async Task<IActionResult> DeleteUserType(int id)
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
                    HttpResponseMessage response = await client.DeleteAsync(string.Format("UserType/DeleteUserType/{0}", id));

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
                            TempData["message"] = languageService.Getkey("UserType Deleted Successfully");
                            TempData["RedirectURl"] = "/UserType/GetAllUserType/";
                        }
                        //var message = root["value"].ToString();
                    }
                }
                return RedirectToAction("GetAllUserType");
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;

            }
        }
        public async Task<IActionResult> EditUserType(int Id)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                UserTypeViewModel userTypeViewModel = new UserTypeViewModel();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.GetAsync(string.Format("UserType/GetUserTypeDetail/{0}", Id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var id = root["auditId"].ToString();
                            TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + id;
                            TempData["RedirectURl"] = "/UserType/AddUserType/";
                        }
                        else
                        {
                            userTypeViewModel = root["result"].ToObject<UserTypeViewModel>();
                        }
                    }
                }
                return View(userTypeViewModel);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
    }
}
