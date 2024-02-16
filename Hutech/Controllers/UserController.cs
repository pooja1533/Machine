using DocumentFormat.OpenXml.Wordprocessing;
using Hutech.Models;
using Hutech.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;

namespace Hutech.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<UserController> logger;
        private readonly LanguageService languageService;
        public UserController(IConfiguration _configuration, ILogger<UserController> _logger, LanguageService _languageService)
        {
            configuration = _configuration;
            logger = _logger;
            languageService = _languageService;
        }
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                var loggedinuser = HttpContext.Session.GetString("LoogedInUser");
                logger.LogInformation($"Get All Users method call by {loggedinuser} {DateTime.Now} at controller level");
                List<UserViewModel> users = new List<UserViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var loggedinUserRole = HttpContext.Session.GetString("UserRole").ToString();
                    var loggedinuserId = HttpContext.Session.GetString("UserId").ToString();
                    HttpResponseMessage Res = await client.GetAsync(string.Format("User/GetAllusers/"+loggedinUserRole +"/"+loggedinuserId));

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
                            users = root["result"].ToObject<List<UserViewModel>>();
                        }
                    }
                }
                logger.LogInformation($"Get All Users method executed successfully by {loggedinuser} {DateTime.Now} at controller level");
                return View(users);
            }
            catch(Exception ex) {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> DeleteUser(string id)
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
                    HttpResponseMessage Res = await client.DeleteAsync(string.Format("User/DeleteUser/Id=" + id));

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
                            TempData["message"] = languageService.Getkey("User Deleted Successfully");
                            TempData["RedirectURl"] = "/User/GetAllUsers/";
                        }
                    }
                }
                return RedirectToAction("GetAllUsers");
            }
            catch(Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> EditUser(string id)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                UserViewModel user = new UserViewModel();
                List<RoleViewModel> roles = new List<RoleViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync(string.Format("User/GetUserById/Id=" + id));

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var Id = root["auditId"].ToString();
                            TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                            TempData["RedirectURl"] = "/User/GetAllUsers/";
                        }
                        else
                        {
                            user = root["result"].ToObject<UserViewModel>();
                        }
                    }
                    var loggedinUserRole = HttpContext.Session.GetString("UserRole").ToString();
                    HttpResponseMessage Result = await client.GetAsync(string.Format("Role/GetRoleAccordingToRole/" + loggedinUserRole));

                    if (Result.IsSuccessStatusCode)
                    {
                        var content = await Result.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        roles = root["result"].ToObject<List<RoleViewModel>>();
                        var items = roles.Select(x => new SelectListItem
                        {
                            Text = x.Name,
                            Value = x.Id.ToString()
                        }).ToList();
                        user.Roles = items;
                    }
                }
                return View(user);
            }
            catch(Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserViewModel userViewModel)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                UserViewModel user = new UserViewModel();
                List<RoleViewModel> roles = new List<RoleViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Result = await client.GetAsync(string.Format("Role/GetRoles"));

                    if (Result.IsSuccessStatusCode)
                    {
                        var content = await Result.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        roles = root["result"].ToObject<List<RoleViewModel>>();
                        var items = roles.Select(x => new SelectListItem
                        {
                            Text = x.Name,
                            Value = x.Id.ToString()
                        }).ToList();
                        userViewModel.Roles = items;
                    }



                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var json = JsonConvert.SerializeObject(userViewModel);
                    var stringcontenet = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    HttpResponseMessage Res = await client.PutAsync("User/UpdateUser", stringcontenet);

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var Id = root["auditId"].ToString();
                            TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                            //TempData["RedirectURl"] = "/Activity/EditActivity/";
                            return RedirectToAction("EditUser", new { id = userViewModel.Id });

                        }
                        else
                        {
                            user = root["result"].ToObject<UserViewModel>();
                            TempData["message"] = languageService.Getkey("User Updated Successfully");
                            TempData["RedirectURl"] = "/User/GetAllUsers/";
                        }
                    }
                }
                return RedirectToAction("GetAllUsers");
            }
            catch(Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
    }
}
