using AutoMapper;
using PolyclinicProject.Domain.Entities;
using PolyclinicProject.WebUI.Models.Role;
using PolyclinicProject.WebUI.Models.User;

namespace PolyclinicProject.WebUI.Models
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Polyclinic, PolyclinicModel>().ReverseMap();
            CreateMap<Position, PositionModel>().ReverseMap();
            CreateMap<RoleInfo, CreateRoleModel>().ReverseMap();
            CreateMap<RoleInfo, EditRoleModel>().ReverseMap();
            CreateMap<RegisterModel, UserInfo>().ReverseMap();
            CreateMap<EditModel, UserInfo>().ReverseMap();
            CreateMap<PersonalModel, Personal>().ReverseMap();
            CreateMap<PersonalModel, UserInfo>().ReverseMap();

            CreateMap<Personal, PersonalModel>()
                .ForMember(dest => dest.Birthday, opts => opts.MapFrom(src => src.UserInfo.Birthday))
                .ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.UserInfo.FirstName))
                .ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.UserInfo.LastName))
                .ForMember(dest => dest.SurName, opts => opts.MapFrom(src => src.UserInfo.SurName))
                .ForMember(dest => dest.PhoneNumber, opts => opts.MapFrom(src => src.UserInfo.PhoneNumber))
                .ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.UserInfo.Email))
                .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.UserInfo.UserName))
                .ForMember(dest => dest.IsActive, opts => opts.MapFrom(src => src.UserInfo.IsActive))
                .ForMember(dest => dest.DateRegistration, opts => opts.MapFrom(src => src.UserInfo.DateRegistration))
                .ForMember(dest => dest.RoleId, opts => opts.MapFrom(src => src.UserInfo.RoleInfoId))
                .ForMember(dest => dest.Password, opts => opts.MapFrom(src => src.UserInfo.Password))
                .ForMember(dest => dest.Sex, opts => opts.MapFrom(src => src.UserInfo.Sex))
                .ForMember(dest => dest.UserInfoId, opts => opts.MapFrom(src => src.UserInfo.Id))
                .ForMember(dest => dest.Address, opts => opts.MapFrom(src => src.UserInfo.Address))
                .ForMember(dest => dest.PositionId, opts => opts.MapFrom(src => src.PositionId))
                .ForMember(dest => dest.PolyclinicId, opts => opts.MapFrom(src => src.PolyclinicId))
                ;

       
            CreateMap<Schedule, ScheduleModel>().ReverseMap();
        }
    }
}