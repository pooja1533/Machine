using Hutech.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Hutech.Core.Constants;
using Hutech.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using Hutech.Services;
using Microsoft.AspNetCore.Mvc.Rendering;   

namespace Hutech.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        public IConfiguration configuration { get; set; }
        private readonly ILogger<DepartmentController> logger;
        private readonly LanguageService languageService;
        public DepartmentController(IConfiguration _configuration,ILogger<DepartmentController> _logger, LanguageService _languageService)
        {
            configuration = _configuration;
            logger = _logger;
            languageService = _languageService;
        }
        [HttpPost]
        public async Task<IActionResult> GetAllDepartment([FromBody] DepartmentModel departmentModel)
        {
            departmentModel.pageNumber = 1;
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
                List<DepartmentViewModel> departments = new List<DepartmentViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var json = JsonConvert.SerializeObject(departmentModel);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    HttpResponseMessage Res = await client.PostAsync("Department/GetAllFilterDepartment", stringContent);

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
                            departments = root["result"].ToObject<List<DepartmentViewModel>>();
                            pageNumber = (int)root["currentPage"];
                            totalRecords = (int)root["totalRecords"];
                            totalPage = (int)root["totalPage"];
                        }
                    }
                }
                var data = new DepartmentsViewModel()
                {
                    CurrentPage = pageNumber,
                    DepartmentViewModels = departments,
                    TotalPages = totalPage,
                    TotalRecords = totalRecords
                };
                data.DepartmentName = !string.IsNullOrEmpty(departmentModel.departmentName)?departmentModel.departmentName:"";
                data.UpdatedBy = !string.IsNullOrEmpty(departmentModel.updatedBy)? departmentModel.updatedBy:"";
                data.UpdatedDate = departmentModel.updatedDate;
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
                data.SelectedStatus = System.Convert.ToInt32(departmentModel.status);
                string requestedWithHeader = Request.Headers["X-Requested-With"];
                return View("GetAllDepartment", data);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> GetAllDepartment(int pageNumber = 1,string? DepartmentName=null,string? UpdatedBy = null,string? UpdatedDate = null,string? SelectedStatus = null)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                DepartmentModel departmentModel = new DepartmentModel();
                int totalRecords = 0;
                int totalPage = 0;
                DepartmentsViewModel departmentsViewModel=new DepartmentsViewModel();
                List<DepartmentViewModel> departments = new List<DepartmentViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    departmentModel.pageNumber= pageNumber;
                    departmentModel.departmentName=DepartmentName;
                    departmentModel.updatedBy = UpdatedBy;
                    if (!string.IsNullOrEmpty(UpdatedDate))
                        departmentModel.updatedDate = DateTime.Parse(UpdatedDate);
                    departmentModel.status = SelectedStatus;
                    var json = JsonConvert.SerializeObject(departmentModel);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    HttpResponseMessage Res = await client.PostAsync("Department/GetAllFilterDepartment", stringContent);

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
                            pageNumber = (int)root["currentPage"];
                            totalRecords = (int)root["totalRecords"];
                            totalPage = (int)root["totalPage"];
                            departments = root["result"].ToObject<List<DepartmentViewModel>>();
                        }
                    }
                }
                var data = new DepartmentsViewModel()
                {
                    CurrentPage = pageNumber,
                    DepartmentViewModels = departments,
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
                data.DepartmentName = !string.IsNullOrEmpty(departmentModel.departmentName) ? departmentModel.departmentName : "";
                data.UpdatedBy = !string.IsNullOrEmpty(departmentModel.updatedBy) ? departmentModel.updatedBy : "";
                data.UpdatedDate = departmentModel.updatedDate;
                return View(data);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ ex.Message}");
                throw ex;
            }
        }
        public IActionResult AddDepartment()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDepartment(DepartmentViewModel departmentViewModel)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                var validation = new DepartmentValidator();
                var result = validation.Validate(departmentViewModel);
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
                        departmentViewModel.IsDeleted = false;
                        departmentViewModel.DatecreatedUtc = DateTime.UtcNow;
                        departmentViewModel.DateModifiedUtc = DateTime.UtcNow;
                        departmentViewModel.CreatedByUserId = HttpContext.Session.GetString("UserId");
                        var json = JsonConvert.SerializeObject(departmentViewModel);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                        HttpResponseMessage Res = await client.PostAsync("Department/PostDepartment", stringContent);

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
                                TempData["RedirectURl"] = "/Department/AddDepartment/";
                            }
                            else
                            {
                                string message= languageService.Getkey("Department Added Successfully");
                                TempData["message"] = message;
                                TempData["RedirectURl"] = "/Department/GetAllDepartment/";
                            }
                        }
                    }
                    return RedirectToAction("GetAllDepartment");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> EditDepartment(long id)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                DepartmentViewModel departmentViewModel = new DepartmentViewModel();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.GetAsync(string.Format("Department/GetDepartmentDetail/{0}", id));

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
                            TempData["RedirectURl"] = "/Department/AddDepartment/";
                        }
                        else
                        {
                            departmentViewModel = root["result"].ToObject<DepartmentViewModel>();
                        }
                    }
                }
                return View(departmentViewModel);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDepartment(DepartmentViewModel departmentViewModel)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                var validation = new DepartmentValidator();
                var result = validation.Validate(departmentViewModel);
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
                        departmentViewModel.DateModifiedUtc = DateTime.UtcNow;
                        departmentViewModel.ModifiedByUserId = HttpContext.Session.GetString("UserId");
                        var json = JsonConvert.SerializeObject(departmentViewModel);
                        var stringcontenet = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpResponseMessage response = await client.PutAsync("Department/PutDepartment", stringcontenet);

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
                                TempData["RedirectURl"] = "/Department/EditDepartment/";
                                return RedirectToAction("EditDepartment", new { id = departmentViewModel.Id });

                            }
                            else
                            {
                                departmentViewModel = root["result"].ToObject<DepartmentViewModel>();
                                string message= languageService.Getkey("Department Updated Successfully");
                                TempData["message"] = message;
                                TempData["RedirectURl"] = "/Department/GetAllDepartment/";
                            }
                        }
                    }
                    return RedirectToAction("GetAllDepartment");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> DeleteDepartment(long id)
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
                    HttpResponseMessage response = await client.DeleteAsync(string.Format("Department/DeleteDepartment/{0}", id));

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
                            string message= languageService.Getkey("Department Deleted Successfully");
                            TempData["message"] = message;
                            TempData["RedirectURl"] = "/Department/GetAllDepartment/";
                        }
                        //var message = root["value"].ToString();
                    }
                }
                return RedirectToAction("GetAllDepartment");
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
    }
}

