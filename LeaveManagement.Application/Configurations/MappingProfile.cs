using AutoMapper;
using LeaveManagement.Domain.Dto;
using LeaveManagement.Domain.Models;

namespace LeaveManagement.Application.MappingProfile
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<LeaveTypes,LeaveTypesVM>().ReverseMap();
        }
    }
}