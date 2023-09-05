using System.ComponentModel.DataAnnotations;
namespace LeaveManagement.Domain.Model.Users
{
    public class ApiUserDto :LoginDto
    {
        [Required]
        public string FristName { get; set; }
        [Required]
        public string LastName { get; set;}
        [Required]
        public string PhoneNumber { get; set; }


    }
}