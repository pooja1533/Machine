using Hutech.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace Hutech.Controllers
{
    public class AuditTrailController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<AuditTrailController> logger;
        public AuditTrailController(IConfiguration _configuration, ILogger<AuditTrailController> _logger)
        {
            configuration = _configuration;
            logger = _logger;
        }
        public async Task<IActionResult> GetAllAuditTrail(string? fdate,string? tdate,string? keyword)
        {
            DateTime endDate = DateTime.UtcNow;
            DateTime startDate = DateTime.UtcNow.AddDays(-7);
            
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                string fDate = string.Empty;
                string tDate = string.Empty;
                List<AuditViewModel> audit = new List<AuditViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    if (string.IsNullOrEmpty(fdate)){
                        fDate = startDate.ToString("yyyy-MM-dd");
                        fDate = fDate.Replace("/", "-");
                    }
                    else
                    {
                        fDate = fdate.Replace("/", "-");
                    }
                    if (string.IsNullOrEmpty(tdate))
                    {
                        tDate = endDate.ToString("yyyy-MM-dd");
                        tDate = tDate.Replace("/", "-");
                    }
                    else
                    {
                        tDate = tdate.Replace("/", "-");
                    }
                    if (string.IsNullOrEmpty(keyword))
                        keyword = "null";
                    //tDate =endDate.ToShortDateString();
                    //tDate=tDate.Replace("/", "-");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    string url = string.Format("AuditTrail/GetAuditTrail/{0}/{1}/{2}", fDate,tDate,keyword);
                    apiUrl += url;

                    HttpResponseMessage Res = await client.GetAsync(apiUrl);

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        audit = root["result"].ToObject<List<AuditViewModel>>();
                    }
                }
                ViewBag.StartDate = fDate;
                ViewBag.EndDate= tDate;
                if(keyword=="null")
                {
                    keyword = "";
                }
                ViewBag.Keyword = keyword;
                return View(audit);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
            return View();
        }
    }
}
