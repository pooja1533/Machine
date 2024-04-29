using Hutech.Models;
using Hutech.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Bibliography;

namespace Hutech.Controllers
{
    public class OrganizationController : Controller
    {
        public IConfiguration configuration { get; set; }
        private readonly ILogger<GroupController> logger;
        private readonly LanguageService languageService;
        public OrganizationController(IConfiguration _configuration, ILogger<GroupController> _logger, LanguageService _languageService)
        {
            configuration=_configuration;
            logger= _logger;
            languageService= _languageService;
        }
        public async Task <IActionResult> GetAllOrganization(int pageNumber = 1)
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
                List<OrganizationViewModel> organizations = new List<OrganizationViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.GetAsync(string.Format("Organization/GetOrganization/{0}", pageNumber));

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
                            organizations = root["result"].ToObject<List<OrganizationViewModel>>();
                            pageNumber = (int)root["currentPage"];
                            totalRecords = (int)root["totalRecords"];
                            totalPage = (int)root["totalPage"];
                        }
                    }
                }
                var data = new GridData<OrganizationViewModel>()
                {
                    CurrentPage = pageNumber,
                    GridRecords = organizations,
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
        public IActionResult AddOrganization()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task <IActionResult> AddOrganization(OrganizationViewModel organizationViewModel)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                var validation = new OrganizationValidator();
                var result = validation.Validate(organizationViewModel);
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
                        organizationViewModel.IsDeleted = false;
                        organizationViewModel.DateCreatedUtc = DateTime.UtcNow;
                        organizationViewModel.DateModifiedUtc = DateTime.UtcNow;
                        if (organizationViewModel.Id > 0)
                        {
                            organizationViewModel.IsActive = organizationViewModel.IsActive;
                            organizationViewModel.DateModifiedUtc = DateTime.UtcNow;
                            organizationViewModel.ModifiedByUserId= HttpContext.Session.GetString("UserId");
                        }
                        else
                        {
                            organizationViewModel.IsActive = true;

                        }
                        organizationViewModel.CreatedByUserId= HttpContext.Session.GetString("UserId");
                        var json = JsonConvert.SerializeObject(organizationViewModel);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                        HttpResponseMessage Res = await client.PostAsync("Organization/PostOrganization", stringContent);

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
                                TempData["RedirectURl"] = "/Organization/AddOrganization/";
                            }
                            else
                            {
                                string message = languageService.Getkey("Organization Added Successfully");
                                TempData["message"] = message;
                                TempData["RedirectURl"] = "/Organization/GetAllOrganization/";
                            }
                        }
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }

        public async Task<IActionResult> DeleteOrganization(long id)
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
                    HttpResponseMessage response = await client.DeleteAsync(string.Format("Organization/DeleteOrganization/{0}", id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
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
                            string message = languageService.Getkey("Organization Deleted Successfully");
                            TempData["message"] = message;
                            TempData["RedirectURl"] = "/Organization/GetAllOrganization/";
                        }
                        //var message = root["value"].ToString();
                    }
                }
                return RedirectToAction("GetAllOrganization");
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;

            }
        }
        public async Task<IActionResult> EditOrganization(int Id)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                OrganizationViewModel organizationViewModel = new OrganizationViewModel();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.GetAsync(string.Format("Organization/GetOrganizationDetail/{0}", Id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var id = root["auditId"].ToString();
                            string message = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + id;
                            TempData["message"] = message;
                            TempData["RedirectURl"] = "/Organization/AddOrganization/";
                        }
                        else
                        {
                            organizationViewModel = root["result"].ToObject<OrganizationViewModel>();
                        }
                    }
                }
                return View(organizationViewModel);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
    }
}
