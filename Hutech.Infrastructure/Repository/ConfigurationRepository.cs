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
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Infrastructure.Repository
{
    public class ConfigurationRepository:IConfigurationRepository
    {
        public IConfiguration configuration;
        public ConfigurationRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public async Task<List<Configure>> GetAllConfiguration()
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Configure>(ConfigurationQueries.GetAllConfiguration);
                    return result.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<Configure> GetConfigurationDetail(long Id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<Configure>(ConfigurationQueries.GetConfigurationDetail, new { Id = Id });
                    return result.First();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> PostConfiguration(Configure configure)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(configuration.GetConnectionString("DBConnection")))
                {
                    connection.Open();
                    if (configure.Id > 0)
                    {
                        var result = await connection.QueryAsync<string>(ConfigurationQueries.PutConfiguration, configure);
                        return true;
                    }
                    else
                    {
                        var Id = await connection.QuerySingleOrDefaultAsync<long>(ConfigurationQueries.CheckWhetherConfigurationExist);
                        long configureId=System.Convert.ToInt64(Id);
                        if (configureId > 0)
                        {
                            configure.Id = configureId;
                            var result = await connection.QueryAsync<string>(ConfigurationQueries.PutConfiguration, configure);
                            return true;
                        }
                        else
                        {
                            var result = await connection.QueryAsync<string>(ConfigurationQueries.PostConfiguration, configure);
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
