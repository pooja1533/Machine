using Hutech.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Hutech.Controllers
{
    public class ActivityController : Controller
    {
        public IConfiguration configuration { get; set; }
        private readonly ILogger<ActivityController> logger;
        public ActivityController(IConfiguration _configuration, ILogger<ActivityController> _logger)
        {
            configuration = _configuration;
            logger = _logger;
        }
        public async Task<IActionResult> GetAllActivity()
        {
            try
            {
                List<ActivityViewModel> activities = new List<ActivityViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("Activity/GetActivity");

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        activities = root["result"].ToObject<List<ActivityViewModel>>();
                    }
                }
                return View(activities);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public IActionResult AddActivity()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddActivity(ActivityViewModel activityViewModel)
        {
            try
            {
                var validation = new ActivityValidator();
                var result = validation.Validate(activityViewModel);
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
                        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        activityViewModel.IsDeleted = false;
                        var json = JsonConvert.SerializeObject(activityViewModel);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                        HttpResponseMessage Res = await client.PostAsync("Activity/PostActivity", stringContent);

                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                        }
                    }
                    return RedirectToAction("GetAllActivity");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> EditActivity(long id)
        {
            try
            {
                ActivityViewModel activityViewModel = new ActivityViewModel();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.GetAsync(string.Format("Activity/GetActivityDetail/{0}", id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        activityViewModel = root["result"].ToObject<ActivityViewModel>();
                    }
                }
                return View(activityViewModel);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditActivity(ActivityViewModel activityViewModel)
        {
            try
            {
                var validation = new ActivityValidator();
                var result = validation.Validate(activityViewModel);
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

                        var json = JsonConvert.SerializeObject(activityViewModel);
                        var stringcontenet = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.PutAsync("Activity/PutActivity", stringcontenet);

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            activityViewModel = root["result"].ToObject<ActivityViewModel>();
                        }
                    }
                    return RedirectToAction("GetAllActivity");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> DeleteActivity(long id)
        {
            try
            {
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.DeleteAsync(string.Format("Activity/DeleteActivity/{0}", id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        //var message = root["value"].ToString();
                    }
                }
                return RedirectToAction("GetAllActivity");
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
    }
}
