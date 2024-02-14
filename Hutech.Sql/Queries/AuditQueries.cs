using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hutech.Sql.Queries
{
    public class AuditQueries
    {
        public static string GetAuditTrail => " Select AuditId,ModuleName,Request_Data,Description,CreatedDatetime,a.UserName as UserId  " +
           "from Audit au left join AspNetUsers a on au.UserID=a.Id where cast(au.CreatedDatetime as date)>=CONVERT(datetime,@fromDate) and cast(au.CreatedDatetime as date)<=CONVERT(datetime,@toDate)AND((@keyword = '' AND 1 = 1) OR((@keyword != '' AND au.ModuleName like + '%' +@keyword + '%') or(@keyword != '' AND au.Description like + '%' + @keyword + '%')or(@keyword != '' AND au.Request_Data like + '%' + @keyword + '%')or(@keyword != '' AND a.UserName like + '%' + @keyword + '%')))order by au.auditId desc";
        public static string GetUserAuditTrail => " Select AuditId,ModuleName,Request_Data,Description,CreatedDatetime,a.UserName as UserId  " +
            "from Audit au left join AspNetUsers a on au.UserID=a.Id where au.UserID=@userId and cast(au.CreatedDatetime as date)>=CONVERT(datetime,@fromDate) and cast(au.CreatedDatetime as date)<=CONVERT(datetime,@toDate)AND((@keyword = '' AND 1 = 1) OR((@keyword != '' AND au.ModuleName like + '%' +@keyword + '%') or(@keyword != '' AND au.Description like + '%' + @keyword + '%')or(@keyword != '' AND au.Request_Data like + '%' + @keyword + '%')or(@keyword != '' AND a.UserName like + '%' + @keyword + '%')))order by au.auditId desc";
        public static string AddExceptionDetails => "Update Audit set Exception_Details=@Exception_Details where AuditId=@AuditId";
    }
}
