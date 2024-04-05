using Hutech.Core.ApiResponse;
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
        public Task<ExecutionResult<GridData<Location>>> GetLocation(int pageNumber);
        public Task<List<Location>> GetActiveLocation();
        public Task<Location> GetLocationDetail(long Id);
        public Task<String> DeleteLocation(long Id);
        public Task<String> PutLocation(Location aspNetRole);
        public Task<ExecutionResult<GridData<Location>>> GetAllFilterLocation(string? LocationName,int pageNumber,string? updatedBy,string? status,string? updatedDate);

    }
}
