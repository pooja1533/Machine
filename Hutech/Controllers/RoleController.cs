using Azure;
using DocumentFormat.OpenXml.Wordprocessing;
using Hutech.Models;
using Hutech.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using System.Configuration;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Hutech.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly IConfiguration configuration;
        private string BaseUrl = string.Empty;
        private readonly ILogger<RoleController> logger;
        private readonly LanguageService languageService;
        public RoleController(IConfiguration _configuration, ILogger<RoleController> logger, LanguageService _languageService)
        {
            configuration = _configuration;
            BaseUrl = configuration["Baseurl"];
            this.logger = logger;
            languageService = _languageService;
        }
        public async Task<IActionResult> EditRole(Guid id)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                RoleViewModel roleViewModel = new RoleViewModel();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.GetAsync(string.Format("Role/GetRoleDetail/{0}", id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var Id = root["auditId"].ToString();
                            TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                            TempData["RedirectURl"] = "/Role/AddRole/";
                        }
                        else
                        {
                            roleViewModel = root["result"].ToObject<RoleViewModel>();
                        }
                    }
                }
                return View(roleViewModel);
            }
            catch(Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> EditRole(RoleViewModel roleViewModel)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                var validateRole = new RoleValidator();
                var validateResult = validateRole.Validate(roleViewModel);
                if (!validateResult.IsValid)
                {
                    return View();
                }
                else
                {
                    string apiUrl = configuration["Baseurl"];
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(BaseUrl);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        roleViewModel.DateModifiedUtc = DateTime.UtcNow;
                        roleViewModel.ModifiedByUserId = HttpContext.Session.GetString("UserId");
                        var json = JsonConvert.SerializeObject(roleViewModel);
                        var stringcontenet = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpResponseMessage response = await client.PutAsync("Role/PutRole", stringcontenet);

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            var resultData = root["success"].ToString();

                            if (resultData == "False" || resultData == "false")
                            {
                                var Id = root["auditId"].ToString();
                                TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                                TempData["RedirectURl"] = "/Role/EditRole/";
                                return RedirectToAction("EditRole", new { id = roleViewModel.Id });

                            }
                            else
                            {
                                roleViewModel = root["result"].ToObject<RoleViewModel>();
                                TempData["message"] = languageService.Getkey("Role Updated Successfully");
                                TempData["RedirectURl"] = "/Role/GetAllRoles/";
                            }

                        }
                    }
                    return RedirectToAction("GetAllRoles");
                }
            }
            catch(Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }

        }
        public async Task<IActionResult> DeleteRole(Guid id)
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
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.DeleteAsync(string.Format("Role/DeleteRole/{0}", id));

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
                            TempData["message"] = languageService.Getkey("Role Deleted Successfully");
                            TempData["RedirectURl"] = "/Role/GetAllRoles/";
                        }
                    }
                }
                return RedirectToAction("GetAllRoles");
            }
            catch(Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetAllRoles([FromBody] RoleModel roleModel)
        {
            roleModel.pageNumber = 1;
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
                List<RoleViewModel> roles = new List<RoleViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var json = JsonConvert.SerializeObject(roleModel);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    HttpResponseMessage Res = await client.PostAsync("Role/GetAllFilterRoles", stringContent);

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
                            roles = root["result"].ToObject<List<RoleViewModel>>();
                            pageNumber = (int)root["currentPage"];
                            totalRecords = (int)root["totalRecords"];
                            totalPage = (int)root["totalPage"];
                        }
                    }
                }
                var data = new RolesViewModel()
                {
                    CurrentPage = pageNumber,
                    Roles = roles,
                    TotalPages = totalPage,
                    TotalRecords = totalRecords
                };
                data.RoleName = !string.IsNullOrEmpty(roleModel.roleName) ? roleModel.roleName : "";
                data.UpdatedBy = !string.IsNullOrEmpty(roleModel.updatedBy) ? roleModel.updatedBy : "";
                //data.Status = locationModel.status;
                data.UpdatedDate = roleModel.updatedDate;
                string requestedWithHeader = Request.Headers["X-Requested-With"];
                return View("GetAllRoles", data);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> GetAllRoles(int pageNumber = 1,string? roleName = null, string? updatedBy = null, string? updatedDate = null)
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
                var loggedinuser = HttpContext.Session.GetString("LoogedInUser");
                RoleModel roleModel = new RoleModel();
                List<RoleViewModel> roles = new List<RoleViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    roleModel.pageNumber = pageNumber;
                    roleModel.roleName = roleName;
                    roleModel.updatedBy = updatedBy;
                    if (!string.IsNullOrEmpty(updatedDate))
                        roleModel.updatedDate = DateTime.Parse(updatedDate);
                    var json = JsonConvert.SerializeObject(roleModel);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                    HttpResponseMessage Res = await client.PostAsync("Role/GetAllFilterRoles", stringContent);

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
                            roles = root["result"].ToObject<List<RoleViewModel>>();
                            pageNumber = (int)root["currentPage"];
                            totalRecords = (int)root["totalRecords"];
                            totalPage = (int)root["totalPage"];
                        }
                    }
                }
                var data = new RolesViewModel()
                {
                    CurrentPage = pageNumber,
                    Roles = roles,
                    TotalPages = totalPage,
                    TotalRecords = totalRecords
                };
                data.RoleName = !string.IsNullOrEmpty(roleModel.roleName) ? roleModel.roleName : "";
                data.UpdatedBy = !string.IsNullOrEmpty(roleModel.updatedBy) ? roleModel.updatedBy : "";
                data.UpdatedDate = roleModel.updatedDate;
                return View(data);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public IActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoleAsync(RoleViewModel roleViewModel)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                var roleValidator = new RoleValidator();
                var validatorResult = roleValidator.Validate(roleViewModel);
                if (!validatorResult.IsValid)
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
                        roleViewModel.CreatedByUserId = HttpContext.Session.GetString("UserId");
                        roleViewModel.DatecreatedUtc = DateTime.UtcNow;
                        roleViewModel.DateModifiedUtc=DateTime.UtcNow;
                        var json = JsonConvert.SerializeObject(roleViewModel);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                        HttpResponseMessage Res = await client.PostAsync("Role/PostRole", stringContent);

                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            var resultData = root["success"].ToString();
                            if (resultData == "False" || resultData == "false")
                            {
                                var Id = root["auditId"].ToString();
                                TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                                TempData["RedirectURl"] = "/Role/AddRole/";
                            }
                            else
                            {
                                TempData["message"] = languageService.Getkey("Role Added Successfully");
                                TempData["RedirectURl"] = "/Role/GetAllRoles/";
                            }
                        }
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetMenuAceessRightForRole(string selectedroleId)
        {
            var token = Request.Cookies["jwtCookie"];
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();

                token = token.Replace("Bearer ", "");
            }
            List<MenuViewModel> allmenus = new List<MenuViewModel>();
            string apiUrl = configuration["Baseurl"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage Res = await client.GetAsync(string.Format("Role/GetMenuAceessRightForRole/{0}", selectedroleId));
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
                        allmenus = root["result"].ToObject<List<MenuViewModel>>();
                        var mastermenu = allmenus.Where(x => x.ParentId == 0);
                        var submenu = allmenus.Where(x => x.ParentId > 0);
                        foreach (var menu in mastermenu)
                        {
                            var list = submenu.Where(x => x.ParentId == menu.Id);
                            bool isuserHaveAccessAllSubMenu = list.All(x => x.IsUserHaveAccess == true);
                            menu.IsUserHaveAccess = isuserHaveAccessAllSubMenu == true ? true : false;
                        }
                    }
                }
            }
            return Json(allmenus);
        }
        [HttpPost]
        public async Task<IActionResult> SaveMenuAccessOfRole(string selectedMenuIds, string roleId)
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
                    UserMenuPermissionViewModel userMenuPermissionViewModel = new UserMenuPermissionViewModel();
                    userMenuPermissionViewModel.MenuIds = selectedMenuIds;
                    userMenuPermissionViewModel.RoleId=roleId;
                    userMenuPermissionViewModel.DatecreatedUtc = DateTime.UtcNow;
                    userMenuPermissionViewModel.CreatedByUserId= HttpContext.Session.GetString("UserId");
                    // Serialize the data to JSON string
                    string jsonData = JsonConvert.SerializeObject(userMenuPermissionViewModel);

                    // Create StringContent with JSON data
                    var data = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // Send HTTP POST request to the API endpoint
                    HttpResponseMessage Res = await client.PostAsync("Role/SaveMenuAccessOfRole", data);
                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        var response = new
                        {
                            Message = "Menu access saved successfully",
                            StatusCode = 200
                        };
                        return Json(response);
                    }
                }
                var responseData = new
                {
                    Message = "Something Went wrong please contact your administrator",
                    StatusCode = 500
                };
                return Json(responseData);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
    }
}
