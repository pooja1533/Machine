using Hutech.Core.ApiResponse;
using Hutech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Application.Interfaces
{
    public interface ITeamRepository
    {
        Task<bool> PostTeam(Team team);
        public Task<List<Team>> GetTeam();
        public Task<List<Team>> GetActiveTeam();
        public Task<Team> GetTeamDetail(long Id);
        public Task<String> UpdateTeam(Team team);
        public Task<String> DeleteTeam(long Id);
        public Task<ExecutionResult<GridData<Team>>> GetAllFilterTeam(string? TeamName, int pageNumber, string? updatedBy, string? status, string? updatedDate,string? LocationName);
    }
}
