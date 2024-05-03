﻿using DocumentFormat.OpenXml.Wordprocessing;
using Hutech.API.Helpers;
using Hutech.Application.Interfaces;
using Hutech.Core.Entities;
using Hutech.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Nancy.Json;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using static Dapper.SqlMapper;

namespace Hutech.API
{
    public class AuditFilterAttribute : ActionFilterAttribute
    {
        private readonly IAuditRepository _auditRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuditFilterAttribute(IHttpContextAccessor httpContextAccessor, IAuditRepository auditRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _auditRepository = auditRepository;
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!(filterContext.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor))
            {
                return;
            }

            if (controllerActionDescriptor.MethodInfo.GetCustomAttributes(typeof(SkipAuditFilterAttribute), false).Any())
            {
                return;
            }

            var actionArguments = filterContext.ActionDescriptor.Parameters;
            //if (data.Count > 0)
            //{
            //    var innerData = data.FirstOrDefault();
            //    var serializer = new JavaScriptSerializer();
            //    message = serializer.Serialize(innerData);
            //}
            var data= filterContext.HttpContext.Items["ActionMethodParameters"];
            string message = string.Empty;
            message=data!=null? data.ToString() : string.Empty;
            var objaudit = new AuditModels(); // Getting Action Name 
            var controllerName = filterContext.ActionDescriptor.RouteValues["controller"];
            var actionName = filterContext.ActionDescriptor.RouteValues["action"];
            var actionDescriptorRouteValues = filterContext.ActionDescriptor.RouteValues;

            if (actionDescriptorRouteValues.ContainsKey("area"))
            {
                var area = actionDescriptorRouteValues["area"];
                if (area != null)
                {
                    objaudit.Area = Convert.ToString(area);
                }
            }

            var request = filterContext.HttpContext.Request;
            objaudit.LangId = "";
            var s = filterContext.HttpContext.Request.Headers["Authorization"];
            if (AuthenticationHeaderValue.TryParse(s, out var headerValue))
            {
                var stream = headerValue.Parameter;
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(stream);
                var tokenS = handler.ReadToken(stream) as JwtSecurityToken;
                objaudit.UserId = filterContext.HttpContext.Request.Headers["User_Id"] = tokenS.Claims.FirstOrDefault(a => a.Type == "User_Id")?.Value.ToString();
                filterContext.HttpContext.Session.SetString("UserId", objaudit.UserId);
            }
            objaudit.UserId = filterContext.HttpContext.Session.GetString("UserId");
            objaudit.RoleId = "";
            objaudit.IsFirstLogin = "";
            objaudit.PageAccessed = Convert.ToString(filterContext.HttpContext.Request.Path); // URL User Requested 
            objaudit.LoggedInAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            objaudit.IpAddress = Convert.ToString(filterContext.HttpContext.Connection.RemoteIpAddress);
            objaudit.ModuleName = controllerName; // ControllerName 
            objaudit.ActionName = actionName;
            objaudit.Request_Data = message;
            objaudit.Exception_Details = "";
            RequestHeaders header = request.GetTypedHeaders();
            Uri uriReferer = header.Referer;

            if (uriReferer != null)
            {
                objaudit.UrlReferrer = header.Referer.AbsoluteUri;
            }

            long Id = _auditRepository.InsertAuditLogs(objaudit);
            filterContext.RouteData.Values.Add("AuditId", Id);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if ((filterContext.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(typeof(SkipAuditFilterAttribute), false).Any())
            {
                return;
            }
            else
            {


                //filterContext.ActionArguments.TryGetValue("model", out object model);
                var data = filterContext.ActionArguments;
                string message = string.Empty;
                if (data.Count > 0)
                {
                    var innerData = data.FirstOrDefault();
                    var keyData = innerData.Key;
                    var valueData = innerData.Value;
                    var serializer = new JavaScriptSerializer();
                    message = serializer.Serialize(data);
                    filterContext.HttpContext.Items["ActionMethodParameters"] = message;
                }
                //var objaudit = new AuditModels(); // Getting Action Name 
                //var controllerName = ((ControllerBase)filterContext.Controller)
                //    .ControllerContext.ActionDescriptor.ControllerName;
                //var actionName = ((ControllerBase)filterContext.Controller)
                //    .ControllerContext.ActionDescriptor.ActionName;
                //var actionDescriptorRouteValues = ((ControllerBase)filterContext.Controller)
                //    .ControllerContext.ActionDescriptor.RouteValues;

                //if (actionDescriptorRouteValues.ContainsKey("area"))
                //{
                //    var area = actionDescriptorRouteValues["area"];
                //    if (area != null)
                //    {
                //        objaudit.Area = Convert.ToString(area);
                //    }
                //}

                //var request = filterContext.HttpContext.Request;
                //objaudit.LangId = "";
                //var s = filterContext.HttpContext.Request.Headers["Authorization"];
                //if (AuthenticationHeaderValue.TryParse(s, out var headerValue))
                //{
                //    // we have a valid AuthenticationHeaderValue that has the following details:

                //    var scheme = headerValue.Scheme;
                //    var parameter = headerValue.Parameter;

                //    // scheme will be "Bearer"
                //    // parmameter will be the token itself.
                //    // or
                //    var stream = parameter;
                //    var handler = new JwtSecurityTokenHandler();
                //    var jsonToken = handler.ReadToken(stream);
                //    var tokenS = handler.ReadToken(stream) as JwtSecurityToken;
                //    objaudit.UserId = filterContext.HttpContext.Request.Headers["User_Id"] = tokenS.Claims.FirstOrDefault(a => a.Type == "User_Id")?.Value.ToString();
                //    filterContext.HttpContext.Session.SetString("UserId", objaudit.UserId);
                //}
                //objaudit.UserId = filterContext.HttpContext.Session.GetString("UserId");
                //objaudit.RoleId = "";
                //objaudit.IsFirstLogin = "";
                //objaudit.PageAccessed = Convert.ToString(filterContext.HttpContext.Request.Path); // URL User Requested 
                //objaudit.LoggedInAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                //objaudit.IpAddress = Convert.ToString(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
                //objaudit.ModuleName = controllerName; // ControllerName 
                //objaudit.ActionName = actionName;
                //objaudit.Request_Data = message;
                //objaudit.Exception_Details = "";
                //RequestHeaders header = request.GetTypedHeaders();
                //Uri uriReferer = header.Referer;

                //if (uriReferer != null)
                //{
                //    objaudit.UrlReferrer = header.Referer.AbsoluteUri;
                //}

                //long Id = _auditRepository.InsertAuditLogs(objaudit);
                //filterContext.RouteData.Values.Add("AuditId", Id);
            }
        }
    }
    public class SkipAuditFilterAttribute : Attribute
    {

    }
}

