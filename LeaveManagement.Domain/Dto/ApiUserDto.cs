using System.ComponentModel.DataAnnotations;
namespace LeaveManagement.Domain.Model.Users
{
    public class ApiUserDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set;}
        [Required]
        public string Email { get; set; }
        [Required]
        [StringLength(15,ErrorMessage = "Your Password is limited to {2} to {1} charactoers",MinimumLength =6)]
        public string Password { get; set; }

    }
}