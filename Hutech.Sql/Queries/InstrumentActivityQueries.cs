using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace Hutech.Sql.Queries
{
    public class InstrumentActivityQueries
    {
        public static string PostInstrumentActivity => "Insert into InstrumentActivity values(@InstrumentId,@ActivityId,@InstrumentActivityName,@Frequency,@FrequencyTime,@Days,@RequirementId,@DepartmentId,@BeforeAlerts,@IsActive,@IsDeleted,@BeforeAlertsTime,@CreatedDateTime,@ModifiedDateTime,@InstrumentActivityId,@CreatedByUserId,@ModifiedByUserId)";
        public static string GetLastInsertedId => "Select top 1 Id from InstrumentActivity order by id desc";
        public static string AddUserOfInstrumentActivity => "Insert into InstrumentActivityUserModal values(@InstrumentActivityId,@GroupId,@IsActive,@IsDeleted)";
        public static string GetInstrumentActivity => " Select ia.Id,ia.InstrumentActivityId,ia.BeforeAlerts,ia.Days,ia.IsActive,ia.BeforeAlertsTime,ia.InstrumentActivityName,ia.FrequencyTime,i.Name as InstrumentName,a.Name as activityName,ia.FrequencyUnit as Frequency,r.name as RequirementName,d.name as DeaprtmentName from InstrumentActivity ia left join Instrument i on i.Id=ia.InstrumentId " +
            " left join activity a on a.Id= ia.activityId "+
            " left join Requirement r on r.Id = ia.RequirementID"+
            " left join Department d on d.Id=ia.DepartmentId where ia.IsDeleted=0 order by ia.Id desc";
        public static string GetActiveInstrumentActivity => "Select ia.Id,ia.Days,ia.IsActive,ia.BeforeAlertsTime,ia.CreatedDateTime,ia.BeforeAlerts,ia.InstrumentActivityName,ia.FrequencyTime,i.Name as InstrumentName,a.Name as activityName,ia.FrequencyUnit as Frequency,r.name as RequirementName,d.name as DeaprtmentName from InstrumentActivity ia left join Instrument i on i.Id=ia.InstrumentId " +
            " left join activity a on a.Id= ia.activityId left join Requirement r on r.Id = ia.RequirementID left join Department d on d.Id=ia.DepartmentId where ia.IsDeleted=0 and ia.IsActive=1";
        public static string DeleteExistingInstrumentActivityForGroup => "Delete from  InstrumentActivityUserModal where InstrumentActivityId= @InstrumentActivityId";
        public static string DeleteExistingInstrumentActivityGroup => "Update InstrumentActivityUserModal set Isdeleted=1 where InstrumentActivityId= @InstrumentActivityId";
        public static string DeleteInstrumentActivity => "Update InstrumentActivity set Isdeleted=1 where Id=@Id";
        public static string GetInstrumentActivityDetail => "select ia.*,ia.FrequencyUnit as Frequency, (SELECT STRING_AGG (g.name, ',') FROM InstrumentActivityUserModal nm   INNER JOIN Groups g on g.Id = nm.GroupId WHERE ia.Id = nm.InstrumentActivityId AND nm.IsDeleted = 0) as InstrumentActivityGroup,(SELECT STRING_AGG (nm.GroupId, ',') FROM InstrumentActivityUserModal nm  WHERE ia.Id = nm.InstrumentActivityId AND nm.IsDeleted = 0) as SelectedGroups from InstrumentActivity ia where ia.Id=@Id";
        public static string GetInstrumentActivityDetailData => "select ia.*,ia.FrequencyUnit as Frequency,d.Name as DepartmentName,r.Name as RequirementName, (SELECT STRING_AGG (g.name, ',') FROM InstrumentActivityUserModal nm   INNER JOIN Groups g on g.Id = nm.GroupId  WHERE ia.Id = nm.InstrumentActivityId AND nm.IsDeleted = 0) as InstrumentActivityGroup,(SELECT STRING_AGG (nm.GroupId, ',') FROM InstrumentActivityUserModal nm   WHERE ia.Id = nm.InstrumentActivityId AND nm.IsDeleted = 0) as SelectedGroups from InstrumentActivity ia  join Department d on d.Id=ia.DepartmentId join  Requirement r on r.Id=ia.RequirementId where ia.Id=@Id";
        public static string PutInstrumentActivity => "Update InstrumentActivity set InstrumentId=@InstrumentId,ActivityId=@ActivityId," +
            "InstrumentActivityName=@InstrumentActivityName,FrequencyUnit=@Frequency,FrequencyTime=@FrequencyTime,Days=@Days,RequirementId=@RequirementId,DepartmentId=@DepartmentId,BeforeAlerts=@BeforeAlerts,BeforeAlertsTime=@BeforeAlertsTime,IsActive=@IsActive,ModifiedDateTime=@ModifiedDateTime,ModifiedByUserId=@ModifiedByUserId,InstrumentActivityId=@InstrumentActivityId where Id=@Id";
        //public static string AddInstrumentActivityForGroup => "Insert into InstrumentActivityUserModal";
        public static string GetLastPerformedDateForInstrumentActivity => "Select top 1 PerformedDate from ActivityDetails where InstrumentActivityId=@Id and isdeleted=0 order by PerformedDate desc";
        public static string GetAllFilterInstrumentActivity => "Select ia.Id,ia.InstrumentActivityId,ud.FirstName + ' ' + ud.LastName AS fullname,ia.BeforeAlerts,ia.Days,ia.IsActive,ia.BeforeAlertsTime,ia.InstrumentActivityName,ia.CreatedDateTime,ia.ModifiedDateTime,ia.FrequencyTime,i.Name as InstrumentName,a.Name as activityName,ia.FrequencyUnit as Frequency,r.name as RequirementName,d.name as DepartmentName,ia.BeforeAlertsTime,(SELECT STRING_AGG(r.name, ',') FROM AspNetRoles r JOIN AspNetUserRoles ar ON ar.RoleId = r.Id WHERE ar.UserId = CASE WHEN ia.ModifiedByUserId IS NOT NULL THEN ia.ModifiedByUserId ELSE ia.CreatedByUserId END) AS Role from InstrumentActivity ia join UserDetail ud on  ud.AspNetUserId=case when ia.ModifiedByUserId is not null then ia.ModifiedByUserId else ia.CreatedByUserId end " +
"  join Instrument i on i.Id=ia.InstrumentId and i.Name like case when @InstrumentName is not null then @InstrumentName else '%' end  join activity a on a.Id= ia.activityId and a.Name like  case when @ActivityName is not null then @ActivityName else '%' end" +
"  join Requirement r on r.Id = ia.RequirementID and r.Name like case when @RequirementName is not null then @RequirementName else '%' end " +
"  join Department d on d.Id= ia.DepartmentId and d.Name like case when @DepartmentName is not null then @DepartmentName else '%' end" +
" where ia.IsDeleted= 0 and ia.IsActive =@Status and ia.InstrumentActivityId like case when @InstrumentActivityId is not null then @InstrumentActivityId else '%' end " +
" AND (@UpdatedDate IS NULL OR cast(ia.ModifiedDateTime as date) =@UpdatedDate ) AND(ud.FirstName LIKE (CASE WHEN @UpdatedBy IS NOT NULL THEN @UpdatedBy ELSE '%' END) OR ud.LastName LIKE (CASE WHEN @UpdatedBy IS NOT NULL THEN @UpdatedBy ELSE '%'END)" +
 " or(select STRING_AGG(r.name,',') from AspNetRoles r join AspNetUserRoles ar on ar.RoleId=r.Id where ar.UserId=case when ia.ModifiedByUserId is not null then ia.ModifiedByUserId else ia.CreatedByUserId end) like @UpdatedBy )order by ia.Id desc";
    }
}
