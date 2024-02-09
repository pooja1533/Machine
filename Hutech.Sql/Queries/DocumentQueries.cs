using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Sql.Queries
{
    public class DocumentQueries
    {
        public static string UploadInstrumentDocument => "Insert into Document values(@Path,@FileName,@FileExtension,@CreatedDate,@CreatedBy,@ModifiedDate,@ModifiedBy,@IsDeleted) SELECT @@IDENTITY";
        public static string DeleteDocument => "Update Document set IsDeleted=1 where Id=@Id";

    }
}
