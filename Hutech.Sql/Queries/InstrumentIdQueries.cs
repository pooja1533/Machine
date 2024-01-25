using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Sql.Queries
{
    public class InstrumentIdQueries
    {
        public static string UpdateInstrumentId => "Update InstrumentsIds set InstrumentsId=@InstrumentsId,Model=@Model,InstrumentSerial=@InstrumentSerial,InstrumentId=@InstrumentId,LocationId=@LocationId,TeamId=@TeamId,TeamLocation=@TeamLocation,IsActive=@IsActive where Id=@Id";
        public static string GetInstrumentIdDetail => "Select i.*,t.Name as TeamName,l.Name as LocationName,ins.Name as InstrumentName  from InstrumentsIds i   join team t on i.TeamId=t.Id   join location l on l.Id=i.locationId   join instrument ins on ins.Id=i.InstrumentId   where i.Id=@Id";
        public static string DeleteInstrumentId => "update  InstrumentsIds set IsDeleted=1 where Id=@Id";
        public static string GetInstrumentId => "Select i.*,t.Name as TeamName,l.Name as LocationName,ins.Name as InstrumentName  from InstrumentsIds i  left join team t on i.TeamId=t.Id left  join location l on l.Id=i.locationId  left join instrument ins on ins.Id=i.InstrumentId   where i.IsDeleted=0";
        public static string GetActiveInstrumentId => "Select i.*,t.Name as TeamName,l.Name as LocationName,ins.Name as InstrumentName  from InstrumentsIds i   join team t on i.TeamId=t.Id   join location l on l.Id=i.locationId   join instrument ins on ins.Id=i.InstrumentId   where i.IsDeleted=0 and i.IsActive=1";
        public static string PostInstrumentId => "Insert into InstrumentsIds values(@InstrumentsId,@Model,@InstrumentSerial,@InstrumentId,@LocationId,@TeamId,@TeamLocation,@IsActive,@IsDeleted)";
    }
}
