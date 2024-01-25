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
        public static string UpdateTeam => "Update Team set Name=@Name,IsActive=@IsActive,LocationId=@LocationId where Id=@Id";
        public static string PostTeam => "Insert into Team values(@Name,@LocationId,@isActive,@IsDeleted)";
        public static string DeleteTeam => "update  Team set IsDeleted=1 where Id=@Id";
        public static string GetActiveTeam => "Select * from Team where IsActive=1 and IsDeleted=0";
        public static string GetTeam => "Select Team.Id,Team.Name,Team.IsActive,Location.Id as LocationId,Location.Name as LocationName from Team left join location on location.Id=team.LocationId where Team.IsDeleted=0";
    }
}
