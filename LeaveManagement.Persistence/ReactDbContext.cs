using LeaveManagement.Domain.Configurations;
using LeaveManagement.Domain.Model;
using LeaveManagement.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class ReactDbContext : IdentityDbContext<ApiUser>
    {
        public ReactDbContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<LeaveTypes> LeaveTypes { get;set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new LeaveTypeConfiguration());
        }
    }
  
}