using LeaveManagement.Application.Contracts;
using LeaveManagement.Domain.Models;
using Persistence;

namespace LeaveManagement.Application.Repositories
{
    public class LeaveTypesRepository : GenericRepository<LeaveTypes>,ILeaveTypesRepository
    {
        public LeaveTypesRepository(ReactDbContext context): base(context)
        {
            
        }
    }
}