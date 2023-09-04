using LeaveManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagement.Domain.Configurations
{
    public class LeaveTypeConfiguration : IEntityTypeConfiguration<LeaveTypes>
    {
        public void Configure(EntityTypeBuilder<LeaveTypes> builder)
        {
            builder.HasData(
                new LeaveTypes
                {
                    Id = "1FA85F64-5717-4562-B3FC-2C963F66AFA6",
                    DateCreated=DateTime.Now,
                    DateModified=DateTime.Now,
                    Name="Sick Leave",
                    DefaultDays=18,

                },
                new LeaveTypes
                {
                    Id = "2FA85F64-5717-4562-B3FC-2C963F66AFA6",
                    DateCreated=DateTime.Now,
                    DateModified=DateTime.Now,
                    Name="Annual Block Leave",
                    DefaultDays=0,

                },
                new LeaveTypes
                {
                    Id = "3FA85F64-5717-4562-B3FC-2C963F66AFA6",
                    DateCreated=DateTime.Now,
                    DateModified=DateTime.Now,
                    Name="Compassionate Leave",
                    DefaultDays=0,

                },
                new LeaveTypes
                {
                    Id = "4FA85F64-5717-4562-B3FC-2C963F66AFA6",
                    DateCreated=DateTime.Now,
                    DateModified=DateTime.Now,
                    Name="Compensatory Leave",
                    DefaultDays=0,

                },
                new LeaveTypes
                {
                    Id = "5FA85F64-5717-4562-B3FC-2C963F66AFA6",
                    DateCreated=DateTime.Now,
                    DateModified=DateTime.Now,
                    Name="Marriage Leave",
                    DefaultDays=7,

                },
                new LeaveTypes
                {
                    Id = "6FA85F64-5717-4562-B3FC-2C963F66AFA6",
                    DateCreated=DateTime.Now,
                    DateModified=DateTime.Now,
                    Name="On-Duty Leave",
                    DefaultDays=0,

                },
                new LeaveTypes
                {
                    Id = "7FA85F64-5717-4562-B3FC-2C963F66AFA6",
                    DateCreated=DateTime.Now,
                    DateModified=DateTime.Now,
                    Name="Sat-off",
                    DefaultDays=0,

                },
                new LeaveTypes
                {
                    Id = "8FA85F64-5717-4562-B3FC-2C963F66AFA6",
                    DateCreated=DateTime.Now,
                    DateModified=DateTime.Now,
                    Name="Uppaid Leave",
                    DefaultDays=0,

                }
            );
        }
    }
}