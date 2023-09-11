using AutoMapper;
using LeaveManagement.Application.Contracts;
using LeaveManagement.Domain.Models;
using Persistence;

namespace LeaveManagement.Application.Repositories
{
    public class LeaveTypesRepository : GenericRepository<LeaveTypes>,ILeaveTypesRepository
    {
        private readonly IMapper _mapper;
        public LeaveTypesRepository(ReactDbContext context,IMapper mapper): base(context,mapper)
        {
            _mapper = mapper;
            
        }
    }
}