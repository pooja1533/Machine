using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Sql.Queries
{
    public  class OrganizationQueries
    {
        public static string GetOrganization => "Select * from Organization where IsActive=1 and IsDeleted=0";
        public static string DeleteOrganization => "Update Organization set IsDeleted=1,IsActive=0 where Id=@Id ";
        public static string GetOrganizationDetail => "Select * from Organization where Id=@Id";
        public static string UpdateOrganization => "Update Organization set Name=@Name,IsActive=@IsActive where Id=@Id";
        public static string AddOrganization => "Insert into Organization values(@Name,@IsActive,@IsDeleted,@CreatedByUserId,@DateCreatedUtc,@ModifiedByUserId,@DateModifiedUtc)";
    }
}
