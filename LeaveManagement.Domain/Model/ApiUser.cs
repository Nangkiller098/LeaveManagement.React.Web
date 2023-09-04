using Microsoft.AspNetCore.Identity;

namespace LeaveManagement.Domain.Model
{
    public class ApiUser:IdentityUser
    {
        public string FristName { get; set; }
        public string LastName  { get; set; }

    }
}