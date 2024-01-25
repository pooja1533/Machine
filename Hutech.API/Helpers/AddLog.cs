using AutoMapper;
using Hutech.API.Model;
using Hutech.Application.Interfaces;
using Hutech.Core.Entities;
using Hutech.Infrastructure.Repository;
using Hutech.Models;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hutech.API.Helpers
{
    public  class AddLog
    {
        private readonly IAuditRepository auditRepository;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ISession session;
        public AddLog() { }
        public AddLog(IAuditRepository _auditRepository,IMapper _mapper,IHttpContextAccessor _httpContextAccessor,ISession _session)
        {
            auditRepository = _auditRepository; 
            mapper = _mapper;
            session = _session;
            httpContextAccessor = _httpContextAccessor;
        }
        public void AuditActivity(AuditModels data)
        {
//            var objaudit = new AuditModel();
//            //objaudit.RoleId = Convert.ToString(HttpContext.Session.GetInt32(AllSessionKeys.RoleId));
//            objaudit.ControllerName = "Portal";
//            objaudit.ActionName = "Login";
//            objaudit.Area = "";
//            objaudit.LoggedInAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
//            //if (_httpContextAccessor.HttpContext != null)
//            //    objaudit.IpAddress = Convert.ToString(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
//            objaudit.UserId = httpContextAccessor.HttpContext.Session.GetString("UserId");
//;            objaudit.PageAccessed = "";
//            objaudit.UrlReferrer = "";
//            //objaudit.SessionId = HttpContext.Session.Id;
//            var activitydata = mapper.Map<AuditModel, AuditModels>(objaudit);
            auditRepository.InsertAuditLogs(data);
        }

        public void InsertAuditLogs(AuditModels objauditmodel)
        {
            auditRepository.InsertAuditLogs(objauditmodel);
        }
    }
}
