using Hutech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Application.Interfaces
{
    public interface IConfigurationRepository
    {
        Task<bool> PostConfiguration(Configure configuration);
        public Task<List<Configure>> GetAllConfiguration();
        public Task<Configure> GetConfigurationDetail(long Id);

    }
}
