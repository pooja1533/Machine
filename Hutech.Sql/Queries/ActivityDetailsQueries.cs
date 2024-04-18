using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Sql.Queries
{
    public class ActivityDetailsQueries
    {
        public static string GetActivityDetail => "Select ad.*,ad.Department as DeaprtmentName,ad.Requirement as RequirementName,ad.Team as TeamName, DocumentId = STUFF((SELECT  ', ' + CAST(s.DocumentId AS varchar) FROM ActivityDetailDocumentMapping s WHERE s.ActivityDetailId = ad.Id and s.Isdeleted=0 FOR XML PATH('')), 1, 1, ''),Path= STUFF((SELECT  ', ' + CAST(d.Path AS varchar(max))FROM Document d join ActivityDetailDocumentMapping s on d.Id=s.DocumentId  where d.Id=s.DocumentId and s.ActivityDetailId=ad.Id  and s.Isdeleted=0 FOR XML PATH('')), 1, 1, '') from ActivityDetails ad where Id=@Id";

        //public static string GetActivityDetail => "Select ad.*,ad.Department as DeaprtmentName,ad.Requirement as RequirementName,ad.Team as TeamName from ActivityDetails ad where Id=@Id";
        public static string GetAllActivityDetails => "Select ad.*,ad.Department as DepartmentName,i.InstrumentsId as InstrumentIdName,ud.FirstName + ' ' + ud.LastName AS fullname,(SELECT STRING_AGG(r.name, ',') FROM AspNetRoles r JOIN AspNetUserRoles ar ON ar.RoleId = r.Id WHERE ar.UserId = CASE WHEN ad.ModifyByUserId IS NOT NULL THEN ad.ModifyByUserId ELSE ad.CreatedByUserId END) AS Role from ActivityDetails ad   JOIN UserDetail ud ON ud.AspNetUserId = CASE WHEN ad.ModifyByUserId IS NOT NULL THEN ad.ModifyByUserId ELSE ad.CreatedByUserId END join InstrumentsIds i on i.Id=ad.InstrumentId where ad.Isdeleted=0  and ad.CreatedByUserId=@LoggedInUser order by ad.Id desc";
        public static string GetAllUsersActivityDetails => "Select * from ActivityDetails where Isdeleted=0 ";
        public static string PutActivityDetail = "Update ActivityDetails set Remark=@Remark,PerformedDate=@PerformedDate,IsActive=@IsActive,ModifyByUserId=@ModifyByUserId,ModifiedDate=@ModifiedDate where Id=@Id";
        public static string DeleteActivityDetails => "Update ActivityDetails set IsDeleted=1 where Id=@Id";
        public static string PostActivityDetail => "Insert into ActivityDetails values(@InstrumentId,@InstrumentName,@InstrumentSerial," +
            "@Model,@LocationName,@InstrumentActivityId,@Days,@Frequency,@TeamName,@TeamLocation,@RequirementName,@DepartmentName," +
            "@Remark,@CreatedByUserId,@PerformedDate,@CreatedDate,@ModifyByUserId,@ModifiedDate,@IsActive,@IsDeleted)";

        public static string AddActivityDetailDocumentMapping => "Insert into ActivityDetailDocumentMapping values(@ActivityDetailId,@DocumentId,@IsDeleted,@CreatedDate,@CreatedBy,@ModifiedDate,@ModifiedBy)";
        public static string GetLastInsertedActivityDetailId => "Select Top 1 Id from ActivityDetails order by Id desc";
        public static string DeleteActivityDetailDocument => "Update ActivityDetailDocumentMapping set IsDeleted=1 where ActivityDetailId=@ActivityDetailId and DocumentId=@DocumentId";
        public static string GetAllFilterActivityDetail => "Select ad.*,ad.Department as DepartmentName,i.InstrumentsId as InstrumentIdName,ud.FirstName + ' ' + ud.LastName AS fullname,(SELECT STRING_AGG(r.name, ',') FROM AspNetRoles r JOIN AspNetUserRoles ar ON ar.RoleId = r.Id WHERE ar.UserId = CASE WHEN ad.ModifyByUserId IS NOT NULL THEN ad.ModifyByUserId ELSE ad.CreatedByUserId END) AS Role from ActivityDetails ad   JOIN UserDetail ud ON ud.AspNetUserId = CASE WHEN ad.ModifyByUserId IS NOT NULL THEN ad.ModifyByUserId ELSE ad.CreatedByUserId END join InstrumentsIds i on i.Id=ad.InstrumentId and i.InstrumentsId like case when @InstrumentIdName is not null then @InstrumentIdName else '%' end where ad.Isdeleted=0 and ad.InstrumentName like case when  @InstrumentName is not null then @InstrumentName else '%' end and ad.InstrumentSerial like case when @InstrumentSerial is not null then @InstrumentSerial else '%' end and ad.Model like case when @Model is not null then @Model else '%' end and ad.LocationName like case when @Location is not null then @Location else '%' end and ad.Department like case when @Department is not null then @Department else '%' end and   (ud.FirstName LIKE (CASE WHEN @UpdatedBy IS NOT NULL THEN @UpdatedBy ELSE '%' END) OR ud.LastName LIKE (CASE WHEN @UpdatedBy IS NOT NULL THEN @UpdatedBy ELSE '%'END) or(select STRING_AGG(r.name,',') from AspNetRoles r join AspNetUserRoles ar on ar.RoleId=r.Id where ar.UserId=case when ad.ModifyByUserId is not null then ad.ModifyByUserId else ad.CreatedByUserId end) like @UpdatedBy ) and ad.IsActive=@Status AND (@UpdatedDate IS NULL OR cast(ad.PerformedDate as date) =@UpdatedDate ) order by ad.Id desc";
        //public static string GetAllFilterActivityDetail => "Select ad.*,ad.Department as DepartmentName,i.InstrumentsId as InstrumentIdName,ud.FirstName + ' ' + ud.LastName AS fullname,(SELECT STRING_AGG(r.name, ',') FROM AspNetRoles r JOIN AspNetUserRoles ar ON ar.RoleId = r.Id WHERE ar.UserId = CASE WHEN ad.ModifyByUserId IS NOT NULL THEN ad.ModifyByUserId ELSE ad.CreatedByUserId END) AS Role from ActivityDetails ad   JOIN UserDetail ud ON ud.AspNetUserId = CASE WHEN ad.ModifyByUserId IS NOT NULL THEN ad.ModifyByUserId ELSE ad.CreatedByUserId END join InstrumentsIds i on i.Id=ad.InstrumentId where ad.Isdeleted=0  and ad.CreatedByUserId=@LoggedInUser order by ad.Id desc";
    }
}
