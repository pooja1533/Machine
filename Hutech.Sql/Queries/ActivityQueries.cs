using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Sql.Queries
{
    public class ActivityQueries
    {
        public static string PostActivity => "Insert into Activity values(@Name,@isActive,@IsDeleted,@DatecreatedUtc,@CreatedByUserId,@DateModifiedUtc,@ModifiedByUserId)";
        public static string GetActivity => "Select * from Activity where IsDeleted=0";
        public static string GetActiveActivity => "Select * from Activity where IsDeleted=0 and IsActive=1";
        public static string GetActivityDetail => "Select * from Activity where Id=@Id";

        public static string DeleteActivity => "update  Activity set IsDeleted=1 where Id=@Id";
        public static string UpdateActivity => "update Activity set Name=@Name,IsActive=@IsActive,DateModifiedUtc=@DateModifiedUtc,ModifiedByUserId=@ModifiedByUserId where Id=@Id";
        public static string GetAllFilterActivity => "select * from (SELECT  a.*,ud.FirstName + ' ' + ud.LastName AS fullname,(SELECT STRING_AGG(r.name, ',') FROM AspNetRoles r JOIN AspNetUserRoles ar ON ar.RoleId = r.Id WHERE ar.UserId = CASE WHEN a.ModifiedByUserId IS NOT NULL THEN a.ModifiedByUserId ELSE a.CreatedByUserId END) AS Role  FROM Activity a JOIN UserDetail ud ON ud.AspNetUserId = CASE WHEN a.ModifiedByUserId IS NOT NULL THEN a.ModifiedByUserId ELSE a.CreatedByUserId END WHERE a.IsDeleted = 0 and a.IsActive=@Status AND Name LIKE CASE WHEN @Name IS NOT NULL THEN @Name ELSE '%' END AND (@UpdatedDate IS NULL OR cast(a.DateModifiedUtc as date) =@UpdatedDate ) AND (ud.FirstName LIKE (CASE WHEN @UpdatedBy IS NOT NULL THEN @UpdatedBy ELSE '%' END) OR ud.LastName LIKE (CASE WHEN @UpdatedBy IS NOT NULL THEN @UpdatedBy ELSE '%'END) or(select STRING_AGG(r.name,',') from AspNetRoles r join AspNetUserRoles ar on ar.RoleId=r.Id where ar.UserId=case when a.ModifiedByUserId is not null then a.ModifiedByUserId else a.CreatedByUserId end) like @UpdatedBy )) as subquery order by subquery.Id desc";
    }
}
