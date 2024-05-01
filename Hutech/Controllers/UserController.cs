using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using Hutech.Core.Entities;
using Hutech.Models;
using Hutech.Services;
using Irony.Ast;
using iText.Html2pdf;
using iText.IO.Source;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using System.IO;
using iText.Html2pdf;
using iText.IO.Source;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using PageSize = iText.Kernel.Geom.PageSize;
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
                var resultValidation = validation.Validate(userViewModel);
                if (!resultValidation.IsValid)
                {
                    if (userViewModel.UserId > 0)
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
                    JsonResult EmployeeId = await CheckEmployeeIdExist(userViewModel.EmployeeId);
                    bool isEmployeeIdExist = (bool)EmployeeId.Value;
                    if (userViewModel.UserId > 0)
                    {
                        isEmployeeIdExist = false;
                    }
                    if (isEmployeeIdExist)
                    {
                        ViewBag.ErrorMessageForEmployeeId = languageService.Getkey("Employee Id already Exist");
                        return View(userViewModel);
                    }
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
                            if (string.IsNullOrEmpty(userViewModel.Email))
                                userViewModel.Email = userViewModel.EmployeeId;
                            await userStore.SetUserNameAsync(user, userViewModel.Email, CancellationToken.None);
                            await emailStore.SetEmailAsync(user, userViewModel.Email, CancellationToken.None);
                            user.EmailConfirmed = true;
                            user.UserName = !string.IsNullOrEmpty(user.UserName) ? user.UserName : userViewModel.EmployeeId;
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
                                string message= languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                                TempData["message"] = message;
                                TempData["RedirectURl"] = "/USer/AddUser/";
                            }
                            else
                            {
                                if (isEdit == true)
                                {
                                    string message= languageService.Getkey("User Updated Successfully");
                                    TempData["message"] = message;

                                }
                                else
                                {
                                    string message= languageService.Getkey("User Added Successfully");
                                    TempData["message"] = message;


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
        [HttpPost]
        public async Task<IActionResult> GetAllUsers([FromBody] UserModal userModal)
        {
            userModal.PageNumber = 1;
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
                List<UserViewModel> users = new List<UserViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    userModal.LoggedInUserId = HttpContext.Session.GetString("UserId").ToString();
                    var json = JsonConvert.SerializeObject(userModal);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    HttpResponseMessage Res = await client.PostAsync("User/GetAllFilterUser", stringContent);

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
                            users = root["result"].ToObject<List<UserViewModel>>();
                            pageNumber = (int)root["currentPage"];
                            totalRecords = (int)root["totalRecords"];
                            totalPage = (int)root["totalPage"];
                        }
                    }
                }
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
                var data = new UsersViewModel()
                {
                    CurrentPage = pageNumber,
                    userViewModels = users,
                    TotalPages = totalPage,
                    TotalRecords = totalRecords
                };

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
                    Value = System.Convert.ToString(x.Id)
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

                var userTypedata = userTypes.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();
                data.UserTypes = userTypedata;
                data.Roles = roleData;
                data.Locations = locationData;
                data.Departments = departmentData;
                data.FullName = !string.IsNullOrEmpty(userModal.FullName) ? userModal.FullName : "";
                data.EmailId = !string.IsNullOrEmpty(userModal.EmailId) ? userModal.EmailId : "";
                data.UserName = !string.IsNullOrEmpty(userModal.UserName) ? userModal.UserName : "";
                data.EmployeeId = !string.IsNullOrEmpty(userModal.EmployeeId) ? userModal.EmployeeId : "";
                //data.Status = locationModel.status;
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
                data.SelectedStatus = System.Convert.ToInt32(userModal.status);
                data.SelectedUserType = System.Convert.ToInt32(userModal.UserType);
                data.SelectedDepartment =System.Convert.ToInt64(userModal.Department);
                data.SelectedLocation = System.Convert.ToInt64(userModal.Location);
                data.SelectedRole = userModal.Role;
                string requestedWithHeader = Request.Headers["X-Requested-With"];
                return View("GetAllUsers", data);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<ActionResult> ExportToExcel(string? FullName = null, string? UserName = null, string? EmailId = null, string? Status = null, string? EmployeeId = null, string? userType = null, string? department = null, string? role = null, string? location = null)
        {
            var csvBuilder = new StringBuilder();
            try
            {
                int PageNumber = 0;
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                int totalRecords = 0;
                int totalPage = 0;
                var loggedinuser = HttpContext.Session.GetString("LoogedInUser");
                logger.LogInformation($"Get All Users method call by {loggedinuser} {DateTime.Now} at controller level");
                UsersViewModel usersViewModel = new UsersViewModel();
                List<UserViewModel> users = new List<UserViewModel>();
                string apiUrl = configuration["Baseurl"];
                UserModal userModal = new UserModal();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    userModal.PageNumber = PageNumber;
                    userModal.FullName = FullName;
                    userModal.EmailId = EmailId;
                    userModal.UserName = UserName;
                    userModal.status = Status;
                    userModal.EmployeeId = EmployeeId;
                    userModal.UserType = userType;
                    userModal.Department = department;
                    userModal.Role = role;
                    userModal.Location = location;
                    var loggedinUserRole = HttpContext.Session.GetString("UserRole").ToString();
                    userModal.LoggedInUserId = HttpContext.Session.GetString("UserId").ToString();
                    var json = JsonConvert.SerializeObject(userModal);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                    HttpResponseMessage Res = await client.PostAsync("User/GetAllFilterUser", stringContent);
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
                            users = root["result"].ToObject<List<UserViewModel>>();
                            PageNumber = (int)root["currentPage"];
                            totalRecords = (int)root["totalRecords"];
                            totalPage = (int)root["totalPage"];
                        }
                    }
                }
                if (users.Count != 0)
                {
                    var firstOrDefaultRec = users.FirstOrDefault();
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // or LicenseContext.Commercial

                    if (firstOrDefaultRec != null)
                    {
                        // Create a new Excel package
                        using (ExcelPackage package = new ExcelPackage())
                        {
                            // Add a new worksheet to the Excel package
                            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Users");

                            // Define headers for your Excel columns
                            worksheet.Cells[1, 1].Value = "FullName";
                            worksheet.Cells[1, 2].Value = "UserType";
                            worksheet.Cells[1, 3].Value = "UserName";
                            worksheet.Cells[1, 4].Value = "Email";
                            worksheet.Cells[1, 5].Value = "Employee Id";
                            worksheet.Cells[1, 6].Value = "Remark";
                            worksheet.Cells[1, 7].Value = "Department Name";
                            worksheet.Cells[1, 8].Value = "Role";
                            worksheet.Cells[1, 9].Value = "Location";

                            int row = 2; // Start from the second row

                            // Iterate over each user and populate data into the worksheet
                            foreach (var user in users)
                            {
                                // Populate data for the current user
                                worksheet.Cells[row, 1].Value = user.FullName;
                                worksheet.Cells[row, 2].Value = user.UserType;
                                worksheet.Cells[row, 3].Value = user.WindowsUserName;
                                worksheet.Cells[row, 4].Value = user.Email;
                                worksheet.Cells[row, 5].Value = user.EmployeeId;
                                worksheet.Cells[row, 6].Value = user.Remark;
                                worksheet.Cells[row, 7].Value = user.DepartmentName;
                                worksheet.Cells[row, 8].Value = user.RoleName;
                                worksheet.Cells[row, 9].Value = user.LocationName;
                                // Add more data properties as needed

                                // Increment the row index for the next user
                                row++;
                            }

                            // Save the Excel package to a memorystream
                            MemoryStream stream = new MemoryStream();
                            package.SaveAs(stream);

                            // Set the position of the stream to 0 to start reading from the beginning
                            stream.Position = 0;

                            // Return the memorystream
                            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "users.xlsx");
                        }
                    }
                    else
                    {
                        // Handle the case where the collection is empty
                        // For example, return an empty Excel file or display an error message
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
            return File(System.Text.Encoding.ASCII.GetBytes(csvBuilder.ToString()), "text/csv", "User.csv");
        }
        public async Task<ActionResult> ExportToPDF(string? FullName = null, string? UserName = null, string? EmailId = null, string? Status = null, string? EmployeeId = null, string? userType = null, string? department = null, string? role = null, string? location = null)
        {
            var csvBuilder = new StringBuilder();
            try
            {
                int PageNumber = 0;
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                int totalRecords = 0;
                int totalPage = 0;
                var loggedinuser = HttpContext.Session.GetString("LoogedInUser");
                logger.LogInformation($"Get All Users method call by {loggedinuser} {DateTime.Now} at controller level");
                UsersViewModel usersViewModel = new UsersViewModel();
                List<UserViewModel> users = new List<UserViewModel>();
                string apiUrl = configuration["Baseurl"];
                UserModal userModal = new UserModal();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    userModal.PageNumber = PageNumber;
                    userModal.FullName = FullName;
                    userModal.EmailId = EmailId;
                    userModal.UserName = UserName;
                    userModal.status = Status;
                    userModal.EmployeeId = EmployeeId;
                    userModal.UserType = userType;
                    userModal.Department = department;
                    userModal.Role = role;
                    userModal.Location = location;
                    var loggedinUserRole = HttpContext.Session.GetString("UserRole").ToString();
                    userModal.LoggedInUserId = HttpContext.Session.GetString("UserId").ToString();
                    var json = JsonConvert.SerializeObject(userModal);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                    HttpResponseMessage Res = await client.PostAsync("User/GetAllFilterUser", stringContent);
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
                            users = root["result"].ToObject<List<UserViewModel>>();
                        }
                    }
                    //Building an HTML string.
                    StringBuilder sb = new StringBuilder();

                    //Table start.
                    sb.Append("<table border='1' cellpadding='5' cellspacing='0' style='border: 1px solid #ccc;font-family: Arial;'>");

                    //Building the Header row.
                    sb.Append("<tr>");
                    sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>User Type</th>");
                    sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Full Name</th>");
                    sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>User Name</th>");
                    sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Email</th>");
                    sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>EmployeeId</th>");
                    sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Remark</th>");
                    sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Department</th>");
                    sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>UserRole</th>");
                    sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Location</th>");
                    sb.Append("</tr>");
                    //Building the Data rows.
                    for (int i = 0; i < users.Count; i++)
                    {
                        UserViewModel model = new UserViewModel();
                        model = users[i];
                        string[] userData = new string[9];
                        userData[0] = model.UserType.ToString();
                        userData[1] = model.FullName;
                        userData[2] = model.WindowsUserName;
                        userData[3] = model.Email;
                        userData[4] = model.EmployeeId.ToString();
                        userData[5] = model.Remark;
                        userData[6] = model.DepartmentName;
                        userData[7] = model.RoleName;
                        userData[8] = model.LocationName;
                        for (int j = 0; j < userData.Length; j++)
                        {
                            //Append data.
                            sb.Append("<td style='border: 1px solid #ccc'>");
                            sb.Append(userData[j]);
                            sb.Append("</td>");
                        }
                        sb.Append("</tr>");
                    }

                    //Table end.
                    sb.Append("</table>");
                    using (MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(sb.ToString())))
                    {
                        ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
                        PdfWriter writer = new PdfWriter(byteArrayOutputStream);
                        PdfDocument pdfDocument = new PdfDocument(writer);
                        pdfDocument.SetDefaultPageSize(PageSize.A1);
                        HtmlConverter.ConvertToPdf(stream, pdfDocument);
                        pdfDocument.Close();
                        return File(byteArrayOutputStream.ToArray(), "application/pdf", "User.pdf");
                    }
                }
            }
            catch(Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> GetAllUsers(int PageNumber = 1, string? FullName = null, string? UserName = null, string? EmailId = null, string? Status = null, string? EmployeeId = null,string? userType=null,string? department=null,string? role=null,string? location=null)
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
                logger.LogInformation($"Get All Users method call by {loggedinuser} {DateTime.Now} at controller level");
                UsersViewModel usersViewModel = new UsersViewModel();
                List<UserViewModel> users = new List<UserViewModel>();
                string apiUrl = configuration["Baseurl"];
                UserModal userModal = new UserModal();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    userModal.PageNumber = PageNumber;
                    userModal.FullName = FullName;
                    userModal.EmailId = EmailId;
                    userModal.UserName = UserName;
                    userModal.status = Status;
                    userModal.EmployeeId = EmployeeId;
                    userModal.UserType = userType;
                    userModal.Department = department;
                    userModal.Role = role;
                    userModal.Location = location;
                    var loggedinUserRole = HttpContext.Session.GetString("UserRole").ToString();
                    userModal.LoggedInUserId = HttpContext.Session.GetString("UserId").ToString();
                    var json = JsonConvert.SerializeObject(userModal);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                    //HttpResponseMessage Res = await client.GetAsync(string.Format("User/GetAllusers/" + loggedinUserRole + "/" + loggedinuserId));
                    HttpResponseMessage Res = await client.PostAsync("User/GetAllFilterUser", stringContent);
                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var Id = root["auditId"].ToString();
                            string message = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                            TempData["message"] = message;
                        }
                        else
                        {
                            users = root["result"].ToObject<List<UserViewModel>>();
                            PageNumber = (int)root["currentPage"];
                            totalRecords = (int)root["totalRecords"];
                            totalPage = (int)root["totalPage"];
                        }
                    }
                }
                logger.LogInformation($"Get All Users method executed successfully by {loggedinuser} {DateTime.Now} at controller level");
                var data = new UsersViewModel()
                {
                    CurrentPage = PageNumber,
                    userViewModels = users,
                    TotalPages = totalPage,
                    TotalRecords = totalRecords
                };
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
                    Value = System.Convert.ToString(x.Id)
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
                    Value = System.Convert.ToString(x.Id)
                }).ToList();

                var items = new List<SelectListItem>();
                foreach (int value in Enum.GetValues(typeof(StatusViewModel)))
                {
                    items.Add(new SelectListItem
                    {
                        Text = Enum.GetName(typeof(StatusViewModel), value),
                        Value = value.ToString()
                    });
                }
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

                var userTypedata = userTypes.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();
                data.UserTypes = userTypedata;
                data.Roles = roleData;
                data.Departments = departmentData;
                data.Locations = locationData;
                data.Status = items;
                if (Status == null)
                    data.SelectedStatus = (int)StatusViewModel.Active;
                else if (!string.IsNullOrEmpty(Status))
                    data.SelectedStatus = System.Convert.ToInt32(Status);
                if (!string.IsNullOrEmpty(userModal.UserType))
                    data.SelectedUserType = System.Convert.ToInt32(userModal.UserType);
                if (!string.IsNullOrEmpty(userModal.Department))
                    data.SelectedDepartment = System.Convert.ToInt32(userModal.Department);
                if(!string.IsNullOrEmpty(userModal.Location))
                    data.SelectedLocation = System.Convert.ToInt64(userModal.Location);
                if(!string.IsNullOrEmpty(userModal.Role))
                    data.SelectedRole = userModal.Role;
                data.FullName = !string.IsNullOrEmpty(userModal.FullName) ? userModal.FullName : "";
                data.UserName = !string.IsNullOrEmpty(userModal.UserName) ? userModal.UserName : "";
                data.EmailId = !string.IsNullOrEmpty(userModal.EmailId) ? userModal.EmailId : "";
                data.EmployeeId = !string.IsNullOrEmpty(userModal.EmployeeId) ? userModal.EmployeeId : "";
                return View(data);
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
                            string message= languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + auditId;
                            TempData["message"] = message;
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
                                string message= languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + auditId;
                                TempData["message"] = message;
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
                            string message= languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + auditId;
                            TempData["message"] = message;
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
                            string message= languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + id;
                            TempData["message"] = message;

                        }
                        else
                        {
                            string message= languageService.Getkey("User Deleted Successfully");
                            TempData["message"] = message;
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
                            string message= languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                            TempData["message"] = message;
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
        public async Task<JsonResult> CheckEmployeeIdExist(string EmployeeId)
        {
            using (var client = new HttpClient())
            {
                string apiUrl = configuration["Baseurl"];
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //check whether employeeId exist or not
                bool isExist = false;
                HttpResponseMessage Result = await client.GetAsync(string.Format("User/CheckEmployeeIdExist/" + EmployeeId));
                if (Result.IsSuccessStatusCode)
                {
                    var content = await Result.Content.ReadAsStringAsync();
                    JObject root = JObject.Parse(content);
                    var result = root["result"];
                    isExist = (bool)result;
                }
                return Json(isExist);
            }
        }
    }
}
