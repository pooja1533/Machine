using Dapper;
using Hutech.Application.Interfaces;
using Hutech.Core.Entities;
using Hutech.Sql.Queries;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Infrastructure.Repository
{
    public class MachineRepository : IMachineRepository
    {
        private readonly IConfiguration configuration;
        public MachineRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public async Task<string> DeleteMachine(long Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<MachineDetail>(MachineQueries.DeleteMachine, new { Id = Id });
                    return result.ToString(); ;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<MachineDetail>> GetMachine()
        {
            using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
            {
                var machines = await connection.QueryAsync<MachineDetail>(MachineQueries.GetMachine);
                return machines.ToList();
            }
        }
        public async Task<bool> UpdateMachine(MachineDetail machine)
        {
            using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
            {
                var machines = await connection.ExecuteAsync(MachineQueries.UpdateMachine, machine);
                return true;
            }
        }
        public async Task<MachineDetail> GetMachineById(long Id)
        {
            using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
            {
                var machines = await connection.QueryAsync<MachineDetail>(MachineQueries.GetMachineById,new { Id=Id});
                return machines.First();
            }
        }

        public async Task<bool> PostMachine(MachineDetail machine)
        {
            try
            {
                using(IDbConnection connection=new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    machine.IsActive = true;
                    var data = await connection.QueryAsync<string>(MachineQueries.AddMachine, machine);
                    return true;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> PostComment(MachineComment comment)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    var data = await connection.QueryAsync<string>(MachineQueries.PostComment, comment);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
