using Hutech.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Net.Http.Headers;
using System.Text;

namespace Hutech.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<UserController> logger;
        public UserController(IConfiguration _configuration, ILogger<UserController> _logger)
        {
            configuration = _configuration;
            logger = _logger;
        }
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var loggedinuser = HttpContext.Session.GetString("LoogedInUser");
                logger.LogInformation($"Get All Users method call by {loggedinuser} {DateTime.Now} at controller level");
                List<UserViewModel> users = new List<UserViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var loggedinUserRole = HttpContext.Session.GetString("UserRole").ToString();
                    var loggedinuserId = HttpContext.Session.GetString("UserId").ToString();
                    HttpResponseMessage Res = await client.GetAsync(string.Format("User/GetAllusers/"+loggedinUserRole +"/"+loggedinuserId));

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        users = root["result"].ToObject<List<UserViewModel>>();
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
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage Res = await client.DeleteAsync(string.Format("User/DeleteUser/Id=" + id));

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
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
                UserViewModel user = new UserViewModel();
                List<RoleViewModel> roles = new List<RoleViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync(string.Format("User/GetUserById/Id=" + id));

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        user = root["result"].ToObject<UserViewModel>();
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
                UserViewModel user = new UserViewModel();
                List<RoleViewModel> roles = new List<RoleViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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



                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var json = JsonConvert.SerializeObject(userViewModel);
                    var stringcontenet = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    HttpResponseMessage Res = await client.PutAsync("User/UpdateUser", stringcontenet);

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        user = root["result"].ToObject<UserViewModel>();
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
