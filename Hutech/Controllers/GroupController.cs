using Hutech.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Office2010.Excel;
using Hutech;
using Microsoft.Extensions.Localization;
using Hutech.Resources;

namespace Hutech.Controllers
{
    [Authorize]
    public class GroupController : Controller
    {
        public IConfiguration configuration { get; set; }
        private readonly ILogger<GroupController> logger;
        private readonly LanguageService languageService; 
        public GroupController(IConfiguration _configuration, ILogger<GroupController> _logger, LanguageService _languageService)
        {
            configuration = _configuration;
            logger = _logger;
            languageService = _languageService;
        }
        public async Task<IActionResult> GetAllGroup(bool CallFirstTime=false)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                List<GroupViewModel> group = new List<GroupViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("Group/GetAllGroup");

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
                            group = root["result"].ToObject<List<GroupViewModel>>();
                        }
                    }
                }
                if (CallFirstTime)
                {
                    TempData.Remove("message");
                    TempData.Remove("RedirectURl");
                }
                return View(group);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }

        public IActionResult AddGroup()
        {
            return View();
        }
        [ValidateAntiForgeryToken]

        [HttpPost]
        public async Task<IActionResult> AddGroup(GroupViewModel groupViewModel)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                var validation = new GroupValidator();
                var result = validation.Validate(groupViewModel);
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
                        groupViewModel.IsDeleted = false;
                        var json = JsonConvert.SerializeObject(groupViewModel);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                        HttpResponseMessage Res = await client.PostAsync("Group/PostGroup", stringContent);

                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                           var resultData = root["success"].ToString();
                            if(resultData=="False" || resultData=="false")
                            {
                                var Id = root["auditId"].ToString();
                                TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                                TempData["RedirectURl"] = "/Group/AddGroup/";
                            }
                            else
                            {
                                TempData["message"] = languageService.Getkey("Group Added Successfully");
                                TempData["RedirectURl"] = "/Group/GetAllGroup/";
                            }
                        }
                        //else
                        //{
                        //    var content = await Res.Content.ReadAsStringAsync();
                        //    string FinalString;
                        //    string firststring = "System.Exception: ";
                        //    string secondtsring =  "at Hutech.";
                        //    int Pos1 = content.IndexOf(firststring);
                        //    int Pos2 = content.IndexOf(secondtsring);
                        //    FinalString = content.Substring(Pos1, Pos2 - Pos1);
                        //    var number=Regex.Match(FinalString, @"\d+\.*\d*").Value;

                        //    TempData["message"] = "Something went wrong please contact your Administrator Id is "+ number;
                        //    //TempData["RedirectURl"] = "/Group/AddGroup/";
                        //}
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

        public async Task<IActionResult> EditGroup(long id)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                GroupViewModel groupViewModel = new GroupViewModel();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.GetAsync(string.Format("Group/GetGroupDetail/{0}", id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var Id = root["auditId"].ToString();
                            TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                            TempData["RedirectURl"] = "/Group/AddGroup/";
                        }
                        else
                        {
                            groupViewModel = root["result"].ToObject<GroupViewModel>();
                        }

                    }
                }
                return View(groupViewModel);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        [ValidateAntiForgeryToken]

        [HttpPost]
        public async Task<IActionResult> EditGroup(GroupViewModel groupViewModel)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                var validation = new GroupValidator();
                var result = validation.Validate(groupViewModel);
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

                        var json = JsonConvert.SerializeObject(groupViewModel);
                        var stringcontenet = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpResponseMessage response = await client.PutAsync("Group/PutGroup", stringcontenet);

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            var resultData = root["success"].ToString();
                            if (resultData == "False" || resultData == "false")
                            {
                                var Id = root["auditId"].ToString();
                                TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                                TempData["RedirectURl"] = "/Group/EditGroup/";
                                return RedirectToAction("EditGroup", new { id = groupViewModel.Id });

                            }
                            else
                            {
                                groupViewModel = root["result"].ToObject<GroupViewModel>();
                                TempData["message"] = languageService.Getkey("Group Updated Successfully");
                                TempData["RedirectURl"] = "/Group/GetAllGroup/";
                            }
                        }
                    }
                    return RedirectToAction("GetAllGroup");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> DeleteGroup(long id)
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
                    HttpResponseMessage response = await client.DeleteAsync(string.Format("Group/DeleteGroup/{0}", id));

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
                            TempData["message"] = languageService.Getkey("Group Deleted Successfully");
                            TempData["RedirectURl"] = "/Group/GetAllGroup/";
                        }
                    }
                }
                return RedirectToAction("GetAllGroup");
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
    }
}
