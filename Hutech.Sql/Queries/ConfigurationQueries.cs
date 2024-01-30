using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Sql.Queries
{
    public class ConfigurationQueries
    {
        public static string PostConfiguration => "Insert into Configuration values(@FileSize,@FileType)";
        public static string GetAllConfiguration => "Select * from Configuration";
        public static string GetConfigurationDetail => "Select * from Configuration where Id=@Id";
        public static string PutConfiguration => "Update Configuration set FileSize=@FileSize,FileType=@FileType where Id=@Id";

        public static string CheckWhetherConfigurationExist => "Select ID from Configuration";
    }
}
