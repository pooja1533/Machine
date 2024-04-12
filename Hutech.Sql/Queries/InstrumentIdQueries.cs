using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Sql.Queries
{
    public class InstrumentIdQueries
    {
        public static string UpdateInstrumentId => "Update InstrumentsIds set InstrumentsId=@InstrumentsId,Model=@Model,InstrumentSerial=@InstrumentSerial,InstrumentId=@InstrumentId,LocationId=@LocationId,TeamId=@TeamId,TeamLocation=@TeamLocation,IsActive=@IsActive,DateModifiedUtc=@DateModifiedUtc,ModifiedByUserId=@ModifiedByUserId where Id=@Id";
        public static string GetInstrumentIdDetail => "Select i.*,t.Name as TeamName,l.Name as LocationName,ins.Name as InstrumentName  from InstrumentsIds i   join team t on i.TeamId=t.Id   join location l on l.Id=i.locationId   join instrument ins on ins.Id=i.InstrumentId   where i.Id=@Id";
        public static string DeleteInstrumentId => "update  InstrumentsIds set IsDeleted=1 where Id=@Id";
        public static string GetInstrumentId => "Select i.*,t.Name as TeamName,l.Name as LocationName,ins.Name as InstrumentName,ud.FirstName+' '+ud.LastName as FullName,(select STRING_AGG(r.name,',') from AspNetRoles r join AspNetUserRoles ar on ar.RoleId=r.Id where ar.UserId=case when i.ModifiedByUserId is not null then i.ModifiedByUserId else i.CreatedByUserId end) as Role from InstrumentsIds i join UserDetail ud on ud.AspNetUserId=case when i.ModifiedByUserId is not null then i.ModifiedByUserId else i.CreatedByUserId end left join team t on i.TeamId=t.Id left join location l on l.Id=i.locationId left join instrument ins on ins.Id=i.InstrumentId where i.IsDeleted=0 order by i.Id desc";
        public static string GetActiveInstrumentId => "Select i.*,t.Name as TeamName,l.Name as LocationName,ins.Name as InstrumentName  from InstrumentsIds i   join team t on i.TeamId=t.Id   join location l on l.Id=i.locationId   join instrument ins on ins.Id=i.InstrumentId   where i.IsDeleted=0 and i.IsActive=1";
        public static string PostInstrumentId => "Insert into InstrumentsIds values(@InstrumentsId,@Model,@InstrumentSerial,@InstrumentId,@LocationId,@TeamId,@TeamLocation,@IsActive,@IsDeleted,@DatecreatedUtc,@CreatedByUserId,@DateModifiedUtc,@ModifiedByUserId)";
        public static string GetAllFilterInstrumentId => "Select i.*,t.Name as TeamName,l.Name as LocationName,ins.Name as InstrumentName,ud.FirstName+' '+ud.LastName as FullName,(select STRING_AGG(r.name,',') from AspNetRoles r join AspNetUserRoles ar on ar.RoleId=r.Id where ar.UserId=case when i.ModifiedByUserId is not null then i.ModifiedByUserId else i.CreatedByUserId end) as Role from InstrumentsIds i join UserDetail ud on ud.AspNetUserId=case when i.ModifiedByUserId is not null then i.ModifiedByUserId else i.CreatedByUserId end  join team t on i.TeamId=t.Id and t.Name like case when @TeamName is not null then @TeamName else '%' end  join location l on l.Id=i.locationId and l.Name like case when @InstrumentLocation is not null then @InstrumentLocation else '%' end  join instrument ins on ins.Id=i.InstrumentId and ins.Name like case when @InstrumentName is not null then @InstrumentName else '%' end where i.IsDeleted=0 and i.IsActive=@Status and i.InstrumentsId like case when @Name is not null then @Name else '%' end and i.Model like case when @Model is not null then @Model else '%' end and i.InstrumentSerial like case when @InstrumentSerial is not null then @InstrumentSerial else '%' end  AND (@UpdatedDate IS NULL OR cast(i.DateModifiedUtc as date) =@UpdatedDate )  " +
            " AND (ud.FirstName LIKE (CASE WHEN @UpdatedBy IS NOT NULL THEN @UpdatedBy ELSE '%' END) OR ud.LastName LIKE (CASE WHEN @UpdatedBy IS NOT NULL THEN @UpdatedBy ELSE '%'END) or(select STRING_AGG(r.name,',') from AspNetRoles r join AspNetUserRoles ar on ar.RoleId=r.Id where ar.UserId=case when i.ModifiedByUserId is not null then i.ModifiedByUserId else i.CreatedByUserId end) like @UpdatedBy ) order by i.Id desc";
    }
}
