using Hutech.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Protocol.Plugins;
using System.Net.Http.Headers;
using System.Text;

namespace Hutech.Controllers
{
    public class MachineController : Controller
    {
        private readonly ILogger<MachineController> logger;
        private readonly IConfiguration configuration;
        private string BaseUrl = string.Empty;
        public MachineController(ILogger<MachineController> _logger, IConfiguration _configuration)
        {
            logger = _logger;
            configuration = _configuration;
            BaseUrl = configuration["Baseurl"];
        }
        public IActionResult AddMachine()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddMachine(MachineViewModel machineViewModel)
        {
            try
            {
                var machineValidator = new MachineValidator();
                var validatorResult = machineValidator.Validate(machineViewModel);
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

                        var json = JsonConvert.SerializeObject(machineViewModel);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                        HttpResponseMessage Res = await client.PostAsync("Machine/PostMachine", stringContent);

                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                        }
                    }
                }

                return RedirectToAction("GetAllMachine");
            }
            catch(Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> DeleteMachine(long id)
        {
            try
            {
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.DeleteAsync(string.Format("Machine/DeleteMachine/{0}", id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        //var message = root["value"].ToString();
                    }
                }
                return RedirectToAction("GetAllMachine");
            }
            catch(Exception ex) {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> GetAllMachine()
        {
            try
            {
                List<MachineViewModel> machines = new List<MachineViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("Machine/GetMachine");

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        machines = root["result"].ToObject<List<MachineViewModel>>();
                    }
                }
                logger.LogInformation($"Get All machine method executed successfully {DateTime.Now} at controller level");
                return View(machines);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> EditMachine(long id)
        {
            try
            {
                MachineViewModel machine = new MachineViewModel();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.GetAsync(string.Format("Machine/GetMachineById/{0}", id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        machine = root["result"].ToObject<MachineViewModel>();
                    }
                }
                machine.PurchaseDate = machine.PurchaseDate.Date;
                return View(machine);
            }
            catch(Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditMachine(MachineViewModel machineViewModel)
        {
            try
            {
                var machineValidator = new MachineValidator();
                var validatorResult = machineValidator.Validate(machineViewModel);
                if (!validatorResult.IsValid)
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

                        var json = JsonConvert.SerializeObject(machineViewModel);
                        var stringcontenet = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                        HttpResponseMessage response = await client.PutAsync("Machine/UpdateMachine", stringcontenet);

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            machineViewModel = root["result"].ToObject<MachineViewModel>();
                        }
                    }
                    return RedirectToAction("GetAllMachine");
                }
            }
            catch(Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddComment(string Comment, string commentDate, string Id)
        {
            try
            {
                MachineCommentViewModel machineCommentViewModel=new MachineCommentViewModel();
                DateTime date=System.Convert.ToDateTime(commentDate);
                machineCommentViewModel.CommentDate = date;
                machineCommentViewModel.Comment = Comment;
                machineCommentViewModel.MachineId = System.Convert.ToInt64(Id);
                machineCommentViewModel.CreatedDateTime = DateTime.UtcNow;
                machineCommentViewModel.ModifiedDateTime = DateTime.UtcNow;
                machineCommentViewModel.CreatedBy = HttpContext.Session.GetString("UserId");
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var json = JsonConvert.SerializeObject(machineCommentViewModel);
                    var stringcontenet = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync("Machine/PostComment", stringcontenet);

                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { success = true, responseText = "Comment Added successfully." });

                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
            return Json(new { success = false, responseText = "Something went wrong." });
        }
    }
}
