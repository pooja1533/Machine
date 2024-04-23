using Dapper;
using Hutech.Application.Interfaces;
using Hutech.Core.ApiResponse;
using Hutech.Core.Constants;
using Hutech.Core.Entities;
using Hutech.Sql.Queries;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Infrastructure.Repository
{
    public class MenuRepository : IMenuRepository
    {
        public IConfiguration configuration;
        public MenuRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public async Task<List<Menu>> GetLoggedInUserMenuPermission(Guid loggedInUserId)
        {
            try
            {
                List<Menu> menus = new List<Menu>();
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var roles  =await connection.QueryAsync<AspNetRole>(RoleQueries.GetRoleOfLoggedInUser, new { UserId = loggedInUserId });
                    foreach(var role in roles)
                    {
                        var result = await connection.QueryAsync<Menu>(MenuQueries.GetLoggedInUserMenuPermission, new { Role = role.Id });
                        var totalRecords = result.Count();
                        if (result.Count() > 0)
                        {
                            foreach (var data in result)
                            {
                                Menu list = new Menu();
                                list.Id = data.Id;
                                list.Name = data.Name;
                                list.ParentId = data.ParentId;
                                list.ParentName = data.ParentName;
                                list.Isdeleted = data.Isdeleted;
                                list.URL = data.URL;
                                list.sort = data.sort;
                                menus.Add(list);

                            }

                        }
                        //list = result.ToList();
                    }
                    menus = menus.DistinctBy(x=>x.Id).ToList();
                    return menus;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
