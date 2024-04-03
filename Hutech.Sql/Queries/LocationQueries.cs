using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Sql.Queries
{
    public class LocationQueries
    {
        public static string PostLocation => "Insert into Location values(@Name,@isActive,@IsDeleted,@DatecreatedUtc,@CreatedByUserId,@DateModifiedUtc,@ModifiedByUserId)";
        public static string GetActiveLocation => "Select * from Location where IsDeleted=0 and IsActive=1";
        public static string GetLocation => "Select * from Location where IsDeleted=0";
        public static string GetLocationDetail => "Select * from Location where Id=@Id";

        public static string DeleteLocation => "update  Location set IsDeleted=1 where Id=@Id";
        public static string UpdateLocation => "update Location set Name=@Name,IsActive=@IsActive where Id=@Id";
    }
}
