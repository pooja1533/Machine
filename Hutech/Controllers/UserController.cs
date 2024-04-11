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
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager;
        private readonly Microsoft.AspNetCore.Identity.IUserStore<ApplicationUser> userStore;
        private readonly Microsoft.AspNetCore.Identity.IUserEmailStore<ApplicationUser> emailStore;
        public UserController(IConfiguration _configuration, ILogger<UserController> _logger, LanguageService _languageService, Microsoft.AspNetCore.Identity.IUserStore<ApplicationUser> _userStore, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager)
        {
            configuration = _configuration;
            logger = _logger;
            languageService = _languageService;
            userStore = _userStore;
            userManager = _userManager;
            emailStore = GetEmailStore();
        }
        private Microsoft.AspNetCore.Identity.IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            try
            {
                if (!userManager.SupportsUserEmail)
                {
                    throw new NotSupportedException("The default UI requires a user store with email support.");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("SQL Exception:", ex);
            }
            return (Microsoft.AspNetCore.Identity.IUserEmailStore<ApplicationUser>)userStore;
        }
        public async Task<IActionResult> AddUser()
        {
            UserViewModel userViewModel = new UserViewModel();
            var token = Request.Cookies["jwtCookie"];
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();

                token = token.Replace("Bearer ", "");
            }
            string apiUrl = configuration["Baseurl"];
            List<UserTypeViewModel> userTypes = new List<UserTypeViewModel>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("UserType/GetActiveUserType");

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        userTypes = root["result"].ToObject<List<UserTypeViewModel>>();
                    }
                }
                var data = userTypes.Select(x => new SelectListItem
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
                var locationData = locations.Select(x => new SelectListItem
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
                var departmentData = departments.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();
                List<RoleViewModel> roles = new List<RoleViewModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage Res = await client.GetAsync("Role/GetAllRoles");
                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        roles = root["result"].ToObject<List<RoleViewModel>>();
                    }
                }
                var roleData = roles.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();
                userViewModel.UserRolesId = roleData;
                userViewModel.DepartmentsId = departmentData;
                userViewModel.UserTypesId = data;
                userViewModel.LocationsId = locationData;
                userViewModel.WindowsUserNamesId = new List<SelectListItem>
                {
                    new SelectListItem{Text="User1",Value="1"},
                    new SelectListItem{Text="User2",Value="2"},
                    new SelectListItem{Text="user3",Value="3"},
                };
                return View(userViewModel);

            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(UserViewModel userViewModel)
        {
            bool isEdit = false;
            try
            {
                string apiUrl = configuration["Baseurl"];
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                var validation = new UserValidator();
                var resultValidation = validation.Validate(userViewModel);
                if (!resultValidation.IsValid)
                {
                    foreach(var failuremsg in resultValidation.Errors)
                    {
                       
                    }
                    List<UserTypeViewModel> userTypes = new List<UserTypeViewModel>();

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                        HttpResponseMessage Res = await client.GetAsync("UserType/GetActiveUserType");

                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            userTypes = root["result"].ToObject<List<UserTypeViewModel>>();
                        }
                    }
                    var data = userTypes.Select(x => new SelectListItem
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
                    var locationData = locations.Select(x => new SelectListItem
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
                    var departmentData = departments.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }).ToList();
                    List<RoleViewModel> roles = new List<RoleViewModel>();
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                        HttpResponseMessage Res = await client.GetAsync("Role/GetAllRoles");
                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            roles = root["result"].ToObject<List<RoleViewModel>>();
                        }
                    }
                    var roleData = roles.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }).ToList();
                    userViewModel.UserRolesId = roleData;
                    userViewModel.DepartmentsId = departmentData;
                    userViewModel.UserTypesId = data;
                    userViewModel.LocationsId = locationData;
                    userViewModel.WindowsUserNamesId = new List<SelectListItem>
                {
                    new SelectListItem{Text="User1",Value="1"},
                    new SelectListItem{Text="User2",Value="2"},
                    new SelectListItem{Text="user3",Value="3"},
                };
                   if(userViewModel.UserId>0)
                    {
                        return View("EditUser", userViewModel);
                    }
                    else
                    {
                        return View(userViewModel);


                    }
                }
                else
                {

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        if (userViewModel.UserId > 0)
                        {
                            var user = await userManager.FindByIdAsync(userViewModel.AspNetUserId);
                            user.UserName = userViewModel.Email;
                            user.Email = userViewModel.Email;
                            user.NormalizedEmail = userViewModel.Email.ToUpper();
                            user.NormalizedUserName = userViewModel.Email.ToUpper();
                            var result = await userManager.UpdateAsync(user);
                            var userId = await userManager.GetUserIdAsync(user);
                            userViewModel.AspNetUserId = userId;
                            isEdit = true;
                        }
                        else
                        {
                            var user = CreateUser();
                            await userStore.SetUserNameAsync(user, userViewModel.Email, CancellationToken.None);
                            await emailStore.SetEmailAsync(user, userViewModel.Email, CancellationToken.None);
                            user.EmailConfirmed = true;
                            var result = await userManager.CreateAsync(user, "Hutech@123");
                            var userId = await userManager.GetUserIdAsync(user);
                            userViewModel.AspNetUserId = userId;
                        }


                        var json = JsonConvert.SerializeObject(userViewModel);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                        HttpResponseMessage Res = await client.PostAsync("User/PostUser", stringContent);
                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            var resultData = root["success"].ToString();
                            if (resultData == "False" || resultData == "false")
                            {
                                var Id = root["auditId"].ToString();
                                TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                                TempData["RedirectURl"] = "/USer/AddUser/";
                            }
                            else
                            {
                                if (isEdit == true)
                                {
                                    TempData["message"] = languageService.Getkey("User Updated Successfully");

                                }
                                else
                                {
                                    TempData["message"] = languageService.Getkey("User Added Successfully");


                                }
                                TempData["RedirectURl"] = "/User/GetAllUsers/";
                            }
                        }
                    }
                    //return RedirectToAction("GetAllLocation");
                    return RedirectToAction("GetAllUsers");
                }
            }
            catch (Exception ex)
            {

            }
            return View(userViewModel);
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
                    HttpResponseMessage Res = await client.GetAsync(string.Format("User/GetAllusers/" + loggedinUserRole + "/" + loggedinuserId));

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
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<JsonResult> RejectUser(string comment, long userId)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                bool isValidPassword = false;

                UserViewModel model = new UserViewModel();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.GetAsync(string.Format("User/RejectUser/{0}/{1}", comment, userId));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var auditId = root["auditId"].ToString();
                            TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + auditId;
                            TempData["RedirectURl"] = "/User/AddUser/";
                        }
                        else
                        {
                            isValidPassword = true;
                        }
                    }
                    return Json(model);
                }

            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<JsonResult> ApproveUser(string Password, long UserId)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                bool isValidPassword = false;
                var loggedInUserEmail = HttpContext.Session.GetString("LoogedInUser").ToString();
                var user = await userManager.FindByNameAsync(loggedInUserEmail);
                bool isPasswordvalidofLogginUser = await userManager.CheckPasswordAsync(user, Password);
                if (isPasswordvalidofLogginUser == false)
                {
                    return Json(new { isValidPassword });
                }
                else
                {
                    UserViewModel model = new UserViewModel();
                    string apiUrl = configuration["Baseurl"];
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpResponseMessage response = await client.GetAsync(string.Format("User/ApproveUser/{0}", UserId));

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            var resultData = root["success"].ToString();
                            if (resultData == "False" || resultData == "false")
                            {
                                var auditId = root["auditId"].ToString();
                                TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + auditId;
                                TempData["RedirectURl"] = "/User/AddUser/";
                            }
                            else
                            {
                                isValidPassword = true;
                            }
                        }
                    }
                    return Json(model);
                }

            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<JsonResult> GetUserDetail(long Id)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                UserViewModel model = new UserViewModel();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.GetAsync(string.Format("User/GetUserDetail/{0}", Id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var auditId = root["auditId"].ToString();
                            TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + auditId;
                            TempData["RedirectURl"] = "/User/AddUser/";
                        }
                        else
                        {
                            model = root["result"].ToObject<UserViewModel>();
                        }
                    }
                }
                return Json(model);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> DeleteUser(long Id)
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
                    HttpResponseMessage response = await client.DeleteAsync(string.Format("User/DeleteUser/{0}", Id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var id = root["auditId"].ToString();
                            TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + id;

                        }
                        else
                        {
                            TempData["message"] = languageService.Getkey("User Deleted Successfully");
                            TempData["RedirectURl"] = "/User/GetAllUsers/";
                        }
                        //var message = root["value"].ToString();
                    }
                }
                return RedirectToAction("GetAllUsers");
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;

            }
        }
        public async Task<IActionResult> EditUser(long id)
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
                List<UserTypeViewModel> userTypes = new List<UserTypeViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage UserTypes = await client.GetAsync("UserType/GetActiveUserType");

                    if (UserTypes.IsSuccessStatusCode)
                    {
                        var content = await UserTypes.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        userTypes = root["result"].ToObject<List<UserTypeViewModel>>();
                    }

                    var data = userTypes.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }).ToList();
                    user.UserTypesId = data;

                    List<LocationViewModel> locations = new List<LocationViewModel>();
                    HttpResponseMessage LocationRes = await client.GetAsync("Location/GetActiveLocation");
                    if (LocationRes.IsSuccessStatusCode)
                    {
                        var content = await LocationRes.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        locations = root["result"].ToObject<List<LocationViewModel>>();
                    }

                    var locationData = locations.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }).ToList();

                    HttpResponseMessage Res = await client.GetAsync(string.Format("User/GetUserById/{0}", id));

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
                            user.UserTypesId = data;
                            user.LocationsId = locationData;
                        }
                    }
                    List<DepartmentViewModel> departments = new List<DepartmentViewModel>();
                    HttpResponseMessage DepartmentRes = await client.GetAsync("Department/GetActiveDepartment");
                    if (DepartmentRes.IsSuccessStatusCode)
                    {
                        var content = await DepartmentRes.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        departments = root["result"].ToObject<List<DepartmentViewModel>>();
                    }

                    var departmentData = departments.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }).ToList();
                    user.DepartmentsId = departmentData;

                    List<RoleViewModel> roles = new List<RoleViewModel>();
                    HttpResponseMessage RolesRes = await client.GetAsync("Role/GetAllRoles");
                    if (RolesRes.IsSuccessStatusCode)
                    {
                        var content = await RolesRes.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        roles = root["result"].ToObject<List<RoleViewModel>>();
                    }

                    var roleData = roles.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }).ToList();
                    user.UserRolesId = roleData;
                    user.SelectedUserRoleId = !string.IsNullOrEmpty(user.RoleName) ? user.RoleName?.Split(",")?.Select(x => x)?.ToList() : new List<string>(new string[] { });

                    user.WindowsUserNamesId = new List<SelectListItem>
                    {
                    new SelectListItem{Text="User1",Value="1"},
                    new SelectListItem{Text="User2",Value="2"},
                    new SelectListItem{Text="user3",Value="3"},
                    };
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
                    }
                }
                return View(user);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        //[HttpPost]
        //public async Task<IActionResult> UpdateUser(UserViewModel userViewModel)
        //{
        //    try
        //    {
        //        var token = Request.Cookies["jwtCookie"];
        //        if (!string.IsNullOrEmpty(token))
        //        {
        //            var handler = new JwtSecurityTokenHandler();

        //            token = token.Replace("Bearer ", "");
        //        }
        //        UserViewModel user = new UserViewModel();
        //        List<RoleViewModel> roles = new List<RoleViewModel>();
        //        string apiUrl = configuration["Baseurl"];
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(apiUrl);
        //            client.DefaultRequestHeaders.Clear();

        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //            HttpResponseMessage Result = await client.GetAsync(string.Format("Role/GetRoles"));

        //            if (Result.IsSuccessStatusCode)
        //            {
        //                var content = await Result.Content.ReadAsStringAsync();
        //                JObject root = JObject.Parse(content);
        //                roles = root["result"].ToObject<List<RoleViewModel>>();
        //                var items = roles.Select(x => new SelectListItem
        //                {
        //                    Text = x.Name,
        //                    Value = x.Id.ToString()
        //                }).ToList();
        //                userViewModel.Roles = items;
        //            }



        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //            var json = JsonConvert.SerializeObject(userViewModel);
        //            var stringcontenet = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

        //            HttpResponseMessage Res = await client.PutAsync("User/UpdateUser", stringcontenet);

        //            if (Res.IsSuccessStatusCode)
        //            {
        //                var content = await Res.Content.ReadAsStringAsync();
        //                JObject root = JObject.Parse(content);
        //                var resultData = root["success"].ToString();
        //                if (resultData == "False" || resultData == "false")
        //                {
        //                    var Id = root["auditId"].ToString();
        //                    TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
        //                    TempData["RedirectURl"] = "/Activity/EditActivity/";
        //                    return RedirectToAction("EditUser", new { id = userViewModel.Id });

        //                }
        //                else
        //                {
        //                    user = root["result"].ToObject<UserViewModel>();
        //                    TempData["message"] = languageService.Getkey("User Updated Successfully");
        //                    TempData["RedirectURl"] = "/User/GetAllUsers/";
        //                }
        //            }
        //        }
        //        return RedirectToAction("GetAllUsers");
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogInformation($"SQL Exception Occure.{ex.Message}");
        //        throw ex;
        //    }
        //}

    }
}
