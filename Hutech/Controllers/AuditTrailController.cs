using ClosedXML.Excel;
using Hutech.Core.Constants;
using Hutech.Models;
using Irony.Ast;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using System.Composition;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using iText.IO.Source;
using iText.Kernel.Pdf;
using iText.Kernel.Geom;
using iText.Html2pdf;
using System.Collections;
using NuGet.Packaging;
using Microsoft.AspNetCore.Http;
using DocumentFormat.OpenXml.Bibliography;
using iText.Layout.Element;
using iText.Layout.Properties;
using Table = iText.Layout.Element.Table;
using iText.Layout;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using DocumentFormat.OpenXml.Wordprocessing;
using Document = iText.Layout.Document;
using Paragraph = iText.Layout.Element.Paragraph;
using TextAlignment = iText.Layout.Properties.TextAlignment;
using Hutech.Services;
using Color = iText.Kernel.Colors.Color;


namespace Hutech.Controllers
{
    public class AuditTrailController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<AuditTrailController> logger;
        private readonly LanguageService languageService;
        public AuditTrailController(IConfiguration _configuration, ILogger<AuditTrailController> _logger, LanguageService _languageService)
        {
            configuration = _configuration;
            logger = _logger;
            languageService = _languageService;
        }
        public async Task<IActionResult> ExportAuditTrail(string? fdate, string? tdate, string? keyword, int pageNumber = 0)
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
                var role = HttpContext.Session.GetString("UserRole");
                var userId = HttpContext.Session.GetString("UserId");
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
                    string url = string.Format("AuditTrail/GetAuditTrail/{0}/{1}/{2}/{3}/{4}/{5}", fDate, tDate, keyword, pageNumber, role, userId);
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
                            item.Request_Data = item.Request_Data.Replace(",", ";");
                            item.role = item.role.Replace(","," / ");
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
                        worksheet.Cell(currentRow, 4).Value = products[i].Request_Data;
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


        public async Task<IActionResult> ExportAuditTrailPDFFormat(string? fdate, string? tdate, string? keyword)
        {
            DateTime endDate = DateTime.UtcNow;
            DateTime startDate = DateTime.UtcNow.AddDays(-7);
            var pdfStream = new MemoryStream();
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();
                    token = token.Replace("Bearer ", "");
                }
                var role = HttpContext.Session.GetString("UserRole");
                var userId = HttpContext.Session.GetString("UserId");
                string fDate = string.Empty;
                string tDate = string.Empty;
                List<AuditViewModel> allAudit = new List<AuditViewModel>();
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
                    tDate = endDate.ToShortDateString();
                    tDate = tDate.Replace("/", "-");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    //Get organization name
                    List<OrganizationViewModel> organizations = new List<OrganizationViewModel>();

                    HttpResponseMessage ResOfOrganization = await client.GetAsync(string.Format("Organization/GetOrganization/{0}", 0));

                    if (ResOfOrganization.IsSuccessStatusCode)
                    {
                        var content = await ResOfOrganization.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var Id = root["auditId"].ToString();
                            string message = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                            TempData["message"] = message;
                        }
                        else
                        {
                            organizations = root["result"].ToObject<List<OrganizationViewModel>>();
                        }
                    }
                    string organizationName = string.Empty;
                    organizationName = "              " +organizations.FirstOrDefault().Name;
                    //end organization name

                    string url = string.Format("AuditTrail/GetAuditTrail/{0}/{1}/{2}/{3}/{4}/{5}", fDate, tDate, keyword, 0, role, userId);
                    apiUrl += url;

                    HttpResponseMessage Res = await client.GetAsync(apiUrl);

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        allAudit = root["result"].ToObject<List<AuditViewModel>>();
                    }

                    // Building PDF
                    using (var pdfWriter = new PdfWriter(pdfStream))
                    {
                        using (var pdfDocument = new PdfDocument(pdfWriter))
                        {
                            pdfDocument.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A2);

                            var document = new Document(pdfDocument);
                            // Divide allAudit into pages with 10 records each
                            var pages = (int)Math.Ceiling((double)allAudit.Count / 20);
                            var loggedinUserEmail = HttpContext.Session.GetString("LoogedInUser");
                            var loggedinUserRole = HttpContext.Session.GetString("UserRole");
                            var currentDatetime = DateTime.UtcNow;
                            int pageNumber = 1;
                            for (int page = 0; page < pages; page++)
                            {
                                // Define header
                                document.SetMargins(36, 36, 80, 36); // Left, Right, Top, Bottom
                                var header = new Paragraph("Report: Audit Trail User Management, Printed By: " + loggedinUserEmail + ", Role: " + loggedinUserRole + ", DateTime: " + currentDatetime)
                                    .SetTextAlignment(TextAlignment.CENTER)
                                    .SetFontSize(12)
                                    .SetBold()
                                    .SetFontColor(ColorConstants.BLACK);
                                document.Add(header);

                                // Table
                                var table = new Table(UnitValue.CreatePercentArray(new float[] { 10, 15, 15, 15, 15, 20, 10, 10 }));
                                table.SetWidth(UnitValue.CreatePercentValue(100));

                                // Header Row
                                Color color = new DeviceRgb(0, 0, 205); // Adjust the RGB values as needed for your desired shade

                                var auditIdHeaderCell = new Cell().Add(new Paragraph("AuditID"));
                                auditIdHeaderCell.SetBackgroundColor(color);
                                auditIdHeaderCell.SetBold();
                                auditIdHeaderCell.SetFontColor(ColorConstants.WHITE);
                                table.AddHeaderCell(auditIdHeaderCell);
                                //table.AddHeaderCell("AuditID").SetBackgroundColor(ColorConstants.CYAN);
                                var auditIdHeaderCellModeuleName = new Cell().Add(new Paragraph("Module Name"));
                                auditIdHeaderCellModeuleName.SetBackgroundColor(color);
                                auditIdHeaderCellModeuleName.SetBold();
                                auditIdHeaderCellModeuleName.SetFontColor(ColorConstants.WHITE);    
                                table.AddHeaderCell(auditIdHeaderCellModeuleName);
                                //table.AddHeaderCell("Module Name").SetBackgroundColor(ColorConstants.CYAN);
                                var auditIdHeaderCellrole = new Cell().Add(new Paragraph("Role"));
                                auditIdHeaderCellrole.SetBackgroundColor(color);
                                auditIdHeaderCellrole.SetFontColor(ColorConstants.WHITE);
                                auditIdHeaderCellrole.SetBold();
                                table.AddHeaderCell(auditIdHeaderCellrole);
                                //table.AddHeaderCell("Role").SetBackgroundColor(ColorConstants.CYAN);
                                var auditIdHeaderCellipAddress = new Cell().Add(new Paragraph("IP Address"));
                                auditIdHeaderCellipAddress.SetBackgroundColor(color);
                                auditIdHeaderCellipAddress.SetBold();
                                auditIdHeaderCellipAddress.SetFontColor(ColorConstants.WHITE);
                                table.AddHeaderCell(auditIdHeaderCellipAddress);
                                //table.AddHeaderCell("IP Address").SetBackgroundColor(ColorConstants.CYAN);
                                var auditIdHeaderCelluserId = new Cell().Add(new Paragraph("UserId"));
                                auditIdHeaderCelluserId.SetBackgroundColor(color);
                                auditIdHeaderCelluserId.SetBold();
                                auditIdHeaderCelluserId.SetFontColor(ColorConstants.WHITE);
                                table.AddHeaderCell(auditIdHeaderCelluserId);
                                //table.AddHeaderCell("UserId").SetBackgroundColor(ColorConstants.CYAN);
                                var auditIdHeaderCellDescription = new Cell().Add(new Paragraph("Description"));
                                auditIdHeaderCellDescription.SetBackgroundColor(color);
                                auditIdHeaderCellDescription.SetFontColor(ColorConstants.WHITE);
                                auditIdHeaderCellDescription.SetBold();
                                table.AddHeaderCell(auditIdHeaderCellDescription);
                                //table.AddHeaderCell("Description").SetBackgroundColor(ColorConstants.CYAN);
                                var auditIdHeaderCellcreatedDate = new Cell().Add(new Paragraph("Created Date"));
                                auditIdHeaderCellcreatedDate.SetBackgroundColor(color);
                                auditIdHeaderCellcreatedDate.SetFontColor(ColorConstants.WHITE);
                                auditIdHeaderCellcreatedDate.SetBold();
                                table.AddHeaderCell(auditIdHeaderCellcreatedDate);
                                //table.AddHeaderCell("Created Date").SetBackgroundColor(ColorConstants.CYAN);
                                var auditIdHeaderCellRequestData = new Cell().Add(new Paragraph("Request Data"));
                                auditIdHeaderCellRequestData.SetBackgroundColor(color);
                                auditIdHeaderCellRequestData.SetFontColor(ColorConstants.WHITE);
                                auditIdHeaderCellRequestData.SetBold();
                                table.AddHeaderCell(auditIdHeaderCellRequestData);
                                //table.AddHeaderCell("Request Data").SetBackgroundColor(ColorConstants.CYAN);

                                // Data Rows
                                int startIndex = page * 20;
                                int endIndex = Math.Min(startIndex + 20, allAudit.Count);
                                for (int i = startIndex; i < endIndex; i++)
                                {
                                    var model = allAudit[i];
                                    table.AddCell(model.AuditId.ToString());
                                    table.AddCell(model.ModuleName);
                                    if (string.IsNullOrEmpty(model.role))
                                        table.AddCell(string.Empty);
                                    else
                                        table.AddCell(model.role);
                                    if (string.IsNullOrEmpty(model.IPAddress))
                                        table.AddCell(string.Empty);
                                    else
                                        table.AddCell(model.IPAddress);
                                    table.AddCell(model.UserId);
                                    table.AddCell(model.Description);
                                    table.AddCell(model.CreatedDatetime.ToString());
                                    table.AddCell(model.Request_Data);
                                }

                                document.Add(table);
                                var footer = new Paragraph(organizationName + "                   Page:  " + pageNumber)
                                    .SetTextAlignment(TextAlignment.RIGHT)
                                   .SetFontSize(12)
                                   .SetBold()
                                   .SetFontColor(ColorConstants.BLACK); ;
                                document.Add(footer);
                                // Add a new page if there are more records
                                if (page < pages - 1)
                                {
                                    document.Add(new AreaBreak());
                                    pageNumber++;
                                }
                            }
                        }
                    }
                }
                return File(pdfStream.ToArray(), "application/pdf", "Audit.pdf");
            }
            catch (Exception ex)
            {
                logger.LogInformation($"SQL Exception Occurred. {ex.Message}");
                throw ex;
            }
        }
        //public async Task<IActionResult> ExportAuditTrailPDFFormat(string? fdate, string? tdate, string? keyword, int pageNumber = 0)
        //{
        //    DateTime endDate = DateTime.UtcNow;
        //    DateTime startDate = DateTime.UtcNow.AddDays(-7);
        //    var csvBuilder = new StringBuilder();
        //    try
        //    {
        //        var token = Request.Cookies["jwtCookie"];
        //        if (!string.IsNullOrEmpty(token))
        //        {
        //            var handler = new JwtSecurityTokenHandler();

        //            token = token.Replace("Bearer ", "");
        //        }
        //        var role = HttpContext.Session.GetString("UserRole");
        //        var userId = HttpContext.Session.GetString("UserId");
        //        string fDate = string.Empty;
        //        string tDate = string.Empty;
        //        List<AuditViewModel> audit = new List<AuditViewModel>();
        //        string apiUrl = configuration["Baseurl"];
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(apiUrl);
        //            client.DefaultRequestHeaders.Clear();
        //            if (string.IsNullOrEmpty(fdate))
        //            {
        //                fDate = startDate.ToString("yyyy-MM-dd");
        //                fDate = fDate.Replace("/", "-");
        //            }
        //            else
        //            {
        //                fDate = fdate.Replace("/", "-");
        //            }
        //            if (string.IsNullOrEmpty(tdate))
        //            {
        //                tDate = endDate.ToString("yyyy-MM-dd");
        //                tDate = tDate.Replace("/", "-");
        //            }
        //            else
        //            {
        //                tDate = tdate.Replace("/", "-");
        //            }
        //            if (string.IsNullOrEmpty(keyword))
        //                keyword = "null";
        //            tDate = endDate.ToShortDateString();
        //            tDate = tDate.Replace("/", "-");
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //            string url = string.Format("AuditTrail/GetAuditTrail/{0}/{1}/{2}/{3}/{4}/{5}", fDate, tDate, keyword, pageNumber, role, userId);
        //            apiUrl += url;

        //            HttpResponseMessage Res = await client.GetAsync(apiUrl);

        //            if (Res.IsSuccessStatusCode)
        //            {
        //                var content = await Res.Content.ReadAsStringAsync();
        //                JObject root = JObject.Parse(content);
        //                audit = root["result"].ToObject<List<AuditViewModel>>();
        //            }
        //            Building an HTML string.
        //            StringBuilder sb = new StringBuilder();
        //            var loggedinUserEmail = HttpContext.Session.GetString("LoogedInUser");
        //            var loggedinUserRole = HttpContext.Session.GetString("UserRole");
        //            var currentDatetime = DateTime.UtcNow;
        //            string header = "<h2 style='text-align:center;Font-weight:bold;'>Report :- Audit Trail User Management,Printed By:- " + loggedinUserEmail + ",Role:- " + loggedinUserRole + ",DateTime:- " + currentDatetime + "</h2>";
        //            sb.Append(header);
        //            Table start.
        //            sb.Append("<table border='1' cellpadding='5' cellspacing='0' style='border: 1px solid #ccc;font-family: Arial; font-size: 10pt;'>");

        //            Building the Header row.
        //            sb.Append("<tr>");
        //            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>AuditID</th>");
        //            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Module Name</th>");
        //            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Role</th>");
        //            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>IP Address</th>");
        //            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>UserId</th>");
        //            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Description</th>");
        //            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Created Date</th>");
        //            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Request_Data</th>");
        //            sb.Append("</tr>");

        //            Building the Data rows.
        //            for (int i = 0; i < audit.Count; i++)
        //            {
        //                AuditViewModel model = new AuditViewModel();
        //                model = audit[i];
        //                string[] auditData = new string[8];
        //                auditData[0] = model.AuditId.ToString();
        //                auditData[1] = model.ModuleName;
        //                auditData[2] = model.role;
        //                auditData[3] = model.IPAddress;
        //                auditData[4] = model.UserId;
        //                auditData[5] = model.Description;
        //                auditData[6] = model.CreatedDatetime.ToString();
        //                auditData[7] = model.Request_Data;
        //                for (int j = 0; j < auditData.Length; j++)
        //                {
        //                    Append data.
        //                    sb.Append("<td style='border: 1px solid #ccc'>");
        //                    sb.Append(auditData[j]);
        //                    sb.Append("</td>");
        //                }
        //                sb.Append("</tr>");
        //            }

        //            Table end.
        //            sb.Append("</table>");
        //            using (MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(sb.ToString())))
        //            {
        //                ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
        //                PdfWriter writer = new PdfWriter(byteArrayOutputStream);
        //                PdfDocument pdfDocument = new PdfDocument(writer);
        //                pdfDocument.SetDefaultPageSize(PageSize.A1);

        //                HtmlConverter.ConvertToPdf(stream, pdfDocument);
        //                pdfDocument.Close();
        //                return File(byteArrayOutputStream.ToArray(), "application/pdf", "Audit.pdf");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogInformation($"SQL Exception Occure.{ex.Message}");
        //        throw ex;
        //    }
        //}
        public async Task<IActionResult> GetAllAuditTrail(string? fdate, string? tdate, string? keyword, int pageNumber = 1)
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
                var role = HttpContext.Session.GetString("UserRole");
                var userId = HttpContext.Session.GetString("UserId");
                string fDate = string.Empty;
                string tDate = string.Empty;
                List<AuditViewModel> audit = new List<AuditViewModel>();
                int totalRecords = 0;
                int totalPage = 0;
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
                    string url = string.Format("AuditTrail/GetAuditTrail/{0}/{1}/{2}/{3}/{4}/{5}", fDate, tDate, keyword, pageNumber, role, userId);
                    apiUrl += url;

                    HttpResponseMessage Res = await client.GetAsync(apiUrl);

                    if (Res.IsSuccessStatusCode)
                    {
                        var content = await Res.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        audit = root["result"].ToObject<List<AuditViewModel>>();
                        pageNumber = (int)root["currentPage"];
                        totalRecords = (int)root["totalRecords"];
                        totalPage = (int)root["totalPage"];
                    }
                }
                ViewBag.StartDate = fDate;
                ViewBag.EndDate = tDate;
                if (keyword == "null")
                {
                    keyword = "";
                }
                ViewBag.Keyword = keyword;
                var data = new GridData<AuditViewModel>()
                {
                    CurrentPage = pageNumber,
                    GridRecords = audit,
                    TotalPages = totalPage,
                    TotalRecords = totalRecords
                };
                return View(data);
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
