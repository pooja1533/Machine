using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Sql.Queries
{
    public class UserTypeQueries
    {
        public static string GetUserTypeDetail => "Select * from UserType where Id=@Id";
        public static string GetUserType => "Select * from UserType where IsDeleted=0";
        public static string GetActiveUserType => "Select * from UserType where IsDeleted=0 and IsActive=1";
        public static string PutUserType => "Update UserType set Name=@Name,IsActive=@IsActive where Id=@Id";
        public static string DeleteUserType => "Update UserType set IsDeleted=1,IsActive=0 where Id=@Id";
        public static string PostUserType => "Insert into UserType values(@Name,@IsDeleted,@IsActive,@CreatedByUserId,@DateCreatedUtc,@ModifiedByUserId,@DateModifiedUtc)";
    }
}
