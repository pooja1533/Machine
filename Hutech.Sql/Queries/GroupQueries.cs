using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Sql.Queries
{
    public class GroupQueries
    {
        public static string PostGroup => "Insert into Groups values(@Name,@isActive,@IsDeleted)";
        public static string GetGroup => "Select * from Groups where IsDeleted=0";
        public static string GetAllActiveGroup => "Select * from Groups where IsDeleted=0 and IsActive=1";
        public static string GetGroupDetail => "Select * from Groups where Id=@Id";

        public static string DeleteGroup => "update  Groups set IsDeleted=1 where Id=@Id";
        public static string UpdateGroup => "update Groups set Name=@Name,IsActive=@IsActive where Id=@Id";
    }
}
