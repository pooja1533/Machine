using AutoMapper;
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
        }
    }
}
