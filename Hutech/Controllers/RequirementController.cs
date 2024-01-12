using Hutech.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Hutech.Controllers
{
    public class RequirementController : Controller
    {
        public IConfiguration configuration { get; set; }
        private readonly ILogger<RequirementController> logger;
        public RequirementController(IConfiguration _configuration, ILogger<RequirementController> _logger)
        {
            configuration = _configuration;
            logger = _logger;
        }
        public async Task<IActionResult> GetAllRequirement()
        {
            try
            {
                List<RequirementViewModel> requirements = new List<RequirementViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("Requirement/GetRequirement");

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        requirements = root["result"].ToObject<List<RequirementViewModel>>();
                    }
                }
                return View(requirements);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public IActionResult AddRequirement()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRequirement(RequirementViewModel requirementViewModel)
        {
            try
            {
                var validation = new RequirementValidator();
                var result = validation.Validate(requirementViewModel);
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
                        requirementViewModel.IsDeleted = false;
                        var json = JsonConvert.SerializeObject(requirementViewModel);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                        HttpResponseMessage Res = await client.PostAsync("Requirement/PostRequirement", stringContent);

                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                        }
                    }
                    return RedirectToAction("GetAllRequirement");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> EditRequirement(long id)
        {
            try
            {
                RequirementViewModel requirementViewModel = new RequirementViewModel();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.GetAsync(string.Format("Requirement/GetRequirementDetail/{0}", id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        requirementViewModel = root["result"].ToObject<RequirementViewModel>();
                    }
                }
                return View(requirementViewModel);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditRequirement(RequirementViewModel requirementViewModel)
        {
            try
            {
                var validation = new RequirementValidator();
                var result = validation.Validate(requirementViewModel);
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

                        var json = JsonConvert.SerializeObject(requirementViewModel);
                        var stringcontenet = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.PutAsync("Requirement/PutRequirement", stringcontenet);

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            requirementViewModel = root["result"].ToObject<RequirementViewModel>();
                        }
                    }
                    return RedirectToAction("GetAllRequirement");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> DeleteRequirement(long id)
        {
            try
            {
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.DeleteAsync(string.Format("Requirement/DeleteRequirement/{0}", id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        //var message = root["value"].ToString();
                    }
                }
                return RedirectToAction("GetAllRequirement");
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
    }
}
