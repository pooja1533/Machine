using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Sql.Queries
{
    public static class RoleQueries
    {
        public static string AddRole => "Insert into AspNetRoles(Id,Name,NormalizedName,ConcurrencyStamp) values(newId(),@Name,@NormalizedName,@ConcurrencyStamp)";
        public static string GetAllRoles => "Select * from AspNetRoles";

        public static string DeleteRole => "Delete from AspNetRoles where Id=@Id";
        public static string GetRoleDetail => "Select * from AspNetRoles where Id=@Id";
        public static string UpdateRole => "update AspNetRoles set Name=@Name,NormalizedName=@NormalizedName where Id=@Id";
    }
}
