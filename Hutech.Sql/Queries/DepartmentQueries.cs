using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Sql.Queries
{
    public class DepartmentQueries
    {
        public static string PostDepartment => "Insert into Department values(@Name,@isActive,@IsDeleted,@DatecreatedUtc,@CreatedByUserId,@DateModifiedUtc,@ModifiedByUserId)";
        public static string GetDepartment => "Select d.*,ud.FirstName+' '+ud.LastName as fullname,(select STRING_AGG(r.name,',')from AspNetRoles r join AspNetUserRoles ar on ar.RoleId=r.Id where ar.UserId= case when d.ModifiedByUserId is not null then d.ModifiedByUserId else d.CreatedByUserId end)as role from Department d  join UserDetail ud on ud.AspNetUserId= case when d.ModifiedByUserId is not null then d.ModifiedByUserId else d.CreatedByUserId end where d.IsDeleted=0 and d.IsActive=1";
        public static string GetActiveDepartment => "Select * from Department where IsDeleted=0 and IsActive=1";
        public static string GetDepartmentDetail => "Select * from Department where Id=@Id";

        public static string DeleteDepartment => "update  Department set IsDeleted=1 where Id=@Id";
        public static string UpdateDepartment => "update Department set Name=@Name,IsActive=@IsActive,DateModifiedUtc=@DateModifiedUtc,ModifiedByUserId=@ModifiedByUserId where Id=@Id";
        public static string GetAllFilterDepartment => "select * from (SELECT  d.*,ud.FirstName + ' ' + ud.LastName AS fullname,(SELECT STRING_AGG(r.name, ',') FROM AspNetRoles r JOIN AspNetUserRoles ar ON ar.RoleId = r.Id WHERE ar.UserId = CASE WHEN d.ModifiedByUserId IS NOT NULL THEN d.ModifiedByUserId ELSE d.CreatedByUserId END) AS Role  FROM Department d JOIN UserDetail ud ON ud.AspNetUserId = CASE WHEN d.ModifiedByUserId IS NOT NULL THEN d.ModifiedByUserId ELSE d.CreatedByUserId END WHERE d.IsDeleted = 0 and d.IsActive=@Status AND Name LIKE CASE WHEN @Name IS NOT NULL THEN @Name ELSE '%' END AND (@UpdatedDate IS NULL OR cast(d.DateModifiedUtc as date) =@UpdatedDate ) AND (ud.FirstName LIKE (CASE WHEN @UpdatedBy IS NOT NULL THEN @UpdatedBy ELSE '%' END) OR ud.LastName LIKE (CASE WHEN @UpdatedBy IS NOT NULL THEN @UpdatedBy ELSE '%'END) or(select STRING_AGG(r.name,',') from AspNetRoles r join AspNetUserRoles ar on ar.RoleId=r.Id where ar.UserId=case when d.ModifiedByUserId is not null then d.ModifiedByUserId else d.CreatedByUserId end) like @UpdatedBy )) as subquery order by subquery.Id desc";
    }
}
