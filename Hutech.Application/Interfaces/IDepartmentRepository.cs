using Hutech.Core.ApiResponse;
using Hutech.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Application.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<bool> PostDepartment(Department location);
        public Task<ExecutionResult<GridData<Department>>> GetDepartment(int pageNumber);
        public Task<List<Department>> GetActiveDepartment();
        public Task<Department> GetDepartmentDetail(long Id);
        public Task<String> DeleteDepartment(long Id);
        public Task<String> PutDepartment(Department aspNetRole);
        public Task<List<Department>> GetAllFilterDepartment(string? DepartmentName, string? updatedBy, string? status, string? updatedDate);
    }
}
