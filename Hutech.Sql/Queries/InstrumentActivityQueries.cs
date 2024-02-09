using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Sql.Queries
{
    public class InstrumentActivityQueries
    {
        public static string PostInstrumentActivity => "Insert into InstrumentActivity values(@InstrumentId,@ActivityId,@InstrumentActivityName,@Frequency,@FrequencyTime,@Days,@RequirementId,@DepartmentId,@BeforeAlerts,@IsActive,@IsDeleted,@BeforeAlertsTime,@CreatedDateTime,@ModifiedDateTime)";
        public static string GetLastInsertedId => "Select top 1 Id from InstrumentActivity order by id desc";
        public static string AddUserOfInstrumentActivity => "Insert into InstrumentActivityUserModal values(@InstrumentActivityId,@GroupId,@IsActive,@IsDeleted)";
        public static string GetInstrumentActivity => " Select ia.Id,ia.Days,ia.IsActive,ia.BeforeAlertsTime,ia.InstrumentActivityName,ia.FrequencyTime,i.Name as InstrumentName,a.Name as activityName,ia.FrequencyUnit as Frequency,r.name as RequirementName,d.name as DeaprtmentName from InstrumentActivity ia left join Instrument i on i.Id=ia.InstrumentId " +
            " left join activity a on a.Id= ia.activityId "+
            " left join Requirement r on r.Id = ia.RequirementID"+
            " left join Department d on d.Id=ia.DepartmentId where ia.IsDeleted=0";
        public static string GetActiveInstrumentActivity => "Select ia.Id,ia.Days,ia.IsActive,ia.BeforeAlertsTime,ia.InstrumentActivityName,ia.FrequencyTime,i.Name as InstrumentName,a.Name as activityName,ia.FrequencyUnit as Frequency,r.name as RequirementName,d.name as DeaprtmentName from InstrumentActivity ia left join Instrument i on i.Id=ia.InstrumentId " +
            " left join activity a on a.Id= ia.activityId left join Requirement r on r.Id = ia.RequirementID left join Department d on d.Id=ia.DepartmentId where ia.IsDeleted=0 and ia.IsActive=1";
        public static string DeleteExistingInstrumentActivityForGroup => "Delete from  InstrumentActivityUserModal where InstrumentActivityId= @InstrumentActivityId";
        public static string DeleteExistingInstrumentActivityGroup => "Update InstrumentActivityUserModal set Isdeleted=1 where InstrumentActivityId= @InstrumentActivityId";
        public static string DeleteInstrumentActivity => "Update InstrumentActivity set Isdeleted=1 where Id=@Id";
        public static string GetInstrumentActivityDetail => "select ia.*,ia.FrequencyUnit as Frequency, (SELECT STRING_AGG (g.name, ',') FROM InstrumentActivityUserModal nm   INNER JOIN Groups g on g.Id = nm.GroupId WHERE ia.Id = nm.InstrumentActivityId AND nm.IsDeleted = 0) as InstrumentActivityGroup,(SELECT STRING_AGG (nm.GroupId, ',') FROM InstrumentActivityUserModal nm  WHERE ia.Id = nm.InstrumentActivityId AND nm.IsDeleted = 0) as SelectedGroups from InstrumentActivity ia where ia.Id=@Id";
        public static string GetInstrumentActivityDetailData => "select ia.*,ia.FrequencyUnit as Frequency,d.Name as DeaprtmentName,r.Name as RequirementName, (SELECT STRING_AGG (g.name, ',') FROM InstrumentActivityUserModal nm   INNER JOIN Groups g on g.Id = nm.GroupId  WHERE ia.Id = nm.InstrumentActivityId AND nm.IsDeleted = 0) as InstrumentActivityGroup,(SELECT STRING_AGG (nm.GroupId, ',') FROM InstrumentActivityUserModal nm   WHERE ia.Id = nm.InstrumentActivityId AND nm.IsDeleted = 0) as SelectedGroups from InstrumentActivity ia  join Department d on d.Id=ia.DepartmentId join  Requirement r on r.Id=ia.RequirementId where ia.Id=@Id";
        public static string PutInstrumentActivity => "Update InstrumentActivity set InstrumentId=@InstrumentId,ActivityId=@ActivityId," +
            "InstrumentActivityName=@InstrumentActivityName,FrequencyUnit=@Frequency,FrequencyTime=@FrequencyTime,Days=@Days,RequirementId=@RequirementId,DepartmentId=@DepartmentId,BeforeAlerts=@BeforeAlerts,BeforeAlertsTime=@BeforeAlertsTime,IsActive=@IsActive where Id=@Id";
        //public static string AddInstrumentActivityForGroup => "Insert into InstrumentActivityUserModal";
    }
}
