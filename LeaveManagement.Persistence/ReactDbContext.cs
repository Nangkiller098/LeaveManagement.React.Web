using LeaveManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class ReactDbContext : DbContext
    {
        public ReactDbContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<LeaveTypes> LeaveTypes { get;set; }
    }
  
}