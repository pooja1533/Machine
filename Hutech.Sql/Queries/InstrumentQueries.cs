using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Sql.Queries
{
    public class InstrumentQueries
    {
        public static string PostInstrument => "Insert into Instrument values(@Name,@isActive,@IsDeleted)";
        public static string GetInstrument => "Select * from Instrument where IsDeleted=0";
        public static string GetInstrumentDetail => "Select * from Instrument where Id=@Id";

        public static string DeleteInstrument => "update  Instrument set IsDeleted=1 where Id=@Id";
        public static string UpdateInstrument => "update Instrument set Name=@Name,IsActive=@IsActive where Id=@Id";
    }
}
