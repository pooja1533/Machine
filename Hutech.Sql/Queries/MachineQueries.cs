using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Sql.Queries
{
    public class MachineQueries
    {
        public static string GetMachine => "Select * from MachineDetail where ISActive=1";
        public static string UpdateMachine => "Update MachineDetail set Name=@Name,PurchaseDate=@PurchaseDate,Price=@Price,Brand=@Brand,Vednor=@Vednor,Warranty=@Warranty,ServiceInterval=@ServiceInterval,Interval=@Interval where Id =@Id";
        public static string GetMachineById => "Select * from MachineDetail where Id=@Id";
        public static string DeleteMachine => "update MachineDetail set Isactive=0 where Id=@Id";
        public static string PostComment => "Insert into MachineComment values(@MachineId,@Comment,@CommentDate,@CreatedDateTime,@CreatedBy,@ModifiedDateTime,@ModifiedBy)";
        public static string AddMachine => "Insert into MachineDetail Values(@Name,@PurchaseDate,@Price,@Brand,@Vednor,@Warranty,@IsActive,@ServiceInterval,@Interval)";
    }
}
