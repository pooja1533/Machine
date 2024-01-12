using Hutech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Application.Interfaces
{
    public interface ILocationRepository
    {
        Task<bool> PostLocation(Location location);
        public Task<List<Location>> GetLocation();
        public Task<Location> GetLocationDetail(long Id);
        public Task<String> DeleteLocation(long Id);
        public Task<String> PutLocation(Location aspNetRole);

    }
}
