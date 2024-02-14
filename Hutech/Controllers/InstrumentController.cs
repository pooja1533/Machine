﻿using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Hutech.Models;
using Hutech.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace Hutech.Controllers
{
    [Authorize]
    public class InstrumentController : Controller
    {
        public IConfiguration configuration { get; set; }
        private readonly ILogger<InstrumentController> logger;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnv;
        private readonly LanguageService languageService;
        public InstrumentController(IConfiguration _configuration, ILogger<InstrumentController> _logger, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, LanguageService _languageService)
        {
            configuration = _configuration;
            logger = _logger;
            hostingEnv = env;
            languageService = _languageService;
        }
        public async Task<IActionResult> GetAllInstrument(int pageNumber = 1)
        {
            try
            {
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                int totalRecords = 10;
                int totalPage = 0;
                List<InstrumentViewModel> instruments = new List<InstrumentViewModel>();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                    HttpResponseMessage Res = await client.GetAsync(string.Format("Instrument/GetInstrument/{0}",pageNumber));

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
                            instruments = root["result"].ToObject<List<InstrumentViewModel>>();
                            pageNumber = (int)root["currentPage"];
                            totalRecords = (int)root["totalRecords"];
                            totalPage = (int)root["totalPage"];
                        }
                    }
                }
                var data = new GridData<InstrumentViewModel>()
                {
                    CurrentPage = pageNumber,
                    GridRecords = instruments,
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
        }
        public IActionResult AddInstrument()
        {
            ViewBag.AllowedFileExtension = HttpContext.Session.GetString("FileType");
            ViewBag.AlloweFileSize = HttpContext.Session.GetString("FileSize");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddInstrument(InstrumentDocumentViewModel instrumentdocumentViewModel)
        {
            try
            {
                ViewBag.AllowedFileExtension = HttpContext.Session.GetString("FileType");
                ViewBag.AlloweFileSize = HttpContext.Session.GetString("FileSize");
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                var validation = new InstrumentValidator();
                var result = validation.Validate(instrumentdocumentViewModel);
                if (!result.IsValid)
                {
                    return View();
                }
                else
                {
                    string apiUrl = configuration["Baseurl"];
                    var instrument = new InstrumentViewModel()
                    {
                        Id = instrumentdocumentViewModel.Id,
                        Name = instrumentdocumentViewModel.Name,
                        IsActive = true,
                        IsDeleted = false
                    };
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        instrumentdocumentViewModel.IsDeleted = false;
                        var json = JsonConvert.SerializeObject(instrument);
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                        HttpResponseMessage Res = await client.PostAsync("Instrument/PostInstrument", stringContent);
                        if (Res.IsSuccessStatusCode)
                        {
                            var content = await Res.Content.ReadAsStringAsync();
                            JObject rootData = JObject.Parse(content);
                            if (instrumentdocumentViewModel.Files != null)
                            {
                                if (instrumentdocumentViewModel.Files.Count > 0)
                                {
                                    var FileDic = "Documents/Instrument/Image/";
                                    string FilePath = Path.Combine(hostingEnv.ContentRootPath, FileDic);

                                    var DocumentDic = "Documents/Instrument/Document";
                                    string FileDocumentPath = Path.Combine(hostingEnv.ContentRootPath, DocumentDic);

                                    if (!Directory.Exists(FilePath))
                                        Directory.CreateDirectory(FilePath);

                                    if (!Directory.Exists(FileDocumentPath))
                                        Directory.CreateDirectory(FileDocumentPath);

                                    if (instrumentdocumentViewModel.Files != null)
                                    {
                                        foreach (var data in instrumentdocumentViewModel.Files)
                                        {
                                            if (data.ContentType.Contains("image"))
                                            {
                                                var imgFilePath = Path.Combine(FilePath, data.FileName);
                                                using (FileStream fs = System.IO.File.Create(imgFilePath))
                                                {
                                                    data.CopyTo(fs);
                                                }
                                            }
                                            else
                                            {
                                                var imgFilePath = Path.Combine(FileDocumentPath, data.FileName);
                                                using (FileStream fs = System.IO.File.Create(imgFilePath))
                                                {
                                                    data.CopyTo(fs);
                                                }
                                            }
                                        }
                                    }
                                    var formData = new Dictionary<string, string>()
                                    {
                            { "IsActive", instrumentdocumentViewModel.IsActive.ToString()},
                            { "IsDelete", instrumentdocumentViewModel.IsDeleted.ToString()},
                            { "Name", instrumentdocumentViewModel.Name},
                        };
                                    var formContent = new MultipartFormDataContent();

                                    foreach (var keyValuePair in formData)
                                    {
                                        formContent.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                                    }

                                    foreach (var file in instrumentdocumentViewModel.Files)
                                    {
                                        formContent.Add(new StreamContent(file.OpenReadStream())
                                        {
                                            Headers = { ContentLength = file.Length, ContentType = new MediaTypeHeaderValue(file.ContentType) }
                                        },
                                        "Files",
                                        file.FileName);
                                    }
                                    HttpResponseMessage response = await client.PostAsync("Instrument/UploadInstrumentDocument/", formContent);
                                    if (response.IsSuccessStatusCode)
                                    {
                                        var contentData = await response.Content.ReadAsStringAsync();
                                        JObject root = JObject.Parse(contentData);
                                    }
                                }
                            }
                            var resultData = rootData["success"].ToString();
                            if (resultData == "False" || resultData == "false")
                            {
                                var Id = rootData["auditId"].ToString();
                                TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                                TempData["RedirectURl"] = "/Instrument/AddAddInstrument/";
                            }
                            else
                            {
                                TempData["message"] = languageService.Getkey("Instrument Added Successfully");
                                TempData["RedirectURl"] = "/Instrument/GetAllInstrument/";
                            }
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
                ViewBag.AllowedFileExtension = HttpContext.Session.GetString("FileType");
                ViewBag.AlloweFileSize = HttpContext.Session.GetString("FileSize");
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                InstrumentViewModel instrumentViewModel = new InstrumentViewModel();
                string apiUrl = configuration["Baseurl"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.GetAsync(string.Format("Instrument/GetInstrumentDetail/{0}", id));

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        JObject root = JObject.Parse(content);
                        var resultData = root["success"].ToString();
                        if (resultData == "False" || resultData == "false")
                        {
                            var Id = root["auditId"].ToString();
                            TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                            TempData["RedirectURl"] = "/Instrument/AddInstrument/";
                        }
                        else
                        {
                            instrumentViewModel = root["result"].ToObject<InstrumentViewModel>();
                        }
                    }
                }
                InstrumentDocumentViewModel model = new InstrumentDocumentViewModel();
                model.Id = instrumentViewModel.Id;
                model.Name = instrumentViewModel.Name;
                model.IsActive = instrumentViewModel.IsActive;
                var documentData = (string[])null;
                var documentIdsData = (string[])null;
                if (!string.IsNullOrEmpty(instrumentViewModel.Path))
                {
                    var documents = instrumentViewModel.Path.Split(",");
                    documentData = documents;
                }
                if (!string.IsNullOrEmpty(instrumentViewModel.DocumentId))
                {
                    var documentIds = instrumentViewModel.DocumentId.Split(",");
                    documentIdsData = documentIds;
                }
                if (documentData != null)
                {
                    for (int i = 0; i < documentData.Length; i++)
                    {
                        var filePath = documentData[i];
                        var fileName = documentData[i].Split("/").Last();
                        FileViewModel file = new FileViewModel();
                        file.FileName = fileName;
                        file.FilePath = filePath;
                        file.Id = System.Convert.ToInt64(documentIdsData[i]);
                        model.UplodedFile.Add(file);
                    }
                }

                //foreach (var item in documents)
                //{
                //    var fileName = item.Split("/").Last();
                //    FileViewModel file = new FileViewModel();
                //    file.FileName=fileName;
                //    model.UplodedFile.Add(file);
                //}
                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Exception Occure.{ex.Message}");
                throw ex;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInstrument(InstrumentDocumentViewModel instrumentDocumentViewModel)
        {
            try
            {
                ViewBag.AllowedFileExtension = HttpContext.Session.GetString("FileType");
                ViewBag.AlloweFileSize = HttpContext.Session.GetString("FileSize");
                var token = Request.Cookies["jwtCookie"];
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();

                    token = token.Replace("Bearer ", "");
                }
                var validation = new InstrumentValidator();
                var result = validation.Validate(instrumentDocumentViewModel);
                if (!result.IsValid)
                {
                    return View();
                }
                else
                {
                    var instrument = new InstrumentViewModel()
                    {
                        Id = instrumentDocumentViewModel.Id,
                        Name = instrumentDocumentViewModel.Name,
                        IsActive = instrumentDocumentViewModel.IsActive,
                        IsDeleted = false
                    };
                    string apiUrl = configuration["Baseurl"];
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(apiUrl);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var json = JsonConvert.SerializeObject(instrument);
                        var stringcontenet = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        HttpResponseMessage response = await client.PutAsync("Instrument/PutInstrument", stringcontenet);

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            JObject root = JObject.Parse(content);
                            //instrumentViewModel = root["result"].ToObject<InstrumentDocumentViewModel>();
                            if (instrumentDocumentViewModel.Files != null)
                            {
                                if (instrumentDocumentViewModel.Files.Count > 0)
                                {
                                    //HttpResponseMessage DeleteExistingInstrumentDocument = await client.DeleteAsync(string.Format("Instrument/DeleteExistingInstrumentDocument/{0}", instrumentDocumentViewModel.Id));


                                    var FileDic = "Documents/Instrument/Image/";
                                    string FilePath = Path.Combine(hostingEnv.ContentRootPath, FileDic);

                                    var DocumentDic = "Documents/Instrument/Document";
                                    string FileDocumentPath = Path.Combine(hostingEnv.ContentRootPath, DocumentDic);

                                    if (!Directory.Exists(FilePath))
                                        Directory.CreateDirectory(FilePath);

                                    if (!Directory.Exists(FileDocumentPath))
                                        Directory.CreateDirectory(FileDocumentPath);

                                    if (instrumentDocumentViewModel.Files != null)
                                    {
                                        foreach (var data in instrumentDocumentViewModel.Files)
                                        {
                                            if (data.ContentType.Contains("image"))
                                            {
                                                var imgFilePath = Path.Combine(FilePath, data.FileName);
                                                using (FileStream fs = System.IO.File.Create(imgFilePath))
                                                {
                                                    data.CopyTo(fs);
                                                }
                                            }
                                            else
                                            {
                                                var imgFilePath = Path.Combine(FileDocumentPath, data.FileName);
                                                using (FileStream fs = System.IO.File.Create(imgFilePath))
                                                {
                                                    data.CopyTo(fs);
                                                }
                                            }
                                        }
                                    }
                                    var formData = new Dictionary<string, string>()
                                    {
                            { "IsActive", instrumentDocumentViewModel.IsActive.ToString()},
                            { "IsDelete", instrumentDocumentViewModel.IsDeleted.ToString()},
                            { "Name", instrumentDocumentViewModel.Name},
                            { "Id",instrumentDocumentViewModel.Id.ToString()}
                        };
                                    var formContent = new MultipartFormDataContent();

                                    foreach (var keyValuePair in formData)
                                    {
                                        formContent.Add(new StringContent(keyValuePair.Value), keyValuePair.Key);
                                    }

                                    foreach (var file in instrumentDocumentViewModel.Files)
                                    {
                                        formContent.Add(new StreamContent(file.OpenReadStream())
                                        {
                                            Headers = { ContentLength = file.Length, ContentType = new MediaTypeHeaderValue(file.ContentType) }
                                        },
                                        "Files",
                                        file.FileName);
                                    }
                                    HttpResponseMessage responseData = await client.PostAsync("Instrument/UploadInstrumentDocument/", formContent);
                                    if (response.IsSuccessStatusCode)
                                    {
                                        var contentData = await responseData.Content.ReadAsStringAsync();
                                        JObject rootData = JObject.Parse(contentData);
                                    }
                                }
                            }
                            var resultData = root["success"].ToString();
                            if (resultData == "False" || resultData == "false")
                            {
                                var Id = root["auditId"].ToString();
                                TempData["message"] = languageService.Getkey("Something went wrong.Please contact Admin with AuditId:- ") + Id;
                                TempData["RedirectURl"] = "/Instrument/EditInstrument/";
                                return RedirectToAction("EditInstrument", new { id = instrumentDocumentViewModel.Id });

                            }
                            else
                            {
                                TempData["message"] = languageService.Getkey("Instrument Updated Successfully");
                                TempData["RedirectURl"] = "/Instrument/GetAllInstrument/";
                            }
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
        public async Task<IActionResult> DeleteDocument(long documentId, long instrumentId)
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
                    HttpResponseMessage response = await client.DeleteAsync(string.Format("Instrument/DeleteDocument/{0}/{1}", documentId, instrumentId));

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
                            TempData["message"] = languageService.Getkey("Document Deleted Successfully");
                        }
                        //var message = root["value"].ToString();
                    }
                }
                return RedirectToAction("EditInstrument", "Instrument", new { @id = instrumentId });
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
                    HttpResponseMessage response = await client.DeleteAsync(string.Format("Instrument/DeleteInstrument/{0}", id));

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
                            TempData["message"] = languageService.Getkey("Instrument Deleted Successfully");
                            TempData["RedirectURl"] = "/Instrument/GetAllInstrument/";
                        }
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
        public ActionResult Download(string fileName)
        {
            string path = string.Empty;
            var name = fileName.Split("/").Last();
            if (fileName.Contains("Image"))
            {
                path = Path.Combine(hostingEnv.ContentRootPath, "Documents/Instrument/Image/" + name);
            }
            else
            {
                path = Path.Combine(hostingEnv.ContentRootPath, "Documents/Instrument/Document/" + name);

            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, name);
        }
    }
}
