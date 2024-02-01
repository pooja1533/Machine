using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Sql.Queries
{
    public class InstrumentQueries
    {
        public static string UploadInstrumentDocument => "Insert into Document values(@Path,@FileName,@FileExtension,@CreatedDate,@CreatedBy,@ModifiedDate,@ModifiedBy,@IsDeleted) SELECT @@IDENTITY";
        public static string PostInstrument => "Insert into Instrument values(@Name,@isActive,@IsDeleted)";
        public static string GetInstrument => "Select * from Instrument where IsDeleted=0";
        public static string GetActiveInstrument => "Select * from Instrument where IsDeleted=0 and IsActive=1";
        //public static string GetInstrumentDetail => "Select * from Instrument where Id=@Id";
        public static string GetInstrumentDetail => "SELECT i.Id,i.Name,DocumentId = STUFF((SELECT  ', ' + CAST(s.DocumentId AS varchar) FROM InstrumentDocumentMapping s WHERE s.InstrumentId = I.Id and s.Isdeleted=0 FOR XML PATH('')), 1, 1, ''),Path= STUFF((SELECT  ', ' + CAST(d.Path AS varchar(max))FROM Document d join InstrumentDocumentMapping s on d.Id=s.DocumentId where d.Id=s.DocumentId and s.InstrumentId=i.Id  and s.Isdeleted=0 FOR XML PATH('')), 1, 1, '') FROM Instrument i (NOLOCK) WHERE Id = @Id";
        public static string DeleteInstrument => "update  Instrument set IsDeleted=1 where Id=@Id";
        public static string UpdateInstrument => "update Instrument set Name=@Name,IsActive=@IsActive where Id=@Id";
        public static string GetLastAddedInstrumentId => "Select Top 1 Id from Instrument order by Id desc";
        public static string AddInstrumentDocumentMapping => "Insert into InstrumentDocumentMapping values(@InstrumentId,@DocumentId,@IsDeleted,@CreatedDate,@CreatedBy,@ModifiedDate,@ModifiedBy)";
        public static string DeleteExistingInstrumentDocument => "Update InstrumentDocumentMapping set IsDeleted=1 where InstrumentId=@Id";
        public static string DeleteInstrumentDocument => "Update InstrumentDocumentMapping set IsDeleted=1 where InstrumentId=@InstrumentId and DocumentId=@DocumentId";
        public static string DeleteDocument => "Update Document set IsDeleted=1 where Id=@Id";
    }
}
