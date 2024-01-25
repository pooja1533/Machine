using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Sql.Queries
{
    public static class UserQueries
    {
        //public static string GetAllUsers = "Select a.Id,a.UserName,u.FirstName,u.Lastname,u.PhoneNumber,u.Address from AspNetUsers a left join UserDetail u on u.userId=a.Id" +
        //    " where u.IsActive=1 and a.Id!=@Id";
        public static string GetRoleAccordingToRole => "select r.Id,r.name from AspNetRoles r where r.id in (select SubRoleId from RoleSubRoleMapping where RoleId=@roleId)";
        public static string GetRoleId => "Select Id from AspNetRoles where Name=@Role";
        public static string GetAllUsers = " Select a.Id,a.UserName,u.FirstName,u.Lastname,u.PhoneNumber,u.Address,r.Name as UserRole from AspNetUsers a left join UserDetail u on u.userId=a.Id join AspNetUserRoles ar on ar.UserId=a.Id join AspNetRoles r on r.Id=ar.RoleId  where u.IsActive=1 and a.Id!=@Id and r.Id in (select SubRoleId from RoleSubRoleMapping where RoleId=@RoleId)";
        public static string GetUsers => "  Select a.Id,a.UserName,u.FirstName,u.Lastname,u.PhoneNumber,u.Address,r.Name as UserRole from AspNetUsers a left join UserDetail u on u.userId=a.Id join AspNetUserRoles ar on ar.UserId=a.Id join AspNetRoles r on r.Id=ar.RoleId  where u.IsActive=1 ";
        public static string GetUserById => "Select a.Id,a.UserName,u.FirstName,u.Lastname,u.PhoneNumber,u.Address,r.Id as roleId from AspNetUsers a left join UserDetail u on u.userId=a.Id left join AspNetUserRoles ur on ur.UserId=a.Id left join AspNetRoles r on r.Id=ur.RoleId where u.UserId=@Id";

        public static string DeleteUser = "Update UserDetail set IsActive=0 where UserId=@userId";

        public static string UpdateUser => "Update AspNetUsers set UserName=@UserName,NormalizedUserName=@NormalizedUserName,Email=@UserName,NormalizedEmail=@NormalizedUserName where Id=@Id";
        public static string UpdateUserDetail => "Update UserDetail set FirstName=@FirstName,LastName=@LastName,PhoneNumber=@PhoneNumber,Address=@Address where UserId=@Id";
        public static string AddUserRole => "Insert into AspNetUserRoles values(@UserId,@RoleId)";
        public static string DeleteRoleOfUser => "Delete From AspNetUserRoles where UserId=@Id";
        public static string GetAllUsersForAdmin = "Select a.Id,a.UserName,u.FirstName,u.Lastname,u.PhoneNumber,u.Address,r.Name as RoleName from AspNetUsers a left join UserDetail u on u.userId=a.Id" +
           " left join AspNetUserRoles ar on ar.UserId=a.Id  left join AspNetRoles r on r.Id=ar.RoleId where u.IsActive=1 and a.Id!=@Id";
    }
}
