using Hutech.Application.Interfaces;
using Hutech.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hutech.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static void RegisterServices(this IServiceCollection service)
        {
            service.AddTransient<IRoleRepository, RoleRepository>();
            service.AddTransient<IUserRepository, UserRepository>();   
            service.AddTransient<IMachineRepository, MachineRepository>();
            service.AddTransient<ILocationRepository, LocationRepository>();
            service.AddTransient<IDepartmentRepository, DepartmentRepository>();
            service.AddTransient<ITeamRepository, TeamRepository>();
            service.AddTransient<IInstrumentRepository, InstrumentRepository>();
            service.AddTransient<IActivityRepository, ActivityRepository>();
            service.AddTransient<IRequirementRepository, RequirementRepository>();
        }
    }
}
