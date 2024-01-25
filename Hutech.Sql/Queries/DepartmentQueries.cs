using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Sql.Queries
{
    public class DepartmentQueries
    {
        public static string PostDepartment => "Insert into Department values(@Name,@isActive,@IsDeleted)";
        public static string GetDepartment => "Select * from Department where IsDeleted=0 ";
        public static string GetActiveDepartment => "Select * from Department where IsDeleted=0 and IsActive=1";
        public static string GetDepartmentDetail => "Select * from Department where Id=@Id";

        public static string DeleteDepartment => "update  Department set IsDeleted=1 where Id=@Id";
        public static string UpdateDepartment => "update Department set Name=@Name,IsActive=@IsActive where Id=@Id";
    }
}
