using Hutech.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace Hutech.Controllers
{
    public class InstrumentController : Controller
    {
        public IConfiguration configuration { get; set; }
        private readonly ILogger<InstrumentController> logger;
        public InstrumentController(IConfiguration _configuration, ILogger<InstrumentController> _logger)
        {
            configuration = _configuration;
            logger = _logger;
        }
        public async Task<IActionResult> GetAllInstrument()
        {
            try
            {
                List<InstrumentViewModel> departments = new List<InstrumentViewModel>();
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
                        departments = root["result"].ToObject<List<InstrumentViewModel>>();
                    }
                }
                return View(departments);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public IActionResult AddInstrument()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddInstrument(InstrumentViewModel instrumentViewModel)
        {
            try
            {
                var validation = new InstrumentValidator();
                var result = validation.Validate(instrumentViewModel);
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
                        instrumentViewModel.IsDeleted = false;
                        var json = JsonConvert.SerializeObject(instrumentViewModel);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                        HttpResponseMessage Res = await client.PostAsync("Instrument/PostInstrument", stringContent);

                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                        }
                    }
                    return RedirectToAction("GetAllInstrument");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> EditInstrument(long id)
        {
            try
            {
                InstrumentViewModel instrumentViewModel = new InstrumentViewModel();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.GetAsync(string.Format("Instrument/GetInstrumentDetail/{0}", id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        instrumentViewModel = root["result"].ToObject<InstrumentViewModel>();
                    }
                }
                return View(instrumentViewModel);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditInstrument(InstrumentViewModel instrumentViewModel)
        {
            try
            {
                var validation = new InstrumentValidator();
                var result = validation.Validate(instrumentViewModel);
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

                        var json = JsonConvert.SerializeObject(instrumentViewModel);
                        var stringcontenet = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.PutAsync("Instrument/PutInstrument", stringcontenet);

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            instrumentViewModel = root["result"].ToObject<InstrumentViewModel>();
                        }
                    }
                    return RedirectToAction("GetAllInstrument");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> DeleteInstrument(long id)
        {
            try
            {
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.DeleteAsync(string.Format("Instrument/DeleteInstrument/{0}", id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        //var message = root["value"].ToString();
                    }
                }
                return RedirectToAction("GetAllInstrument");
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
    }
}
