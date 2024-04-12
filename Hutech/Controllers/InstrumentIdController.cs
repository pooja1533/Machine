using Hutech.Models;
using Hutech.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Hutech.Controllers
{
    [Authorize]
    public class InstrumentIdController : Controller
    {
        public IConfiguration configuration { get; set; }
        private readonly ILogger<InstrumentIdController> logger;
        private readonly LanguageService languageService;
        public InstrumentIdController(IConfiguration _configuration, ILogger<InstrumentIdController> _logger, LanguageService _languageService)
        {
            configuration = _configuration;
            logger = _logger;
            languageService = _languageService;
        }
        public async Task<IActionResult> AddInstrumentIdAsync()
        {
            var token = Request.Cookies["jwtCookie"];
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();

                token = token.Replace("Bearer ", "");
            }
            string apiUrl = configuration["Baseurl"];
            InstrumentIdViewModel instrumentId = new InstrumentIdViewModel();
            try
            {
                List<InstrumentViewModel> instrument = new List<InstrumentViewModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("Instrument/GetActiveInstrument");

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


                //Location
                List<LocationViewModel> locations = new List<LocationViewModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("Location/GetActiveLocation");

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        locations = root["result"].ToObject<List<LocationViewModel>>();
                    }
                }
                var location = locations.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();

                //Team

                List<TeamViewModel> teams = new List<TeamViewModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("Team/GetActiveTeam");

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        teams = root["result"].ToObject<List<TeamViewModel>>();
                    }
                }
                var team = teams.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();
                instrumentId.Teams = team;
                instrumentId.Instruments = data;
                instrumentId.Locations = location;
            }

            catch (Exception ex)
            {

            }
            return View(instrumentId);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddInstrumentId(InstrumentIdViewModel instrumentIdViewModel)
        {
            var token = Request.Cookies["jwtCookie"];
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();

                token = token.Replace("Bearer ", "");
            }
            var validation = new InstrumentIdViewModelValidator();
            var result = validation.Validate(instrumentIdViewModel);
            string apiUrl = configuration["Baseurl"];


            List<InstrumentViewModel> instrument = new List<InstrumentViewModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                HttpResponseMessage Res = await client.GetAsync("Instrument/GetActiveInstrument");

                if (Res.IsSuccessStatusCode)
                {
                    var content = await Res.Content.ReadAsStringAsync();
                    JObject root = JObject.Parse(content);
                    instrument = root["result"].ToObject<List<InstrumentViewModel>>();
                }
            }
            var instruments = instrument.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            List<LocationViewModel> locations = new List<LocationViewModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                HttpResponseMessage Res = await client.GetAsync("Location/GetActiveLocation");

                if (Res.IsSuccessStatusCode)
                {
                    var content = await Res.Content.ReadAsStringAsync();
                    JObject root = JObject.Parse(content);
                    locations = root["result"].ToObject<List<LocationViewModel>>();
                }
            }
            var location = locations.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            List<TeamViewModel> teams = new List<TeamViewModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                HttpResponseMessage Res = await client.GetAsync("Team/GetActiveTeam");

                if (Res.IsSuccessStatusCode)
                {
                    var content = await Res.Content.ReadAsStringAsync();
                    JObject root = JObject.Parse(content);
                    teams = root["result"].ToObject<List<TeamViewModel>>();
                }
            }
            var team = teams.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            if (!result.IsValid)
            {
                instrumentIdViewModel.Locations = location;
                instrumentIdViewModel.Teams = team;
                instrumentIdViewModel.Instruments = instruments;
                return View(instrumentIdViewModel);
            }
            else
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        instrumentIdViewModel.DatecreatedUtc = DateTime.UtcNow;
                        instrumentIdViewModel.DateModifiedUtc = DateTime.UtcNow;
                        instrumentIdViewModel.CreatedByUserId= HttpContext.Session.GetString("UserId");
                        var json = JsonConvert.SerializeObject(instrumentIdViewModel);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                        HttpResponseMessage Res = await client.PostAsync("InstrumentId/PostInstrumentId", stringContent);

                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            var resultData = root["success"].ToString();
                            if (resultData == "False" || resultData == "false")
                            {
                                var Id = root["auditId"].ToString();
                                TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                                TempData["RedirectURl"] = "/InstrumentId/AddInstrumentId/";
                            }
                            else
                            {
                                TempData["message"] = languageService.Getkey("InstrumentId Added Successfully");
                                TempData["RedirectURl"] = "/InstrumentId/GetAllInstrumentId/";
                            }
                        }
                    }
                    return RedirectToAction("GetAllInstrumentId");
                }

                catch (Exception ex)
                {
                    logger.LogInformation($"Exception Occure.{ex.Message}");
                    throw ex;
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetAllInstrumentId([FromBody] InstrumentIdModel instrumentIdModel)
        {
            instrumentIdModel.PageNumber = 1;
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
                List<InstrumentIdViewModel> instrumentIdViewModels = new List<InstrumentIdViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var json = JsonConvert.SerializeObject(instrumentIdModel);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    HttpResponseMessage Res = await client.PostAsync("InstrumentId/GetAllFilterInstrumentId", stringContent);

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
                            instrumentIdViewModels = root["result"].ToObject<List<InstrumentIdViewModel>>();
                            pageNumber = (int)root["currentPage"];
                            totalRecords = (int)root["totalRecords"];
                            totalPage = (int)root["totalPage"];
                        }
                    }
                }
                var data = new InstrumentIdsViewModel()
                {
                    CurrentPage = pageNumber,
                    instrumentIdViewModels = instrumentIdViewModels,
                    TotalPages = totalPage,
                    TotalRecords = totalRecords
                };
                data.InstrumentIdName = !string.IsNullOrEmpty(instrumentIdModel.InstrumentIdName) ? instrumentIdModel.InstrumentIdName : "";
                data.Model = !string.IsNullOrEmpty(instrumentIdModel.Model) ? instrumentIdModel.Model : "";
                data.InstrumentName = !string.IsNullOrEmpty(instrumentIdModel.InstrumentName) ? instrumentIdModel.InstrumentName : "";
                data.InstrumentSerial = !string.IsNullOrEmpty(instrumentIdModel.InstrumentSerial) ? instrumentIdModel.InstrumentSerial : "";
                data.InstrumentLocation = !string.IsNullOrEmpty(instrumentIdModel.InstrumentLocation) ? instrumentIdModel.InstrumentLocation : "";
                data.TeamName = !string.IsNullOrEmpty(instrumentIdModel.TeamName) ? instrumentIdModel.TeamName : "";
                data.UpdatedBy = !string.IsNullOrEmpty(instrumentIdModel.UpdatedBy) ? instrumentIdModel.UpdatedBy : "";
                //data.Status = locationModel.status;
                data.UpdatedDate = instrumentIdModel.UpdatedDate;
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
                data.SelectedStatus = System.Convert.ToInt32(instrumentIdModel.Status);
                string requestedWithHeader = Request.Headers["X-Requested-With"];
                data.InstrumentIdName = !string.IsNullOrEmpty(instrumentIdModel.InstrumentIdName) ? instrumentIdModel.InstrumentIdName : "";
                data.Model = !string.IsNullOrEmpty(instrumentIdModel.Model) ? instrumentIdModel.Model : "";
                data.InstrumentName = !string.IsNullOrEmpty(instrumentIdModel.InstrumentName) ? instrumentIdModel.InstrumentName : "";
                data.InstrumentSerial = !string.IsNullOrEmpty(instrumentIdModel.InstrumentSerial) ? instrumentIdModel.InstrumentSerial : "";
                data.InstrumentLocation = !string.IsNullOrEmpty(instrumentIdModel.InstrumentLocation) ? instrumentIdModel.InstrumentLocation : "";
                data.TeamName = !string.IsNullOrEmpty(instrumentIdModel.TeamName) ? instrumentIdModel.TeamName : "";
                data.UpdatedBy = !string.IsNullOrEmpty(instrumentIdModel.UpdatedBy) ? instrumentIdModel.UpdatedBy : "";
                data.UpdatedDate = instrumentIdModel.UpdatedDate;
                return View("GetAllInstrumentId", data);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> GetAllInstrumentId(int pageNumber = 1, string? instrumentIdName = null, string? model = null, string? instrumentName = null, string? instrumentSerial = null,string? instrumentLocation=null,string? teamName=null, string? updatedBy = null, string? updatedDate = null, string? Status = null)
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
                InstrumentIdsViewModel instrumentIdsViewModel=new InstrumentIdsViewModel();
                List<InstrumentIdViewModel> instrumentIds = new List<InstrumentIdViewModel>();
                string apiUrl = configuration["Baseurl"];
                InstrumentIdModel instrumentIdModel=new InstrumentIdModel();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    instrumentIdModel.InstrumentIdName = instrumentIdName;
                    instrumentIdModel.Model = model;
                    instrumentIdModel.InstrumentName = instrumentName;
                    instrumentIdModel.InstrumentSerial = instrumentSerial;
                    instrumentIdModel.InstrumentLocation = instrumentLocation;
                    instrumentIdModel.TeamName = teamName;
                    instrumentIdModel.UpdatedBy = updatedBy;
                    instrumentIdModel.PageNumber = pageNumber;
                    if (!string.IsNullOrEmpty(updatedDate))
                        instrumentIdModel.UpdatedDate = DateTime.Parse(updatedDate);
                    instrumentIdModel.Status = Status;
                    var json = JsonConvert.SerializeObject(instrumentIdModel);
                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                    HttpResponseMessage Res = await client.PostAsync("InstrumentId/GetAllFilterInstrumentId", stringContent);
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
                            instrumentIds = root["result"].ToObject<List<InstrumentIdViewModel>>();
                            pageNumber = (int)root["currentPage"];
                            totalRecords = (int)root["totalRecords"];
                            totalPage = (int)root["totalPage"];
                        }
                    }
                }
                var data = new InstrumentIdsViewModel()
                {
                    CurrentPage = pageNumber,
                    instrumentIdViewModels = instrumentIds,
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
                if (Status == null)
                    data.SelectedStatus = (int)StatusViewModel.Active;
                else if (!string.IsNullOrEmpty(Status))
                    data.SelectedStatus = System.Convert.ToInt32(Status);
                data.InstrumentIdName = !string.IsNullOrEmpty(instrumentIdModel.InstrumentIdName) ? instrumentIdModel.InstrumentIdName : "";
                data.Model = !string.IsNullOrEmpty(instrumentIdModel.Model) ? instrumentIdModel.Model : "";
                data.InstrumentName = !string.IsNullOrEmpty(instrumentIdModel.InstrumentName) ? instrumentIdModel.InstrumentName : "";
                data.InstrumentSerial = !string.IsNullOrEmpty(instrumentIdModel.InstrumentSerial) ? instrumentIdModel.InstrumentSerial : "";
                data.InstrumentLocation = !string.IsNullOrEmpty(instrumentIdModel.InstrumentLocation) ? instrumentIdModel.InstrumentLocation : "";
                data.TeamName = !string.IsNullOrEmpty(instrumentIdModel.TeamName) ? instrumentIdModel.TeamName : "";
                data.UpdatedBy = !string.IsNullOrEmpty(instrumentIdModel.UpdatedBy) ? instrumentIdModel.UpdatedBy : "";
                data.UpdatedDate = instrumentIdModel.UpdatedDate;
                return View(data);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> DeleteInstrumentId(long id)
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
                    HttpResponseMessage response = await client.DeleteAsync(string.Format("InstrumentId/DeleteInstrumentId/{0}", id));

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
                            TempData["message"] = languageService.Getkey("InstrumentId Deleted Successfully");
                            TempData["RedirectURl"] = "/InstrumentId/GetAllInstrumentId/";
                        }
                        //var message = root["value"].ToString();
                    }
                }
                return RedirectToAction("GetAllInstrumentId");
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        public async Task<IActionResult> EditInstrumentId(long id)
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
                InstrumentIdViewModel activityViewModel = new InstrumentIdViewModel();
                List<InstrumentViewModel> instrument = new List<InstrumentViewModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("Instrument/GetActiveInstrument");

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


                //Location
                List<LocationViewModel> locations = new List<LocationViewModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("Location/GetActiveLocation");

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        locations = root["result"].ToObject<List<LocationViewModel>>();
                    }
                }
                var location = locations.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();

                //Team

                List<TeamViewModel> teams = new List<TeamViewModel>();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync("Team/GetActiveTeam");

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        teams = root["result"].ToObject<List<TeamViewModel>>();
                    }
                }
                var team = teams.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.GetAsync(string.Format("InstrumentId/GetInstrumentIdDetail/{0}", id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var Id = root["auditId"].ToString();
                            TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                            TempData["RedirectURl"] = "/InstrumentId/AddInstrumentId/";
                        }
                        else
                        {
                            activityViewModel = root["result"].ToObject<InstrumentIdViewModel>();
                        }
                    }
                }
                activityViewModel.Teams = team;
                activityViewModel.Instruments = data;
                activityViewModel.Locations = location;
                return View(activityViewModel);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInstrumentId(InstrumentIdViewModel instrumentIdViewModel)
        {
            string apiUrl = configuration["Baseurl"];
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                var validation = new InstrumentIdViewModelValidator();
                var result = validation.Validate(instrumentIdViewModel);
                if (!result.IsValid)
                {
                    List<InstrumentViewModel> instrument = new List<InstrumentViewModel>();
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                        HttpResponseMessage Res = await client.GetAsync("Instrument/GetActiveInstrument");

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


                    //Location
                    List<LocationViewModel> locations = new List<LocationViewModel>();
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                        HttpResponseMessage Res = await client.GetAsync("Location/GetActiveLocation");

                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            locations = root["result"].ToObject<List<LocationViewModel>>();
                        }
                    }
                    var location = locations.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }).ToList();

                    //Team

                    List<TeamViewModel> teams = new List<TeamViewModel>();
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                        HttpResponseMessage Res = await client.GetAsync("Team/GetActiveTeam");

                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            teams = root["result"].ToObject<List<TeamViewModel>>();
                        }
                    }
                    var team = teams.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }).ToList();
                    instrumentIdViewModel.Teams = team;
                    instrumentIdViewModel.Instruments = data;
                    instrumentIdViewModel.Locations = location;
                    return View(instrumentIdViewModel);
                }
                else
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        instrumentIdViewModel.DateModifiedUtc= DateTime.UtcNow;
                        instrumentIdViewModel.ModifiedByUserId= HttpContext.Session.GetString("UserId");
                        var json = JsonConvert.SerializeObject(instrumentIdViewModel);
                        var stringcontenet = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpResponseMessage response = await client.PutAsync("InstrumentId/PutInstrumentId", stringcontenet);

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            var resultData = root["success"].ToString();
                            if (resultData == "False" || resultData == "false")
                            {
                                var Id = root["auditId"].ToString();
                                TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                                TempData["RedirectURl"] = "/InstrumentId/EditInstrumentId/";
                                return RedirectToAction("InstrumentId", new { id = instrumentIdViewModel.Id });

                            }
                            else
                            {
                                instrumentIdViewModel = root["result"].ToObject<InstrumentIdViewModel>();
                                TempData["message"] = languageService.Getkey("InstrumentId Updated Successfully");
                                TempData["RedirectURl"] = "/InstrumentId/GetAllInstrumentId/";
                            }
                        }
                    }
                    return RedirectToAction("GetAllInstrumentId");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }

    }

}

