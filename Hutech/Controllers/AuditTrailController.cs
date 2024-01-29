using ClosedXML.Excel;
using Hutech.Models;
using Irony.Ast;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using System.Composition;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;

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
        public async Task<IActionResult> ExportAuditTrail(string? fdate, string? tdate, string? keyword)
        {
            DateTime endDate = DateTime.UtcNow;
            DateTime startDate = DateTime.UtcNow.AddDays(-7);
            var csvBuilder = new StringBuilder();
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
                    if (string.IsNullOrEmpty(fdate))
                    {
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
                    string url = string.Format("AuditTrail/GetAuditTrail/{0}/{1}/{2}", fDate, tDate, keyword);
                    apiUrl += url;

                    HttpResponseMessage Res = await client.GetAsync(apiUrl);

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        audit = root["result"].ToObject<List<AuditViewModel>>();
                    }
                    //ExportToExcel(audit);
                    if (audit.Count != 0)
                    {
                        var firstOrDefaultRec = audit.FirstOrDefault();
                        var properties = firstOrDefaultRec.GetType().GetProperties();
                        foreach (var prop in properties)
                        {
                            csvBuilder.Append(prop.Name + ",");
                        }
                        csvBuilder.AppendLine("");//Add line break

                        foreach (var item in audit)
                        {
                            item.Message = item.Message.Replace(",", ";");
                            string line = string.Join(",", properties.Select(p => p.GetValue(item, null)).ToArray());
                            csvBuilder.AppendLine(line);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occure.{ex.Message}");
                throw ex;
            }
            return File(System.Text.Encoding.ASCII.GetBytes(csvBuilder.ToString()), "text/csv", "Audit.csv");

        }
        private void ExportToExcel(List<AuditViewModel> products)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("AuditTrail");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Controller Name";
                worksheet.Cell(currentRow, 2).Value = "Date Time";
                worksheet.Cell(currentRow, 3).Value = "Description";
                worksheet.Cell(currentRow, 4).Value = "Message";
                worksheet.Cell(currentRow, 5).Value = "User Email";
                for (int i = 0; i < products.Count; i++)
                {
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = products[i].ModuleName;
                        worksheet.Cell(currentRow, 2).Value = products[i].CreatedDatetime;
                        worksheet.Cell(currentRow, 3).Value = products[i].Description;
                        worksheet.Cell(currentRow, 4).Value = products[i].Message;
                        worksheet.Cell(currentRow, 4).Value = products[i].UserId;
                    }
                }
                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                Response.Clear();
                Response.Headers.Add("content-disposition", "attachment;filename=ProductDetails.xls");
                Response.ContentType = "application/xls";
                Response.Body.WriteAsync(content);
                Response.Body.Flush();
            }
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
