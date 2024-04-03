using AutoMapper;
using Humanizer;
using Hutech.API.Model;
using Hutech.Core.Entities;
using Hutech.Models;
using System.Reflection.PortableExecutable;

namespace Hutech.API.Mapper
{
    public class AutoMapperConfiguration:Profile
    {
        public AutoMapperConfiguration() 
        {
            CreateMap<RoleViewModel, AspNetRole>();
            CreateMap<AspNetRole, RoleViewModel>();
            CreateMap<AspNetUsers, UserViewModel>();
            CreateMap<UserViewModel,AspNetUsers>();
            CreateMap<MachineViewModel, MachineDetail>();
            CreateMap<MachineDetail, MachineViewModel>();
            CreateMap<MachineComment,MachineCommentViewModel>();
            CreateMap<MachineCommentViewModel, MachineComment>();
            CreateMap<LocationViewModel, Location>();
            CreateMap<Location, LocationViewModel>();
            CreateMap<Department, DepartmentViewModel>();
            CreateMap<DepartmentViewModel, Department>();
            CreateMap<TeamViewModel, Team>();
            CreateMap<Team, TeamViewModel>();
            CreateMap<Instrument, InstrumentViewModel>();
            CreateMap<InstrumentViewModel, Instrument>();
            CreateMap<ActivityViewModel,Activity>();
            CreateMap<Activity, ActivityViewModel>();
            CreateMap<RequirementViewModel, Requirement>();
            CreateMap<Requirement, RequirementViewModel>();
            CreateMap<Group, GroupViewModel>();
            CreateMap<GroupViewModel, Group>();
            CreateMap<InstrumentActivityViewModel, InstrumentActivity>();
            CreateMap<InstrumentActivity, InstrumentActivityViewModel>();
            CreateMap<InstrumentIdViewModel, InstrumentsIds>();
            CreateMap<InstrumentsIds,InstrumentIdViewModel>();
            CreateMap<AuditModels, AuditModel>();
            CreateMap<AuditModel, AuditModels>();
            CreateMap<ActivityDetailsViewModel, ActivityDetails>();
            CreateMap<ActivityDetails, ActivityDetailsViewModel>();
            CreateMap<AuditViewModel, Audit>();
            CreateMap<Audit, AuditViewModel>();
            CreateMap<Configure, ConfigurationViewModel>();
            CreateMap<ConfigurationViewModel, Configure>();
            CreateMap<Document, DocumentViewModel>();
            CreateMap<DocumentViewModel, Document>();
            CreateMap<Document, DocumentViewModel>();
            CreateMap<DocumentViewModel, Document>();
            CreateMap<UserTypeViewModel, UserType>();
            CreateMap<UserType, UserTypeViewModel>();
            CreateMap<UserViewModel, UserDetail>();
            CreateMap<UserDetail, UserViewModel>();
        }
    }
}
