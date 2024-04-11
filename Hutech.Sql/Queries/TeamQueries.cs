using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Sql.Queries
{
    public class TeamQueries
    {
        public static string GetTeamDetail => "Select * from Team where Id=@Id";
        public static string UpdateTeam => "Update Team set Name=@Name,IsActive=@IsActive,LocationId=@LocationId,DateModifiedUtc=@DateModifiedUtc,ModifiedByUserId=@ModifiedByUserId where Id=@Id";
        public static string PostTeam => "Insert into Team values(@Name,@LocationId,@isActive,@IsDeleted,@DatecreatedUtc,@CreatedByUserId,@DateModifiedUtc,@ModifiedByUserId)";
        public static string DeleteTeam => "update  Team set IsDeleted=1 where Id=@Id";
        public static string GetActiveTeam => "Select * from Team where IsActive=1 and IsDeleted=0";
        public static string GetTeam => " Select Team.Id,Team.Name,Team.IsActive,Location.Id as LocationId,Location.Name as LocationName,ud.firstname+' '+ud.lastname as fullname,team.DateModifiedUtc,(select STRING_AGG(r.name,',') from AspNetRoles r join AspNetUserRoles ar on ar.RoleId=r.Id where ar.UserId=case when team.ModifiedByUserId is not null then team.ModifiedByUserId else team.CreatedByUserId end)as Role from Team left join location on  location.Id=team.LocationId join UserDetail ud on ud.AspNetUserId=case when team.ModifiedByUserId is not null then team.ModifiedByUserId else team.CreatedByUserId end where Team.IsDeleted=0";
        public static string GetAllFilterTeam => "Select* from(Select Team.Id, Team.Name, Team.IsActive, Location.Id as LocationId, Location.Name as LocationName, ud.firstname+' '+ud.lastname as fullname, team.DateModifiedUtc,(select STRING_AGG(r.name,',') from AspNetRoles r join AspNetUserRoles ar on ar.RoleId=r.Id where ar.UserId=case when team.ModifiedByUserId is not null "+
        "then team.ModifiedByUserId else team.CreatedByUserId end)as Role from Team left join location on location.Id=team.LocationId join UserDetail ud on ud.AspNetUserId=case when team.ModifiedByUserId is not null then team.ModifiedByUserId else team.CreatedByUserId end where Team.IsDeleted= 0 and team.IsActive= @Status  and Location.Name like case when @LocationName is not null then @LocationName else '%' end" +
        " and team.Name like case when @Name is not null then @Name else '%' end and (@UpdatedDate is null or cast(team.DateModifiedUtc as date)=@UpdatedDate) and(ud.FirstName like (case when @UpdatedBy is not null then @UpdatedBy else '%' end) or ud.LastName like (case when @UpdatedBy is not null then"+
        " @UpdatedBy else '%' end) or(select STRING_AGG(r.name,',')from AspNetRoles r join AspNetUserRoles ar on ar.RoleId=r.Id where ar.UserId=case when Team.ModifiedByUserId is not null then Team.ModifiedByUserId else Team.CreatedByUserId end) like @UpdatedBy)) as subquery order by subquery.Id";
    }
}
