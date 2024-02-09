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
        public static string GetAllActivityDetails => "Select * from ActivityDetails where Isdeleted=0  and CreatedByUserId=@LoggedInUser";
        public static string GetAllUsersActivityDetails => "Select * from ActivityDetails where Isdeleted=0 ";
        public static string PutActivityDetail = "Update ActivityDetails set Remark=@Remark,PerformedDate=@PerformedDate,IsActive=@IsActive where Id=@Id";
        public static string DeleteActivityDetails => "Update ActivityDetails set IsDeleted=1 where Id=@Id";
        public static string PostActivityDetail => "Insert into ActivityDetails values(@InstrumentId,@InstrumentName,@InstrumentSerial," +
            "@Model,@LocationName,@InstrumentActivityId,@Days,@Frequency,@TeamName,@TeamLocation,@RequirementName,@DeaprtmentName," +
            "@Remark,@CreatedByUserId,@PerformedDate,@CreatedDate,@ModifyByUserId,@ModifiedDate,@IsActive,@IsDeleted)";

        public static string AddActivityDetailDocumentMapping => "Insert into ActivityDetailDocumentMapping values(@ActivityDetailId,@DocumentId,@IsDeleted,@CreatedDate,@CreatedBy,@ModifiedDate,@ModifiedBy)";
        public static string GetLastInsertedActivityDetailId => "Select Top 1 Id from ActivityDetails order by Id desc";
        public static string DeleteActivityDetailDocument => "Update ActivityDetailDocumentMapping set IsDeleted=1 where ActivityDetailId=@ActivityDetailId and DocumentId=@DocumentId";
    }
}
