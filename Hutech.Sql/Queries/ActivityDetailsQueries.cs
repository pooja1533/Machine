using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Sql.Queries
{
    public class ActivityDetailsQueries
    {
        public static string GetActivityDetail => "Select ad.*,ad.Department as DeaprtmentName,ad.Requirement as RequirementName,ad.Team as TeamName from ActivityDetails ad where Id=@Id";
        public static string GetAllActivityDetails => "Select * from ActivityDetails where Isdeleted=0  and CreatedByUserId=@LoggedInUser";
        public static string PutActivityDetail = "Update ActivityDetails set Remark=@Remark,PerformedDate=@PerformedDate,IsActive=@IsActive where Id=@Id";
        public static string DeleteActivityDetails => "Update ActivityDetails set IsDeleted=1 where Id=@Id";
        public static string PostActivityDetail => "Insert into ActivityDetails values(@InstrumentId,@InstrumentName,@InstrumentSerial," +
            "@Model,@LocationName,@InstrumentActivityId,@Days,@Frequency,@TeamName,@TeamLocation,@RequirementName,@DeaprtmentName," +
            "@Remark,@CreatedByUserId,@PerformedDate,@CreatedDate,@ModifyByUserId,@ModifiedDate,@IsActive,@IsDeleted)";
    }
}
