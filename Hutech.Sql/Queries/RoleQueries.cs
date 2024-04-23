using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Sql.Queries
{
    public static class RoleQueries
    {
        public static string AddRole => "Insert into AspNetRoles(Id,Name,NormalizedName,ConcurrencyStamp,DatecreatedUtc,CreatedByUserId,DateModifiedUtc,ModifiedByUserId) values(newId(),@Name,@NormalizedName,@ConcurrencyStamp,@DatecreatedUtc,@CreatedByUserId,@DateModifiedUtc,@ModifiedByUserId)";
        public static string GetAllRoles => "Select r.*,u.FirstName+' '+u.LastName as fullname,(select STRING_AGG(ar.Name,',') from AspNetRoles ar join AspNetUserRoles aur on aur.RoleId=ar.Id where aur.UserId=case when r.ModifiedByUserId is not null then r.modifiedbyuserId else r.createdByUserId end) as role from AspNetRoles r join UserDetail u on u.AspNetUserId=case when r.ModifiedByUserId is not null then r.ModifiedByUserId else r.CreatedByUserId end";

        public static string DeleteRole => "Delete from AspNetRoles where Id=@Id";
        public static string GetRoleDetail => "Select * from AspNetRoles where Id=@Id";
        public static string UpdateRole => "update AspNetRoles set Name=@Name,NormalizedName=@NormalizedName,DateModifiedUtc=@DateModifiedUtc,ModifiedByUserId=@ModifiedByUserId where Id=@Id";
        public static string GetAllFilterRoles => "select * from (Select r.*,ud.FirstName+' '+ud.LastName as fullname,(select STRING_AGG(ar.Name,',') from AspNetRoles ar join AspNetUserRoles aur on aur.RoleId=ar.Id where aur.UserId=case when r.ModifiedByUserId is not null then r.modifiedbyuserId else r.createdByUserId end) as role from AspNetRoles r join UserDetail ud on ud.AspNetUserId=case when r.ModifiedByUserId is not null then r.ModifiedByUserId else r.CreatedByUserId end where Name like case when @Name is not null then @Name else '%' end and (@UpdatedDate IS NULL OR cast(r.DateModifiedUtc as date)=@UpdatedDate ) and (ud.FirstName LIKE (CASE WHEN @UpdatedBy IS NOT NULL THEN @UpdatedBy ELSE '%' END) OR ud.LastName LIKE (CASE WHEN @UpdatedBy IS NOT NULL THEN @UpdatedBy ELSE '%'END) or(select STRING_AGG(ar.name,',') from AspNetRoles ar join AspNetUserRoles aur on ar.Id=aur.RoleId where aur.UserId=case when r.ModifiedByUserId is not null then r.ModifiedByUserId else r.CreatedByUserId end) like @UpdatedBy )) as subquery";
        public static string GetRoleOfLoggedInUser => " Select r.Id from AspNetUserRoles aur join AspNetRoles r  on aur.RoleId=r.Id" +
            " where UserId=@UserId";
        public static string GetMenuAceessRightForRole => "select m.Id,m.name,m.parentId,case when exists(select 1 from UserMenuPermission up where up.MenuId=m.Id and up.RoleId=@Role and up.Isdeleted=0 and up.IsActive=1)then 'true' else 'false' end as IsUserHaveAccess  from menu m ";
        public static string DeleteExistingPermissionOfRole => "Update UserMenuPermission set IsActive=0,IsDeleted=1,DateModifiedUtc=@DateModifiedUtc,ModifiedByUserId=@ModifiedByUserId where RoleId=@RoleId";
        public static string SaveMenuAccessOfRole => "Insert into UserMenuPermission(RoleId,MenuId,IsActive,IsDeleted,DatecreatedUtc,CreatedByUserId) values(@RoleId,@MenuId,@IsActive,@IsDeleted,@DatecreatedUtc,@CreatedByUserId)";
    }
}
