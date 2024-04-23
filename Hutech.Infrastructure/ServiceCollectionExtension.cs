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
            service.AddTransient<IInstrumentActivityRepository, InstrumentActivityRepository>();
            service.AddTransient<IGroupRepository,GroupRepository>();
            service.AddTransient<IInstrumentIdRepository,InstrumentIdRepository>();
            service.AddTransient<IAuditRepository, AuditRepository>();
            service.AddTransient<IActivityDetailRepository, ActivityDetailRepository>();
            service.AddTransient<IAuditTrailRepository, AuditTrailRepository>();
            service.AddTransient<IConfigurationRepository, ConfigurationRepository>();
            service.AddTransient<IDocumentRepository, DocumentRepository>();
            service.AddTransient<IUserTypeRepository, UserTypeRepository>();
            service.AddTransient<IMenuRepository, MenuRepository>();
        }
    }
}
