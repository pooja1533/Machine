using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hutech.Sql.Queries
{
    public class LocationQueries
    {
        public static string PostLocation => "Insert into Location values(@Name,@isActive,@IsDeleted,@DatecreatedUtc,@CreatedByUserId,@DateModifiedUtc,@ModifiedByUserId)";
        public static string GetActiveLocation => "Select * from Location where IsDeleted=0 and IsActive=1";
        public static string GetLocation => "Select l.*,ud.FirstName+' '+ud.LastName as fullname,(select STRING_AGG(r.name,',' )  from AspNetRoles r join AspNetUserRoles ar on ar.RoleId=r.Id where ar.UserId=case when l.ModifiedByUserId is not null then l.ModifiedByUserId else l.CreatedByUserId end)as Role from Location l join UserDetail ud on ud.AspNetUserId= case when l.ModifiedByUserId is not null then l.ModifiedByUserId else l.CreatedByUserId end where l.IsDeleted=0  and l.IsActive=1";
        public static string GetLocationDetail => "Select * from Location where Id=@Id";

        public static string DeleteLocation => "update  Location set IsDeleted=1 where Id=@Id";
        public static string UpdateLocation => "update Location set Name=@Name,IsActive=@IsActive,ModifiedByUserId=@ModifiedByUserId,DateModifiedUtc=@DateModifiedUtc where Id=@Id";
        public static string GetFilterData => "Select * from Location where IsDeleted=0 and Name like case when @Name is not null then @Name else '%' end";
        public static string GetAllFilterLocation => "select * from (SELECT  l.*,ud.FirstName + ' ' + ud.LastName AS fullname,(SELECT STRING_AGG(r.name, ',') FROM AspNetRoles r " +
            " JOIN AspNetUserRoles ar ON ar.RoleId = r.Id WHERE ar.UserId = CASE WHEN l.ModifiedByUserId IS NOT NULL THEN l.ModifiedByUserId ELSE l.CreatedByUserId END) AS Role  FROM Location l JOIN UserDetail ud ON ud.AspNetUserId = CASE WHEN l.ModifiedByUserId IS NOT NULL THEN l.ModifiedByUserId ELSE l.CreatedByUserId END WHERE l.IsDeleted = 0 and l.IsActive=@Status AND Name LIKE CASE WHEN @Name IS NOT NULL THEN @Name ELSE '%' END" +
            " AND (@UpdatedDate IS NULL OR cast(l.DateModifiedUtc as date) =@UpdatedDate )" +
            " AND (ud.FirstName LIKE (CASE WHEN @UpdatedBy IS NOT NULL THEN @UpdatedBy ELSE '%' END) OR ud.LastName LIKE (CASE WHEN @UpdatedBy IS NOT NULL THEN @UpdatedBy ELSE '%'END)" +
            " or(select STRING_AGG(r.name,',') from AspNetRoles r join AspNetUserRoles ar on ar.RoleId=r.Id where ar.UserId=case when l.ModifiedByUserId is not null then l.ModifiedByUserId else l.CreatedByUserId end) like @UpdatedBy )) as subquery" ;
        //public static string GetAllFilterLocation => "Select l.*,ud.FirstName+' '+ud.LastName as fullname,(select STRING_AGG(r.name,',' )  from AspNetRoles r join AspNetUserRoles ar on ar.RoleId=r.Id where ar.UserId=case when l.ModifiedByUserId is not null then l.ModifiedByUserId else l.CreatedByUserId end)as Role from Location l join UserDetail ud on ud.AspNetUserId= case when l.ModifiedByUserId is not null then l.ModifiedByUserId else l.CreatedByUserId end where l.IsDeleted=0 and Name like case when @Name is not null then @Name else '%' end " +
        //    " and (ud.FirstName like(case when @UpdatedBy is not null then  @UpdatedBy else '%' end) or ud.LastName like (case when @UpdatedBy is not null then @UpdatedBy else '%' end))";
    }
}
