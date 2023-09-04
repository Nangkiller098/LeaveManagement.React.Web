using AutoMapper;
using LeaveManagement.Domain.Dto;
using LeaveManagement.Domain.Model;
using LeaveManagement.Domain.Model.Users;
using LeaveManagement.Domain.Models;

namespace LeaveManagement.Application.MappingProfile
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<LeaveTypes,LeaveTypesVM>().ReverseMap();
            CreateMap<ApiUserDto,ApiUser>().ReverseMap();
        }
    }
}