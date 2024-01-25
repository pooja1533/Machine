using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Sql.Queries
{
    public class RequirementQueries
    {
        public static string PostRequirement => "Insert into Requirement values(@Name,@isActive,@IsDeleted)";
        public static string GetRequirement => "Select * from Requirement where IsDeleted=0";
        public static string GetActiveRequirement => "Select * from Requirement where IsDeleted=0 and IsActive=1";
        public static string GetRequirementDetail => "Select * from Requirement where Id=@Id";

        public static string DeleteRequirement => "update  Requirement set IsDeleted=1 where Id=@Id";
        public static string UpdateRequirement => "update Requirement set Name=@Name,IsActive=@IsActive where Id=@Id";
    }
}
