using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Sql.Queries
{
    public class ActivityQueries
    {
        public static string PostActivity => "Insert into Activity values(@Name,@isActive,@IsDeleted)";
        public static string GetActivity => "Select * from Activity where IsDeleted=0";
        public static string GetActiveActivity => "Select * from Activity where IsDeleted=0 and IsActive=1";
        public static string GetActivityDetail => "Select * from Activity where Id=@Id";

        public static string DeleteActivity => "update  Activity set IsDeleted=1 where Id=@Id";
        public static string UpdateActivity => "update Activity set Name=@Name,IsActive=@IsActive where Id=@Id";
    }
}
