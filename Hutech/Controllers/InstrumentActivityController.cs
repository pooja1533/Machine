using Hutech.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace Hutech.Controllers
{
    public class InstrumentActivityController : Controller
    {
        public IConfiguration configuration { get; set; }
        private readonly ILogger<InstrumentActivityController> logger;
        public InstrumentActivityController(IConfiguration _configuration,ILogger<InstrumentActivityController> _logger)
        {
            logger = _logger;
            configuration = _configuration;
        }
        public async Task<IActionResult> AddInstrumentActivity()
        {
            InstrumentActivityViewModel instrumentActivityViewModel = new InstrumentActivityViewModel();
            try
            {
                List<InstrumentViewModel> instrument = new List<InstrumentViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("Instrument/GetInstrument");

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        instrument = root["result"].ToObject<List<InstrumentViewModel>>();
                    }
                }
                var data = instrument.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();


                List<ActivityViewModel> activities = new List<ActivityViewModel>();
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
                var activity = activities.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();

                List<RequirementViewModel> requirements = new List<RequirementViewModel>();
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
                var requirementdata = requirements.Select(x => new SelectListItem
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
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("Department/GetDepartment");

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        departments = root["result"].ToObject<List<DepartmentViewModel>>();
                    }
                }
                var departmentdata = departments.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();

                instrumentActivityViewModel.Requirement = requirementdata;
                instrumentActivityViewModel.Instruments = data;
                instrumentActivityViewModel.Activities = activity;
                instrumentActivityViewModel.Department = departmentdata;
                return View(instrumentActivityViewModel);

            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
    }
}
