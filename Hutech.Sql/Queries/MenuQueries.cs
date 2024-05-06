using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Sql.Queries
{
    public class MenuQueries
    {
        public static string GetLoggedInUserMenuPermission => " Select up.Id,up.RoleId,up.MenuId,m.Id,m.Name,m.URL,m.ParentId,m.Isdeleted,m.sort, case when m.ParentId > 0 then (select name from menu where Id=m.ParentId)else null end as parentName from UserMenuPermission up join menu m on m.Id=up.menuId where RoleId=@Role and up.Isdeleted=0 and up.isactive=1 and m.Isdeleted=0";
    }
}
