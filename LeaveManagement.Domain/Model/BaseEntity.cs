namespace LeaveManagement.Domain.Models
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime DateCreated{ get; set; }
        public DateTime DateModified{ get; set; }
    }
}