using Hutech.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using System.Configuration;
using System.Data;
using System.Net.Http.Headers;
using System.Text;

namespace Hutech.Controllers
{
    //[ServiceFilter(typeof(LogActionAttribute))]
    public class RoleController : Controller
    {
        private readonly IConfiguration configuration;
        private string BaseUrl = string.Empty;
        private readonly ILogger<RoleController> logger;
        public RoleController(IConfiguration _configuration, ILogger<RoleController> logger)
        {
            configuration = _configuration;
            BaseUrl = configuration["Baseurl"];
            this.logger = logger;
        }
        public async Task<IActionResult> EditRole(Guid id)
        {
            try
            {
                RoleViewModel roleViewModel = new RoleViewModel();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.GetAsync(string.Format("Role/GetRoleDetail/{0}", id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        roleViewModel = root["result"].ToObject<RoleViewModel>();
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
        [HttpPost]
        public async Task<IActionResult> EditRole(RoleViewModel roleViewModel)
        {
            try
            {
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

                        var json = JsonConvert.SerializeObject(roleViewModel);
                        var stringcontenet = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.PutAsync("Role/PutRole", stringcontenet);

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            roleViewModel = root["result"].ToObject<RoleViewModel>();
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
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //var uri = $"{BaseUrl}{string.Format("Role/DeleteRole/?Id={0}", id)}";
                    HttpResponseMessage response = await client.DeleteAsync(string.Format("Role/DeleteRole/{0}", id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        //var message = root["value"].ToString();
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
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var loggedinuser = HttpContext.Session.GetString("LoogedInUser");
                //logger.LogInformation($"Get All Roles method call by {loggedinuser} {DateTime.Now} at controller level");
                List<RoleViewModel> roles = new List<RoleViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("Role/GetRoles");

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        roles = root["result"].ToObject<List<RoleViewModel>>();
                    }
                }
                //logger.LogInformation($"Get All Roles method executed successfully {DateTime.Now} at controller level");
                return View(roles);
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
        public async Task<IActionResult> AddRoleAsync(RoleViewModel roleViewModel)
        {
            try
            {
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
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                        var json = JsonConvert.SerializeObject(roleViewModel);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                        HttpResponseMessage Res = await client.PostAsync("Role/PostRole", stringContent);

                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
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
    }
}
