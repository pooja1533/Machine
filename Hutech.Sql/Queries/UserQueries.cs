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
        public static string DeleteExistingRoleOfUser => "Delete from AspNetUserRoles where UserId=@UserId";
        public static string PutUser => "Update UserDetail set UserTypeId=@UserTypeId,FirstName=@FirstName,LastName=@LastName,EmployeeId=@EmployeeId,Email=@Email,DateModifiedUtc=@DateModifiedUtc,LocationId=@LocationId,DepartmentId=@DepartmentId,Remark=@Remark,LoginValidityInDays=@LoginValidityDays,WindowsUserName=@WindowsUserName where Id=@UserId";
        public static string PostAspnetUser => "Insert into AspNetUsersValues(@UserName)";
        public static string PostUser => "Insert into UserDetail Values(@AspNetUserId,@UserTypeId,@FirstName,@LastName,@EmployeeId,@Email,@IsActive,@CreatedDate,@DateModifiedUtc,@LocationId,@DepartmentId,@Remark,@LoginValidityDays,@WindowsUserName,@UserstatusId,@RemarkForReject)";
        public static string GetRoleAccordingToRole => "select r.Id,r.name from AspNetRoles r where r.id in (select SubRoleId from RoleSubRoleMapping where RoleId=@roleId)";
        public static string GetRoleId => "Select Id from AspNetRoles where Name=@Role";
        public static string GetAllUsers = "select u.Id as UserId,ut.Name as UserType,u.EmployeeId,u.Remark,u.FirstName+' '+u.LastName as fullName,us.Name as UserstatusName,u.WindowsUserName,u.Email,d.Name as DepartmentName,L.Name as LocationName,(select STRING_AGG(r.name,',') from AspNetRoles r inner join AspNetUserRoles ar on ar.RoleId=r.Id where u.AspNetUserId=ar.UserId) as RoleName from UserDetail u join Department d on d.Id=u.DepartmentId join Location l on l.Id=u.LocationId join UserType ut on ut.Id=u.UserTypeId join userstatus us on us.Id=u.UserstatusId where u.IsActive=1 and u.AspNetUserId!=@Id";
        public static string GetUserDetail => "select u.Id as UserId,ut.Name as UserType,u.RemarkForReject,u.EmployeeId,u.FirstName,u.LastName,u.FirstName+' '+u.LastName as fullName,u.Remark,us.Name as UserstatusName,u.WindowsUserName,u.Email,d.Name as DepartmentName,L.Name as LocationName,(select STRING_AGG(r.name,',') from AspNetRoles r inner join AspNetUserRoles ar on ar.RoleId=r.Id where u.AspNetUserId=ar.UserId) as RoleName from UserDetail u join Department d on d.Id=u.DepartmentId join Location l on l.Id=u.LocationId join UserType ut on ut.Id=u.UserTypeId join UserStatus us on us.Id=u.UserstatusId where u.IsActive=1 and u.Id=@Id";
        public static string GetUsers => " Select a.Id,a.UserName,u.UserstatusId,u.FirstName,u.Lastname,u.PhoneNumber,u.Address,r.Name as UserRole from AspNetUsers a left join UserDetail u on u.userId=a.Id join AspNetUserRoles ar on ar.UserId=a.Id join AspNetRoles r on r.Id=ar.RoleId  where u.IsActive=1 ";
        public static string GetUserById => "Select u.AspNetUserId,u.Id as UserId,u.UserstatusId,u.LoginValidityInDays as LoginValidityDays,u.FirstName,u.Lastname,u.EmployeeId,u.Email,u.LocationId,u.UserTypeId,u.DepartmentId,u.Remark,u.WindowsUserName ,(select STRING_AGG(ar.Id,',') from AspNetRoles ar join AspNetUserRoles au on au.RoleId=ar.Id where u.AspNetUserId= au.UserId)as RoleName from UserDetail u join AspNetUserRoles ar on ar.UserId=u.AspNetUserId join AspNetRoles r on r.Id=ar.RoleId where u.Id= @Id";

        public static string DeleteUser = "Update UserDetail set IsActive=0 where Id=@userId";

        public static string UpdateUser => "Update AspNetUsers set UserName=@UserName,NormalizedUserName=@NormalizedUserName,Email=@UserName,NormalizedEmail=@NormalizedUserName where Id=@Id";
        public static string UpdateUserDetail => "Update UserDetail set FirstName=@FirstName,LastName=@LastName,PhoneNumber=@PhoneNumber,Address=@Address where UserId=@Id";
        public static string AddUserRole => "Insert into AspNetUserRoles values(@UserId,@RoleId)";
        public static string DeleteRoleOfUser => "Delete From AspNetUserRoles where UserId=@Id";
        public static string GetAllUsersForAdmin = "Select a.Id,a.UserName,u.FirstName,u.Lastname,u.PhoneNumber,u.Address,r.Name as RoleName from AspNetUsers a left join UserDetail u on u.userId=a.Id" +
           " left join AspNetUserRoles ar on ar.UserId=a.Id  left join AspNetRoles r on r.Id=ar.RoleId where u.IsActive=1 and a.Id!=@Id";
        public static string ApproveUser => "Update UserDetail set UserstatusId=(Select Id from UserStatus where Name='Performed') where Id=@Id";
        public static string RejectUser => "Update UserDetail set RemarkForReject=@Comment,UserstatusId=(select Id from UserStatus where name='Rejected')where Id=@Id";
        public static string GetUserDefualtStatus => "Select Id from UserStatus where Name='Pending'";
        public static string GetAllFilterUser = "select u.Id as UserId,ut.Name as UserType,u.EmployeeId,u.Remark,u.FirstName+' '+u.LastName as fullName,us.Name as UserstatusName," +
            "u.WindowsUserName,u.Email,d.Name as DepartmentName,L.Name as LocationName,(select STRING_AGG(r.name,',') from AspNetRoles r inner join AspNetUserRoles ar on ar.RoleId=r.Id  where u.AspNetUserId=ar.UserId) as RoleName from UserDetail u " +
            " join Department d on d.Id=u.DepartmentId join Location l on l.Id=u.LocationId join UserType ut on ut.Id=u.UserTypeId join userstatus us on us.Id=u.UserstatusId where u.IsActive=@Status and u.AspNetUserId!=@Id and " +
            "(u.FirstName like case when @FullName is not null then @FullName else '%' end or u.LastName like case when @FullName is not null then @FullName else '%' end) " +
            " and (u.Email like case when @Email is not null then @Email else '%' end  or u.email is null)" +
            " and u.EmployeeId like case when @EmployeeId is not null then @EmployeeId else '%' end" +
          " AND COALESCE(u.WindowsUserName, '') LIKE COALESCE(@userName, '%') "+
            " and u.UserTypeId= case when @UserTypeId > 0 then @UserTypeId else u.UserTypeId end "+
           " and u.DepartmentId= case when @DepartmentId> 0 then @DepartmentId else u.DepartmentId end"+
             " and u.LocationId= case when @LocationId> 0 then @LocationId else u.LocationId end"+
            " AND (@RoleId IS NULL OR EXISTS (SELECT 1 FROM AspNetUserRoles aur WHERE aur.UserId = u.AspNetUserId AND aur.RoleId = @RoleId))";
    
     public static string CheckEmployeeIdExist => "SELECT CASE WHEN EXISTS (SELECT 1 FROM UserDetail WHERE EmployeeId = @EmployeeId) THEN 1 ELSE 0 END";

    }
}
