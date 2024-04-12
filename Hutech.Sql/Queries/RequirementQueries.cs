using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Sql.Queries
{
    public class RequirementQueries
    {
        public static string PostRequirement => "Insert into Requirement values(@Name,@isActive,@IsDeleted,@DatecreatedUtc,@CreatedByUserId,@DateModifiedUtc,@ModifiedByUserId)";
        public static string GetRequirement => "Select * from Requirement where IsDeleted=0";
        public static string GetActiveRequirement => "Select * from Requirement where IsDeleted=0 and IsActive=1";
        public static string GetRequirementDetail => "Select * from Requirement where Id=@Id";

        public static string DeleteRequirement => "update  Requirement set IsDeleted=1 where Id=@Id";
        public static string UpdateRequirement => "update Requirement set Name=@Name,IsActive=@IsActive,DateModifiedUtc=@DateModifiedUtc,ModifiedByUserId=@ModifiedByUserId where Id=@Id";
        public static string GetAllFilterRequirement => "select * from (SELECT  re.*,ud.FirstName + ' ' + ud.LastName AS fullname,(SELECT STRING_AGG(r.name, ',') FROM AspNetRoles r JOIN AspNetUserRoles ar ON ar.RoleId = r.Id WHERE ar.UserId = CASE WHEN re.ModifiedByUserId IS NOT NULL THEN re.ModifiedByUserId ELSE re.CreatedByUserId END) AS Role  FROM Requirement re JOIN UserDetail ud ON ud.AspNetUserId = CASE WHEN re.ModifiedByUserId IS NOT NULL THEN re.ModifiedByUserId ELSE re.CreatedByUserId END WHERE re.IsDeleted = 0 and re.IsActive=@Status AND Name LIKE CASE WHEN @Name IS NOT NULL THEN @Name ELSE '%' END AND (@UpdatedDate IS NULL OR cast(re.DateModifiedUtc as date) =@UpdatedDate ) AND (ud.FirstName LIKE (CASE WHEN @UpdatedBy IS NOT NULL THEN @UpdatedBy ELSE '%' END) OR ud.LastName LIKE (CASE WHEN @UpdatedBy IS NOT NULL THEN @UpdatedBy ELSE '%'END) or(select STRING_AGG(r.name,',') from AspNetRoles r join AspNetUserRoles ar on ar.RoleId=r.Id where ar.UserId=case when re.ModifiedByUserId is not null then re.ModifiedByUserId else re.CreatedByUserId end) like @UpdatedBy )) as subquery order by subquery.Id desc";
    }
}
