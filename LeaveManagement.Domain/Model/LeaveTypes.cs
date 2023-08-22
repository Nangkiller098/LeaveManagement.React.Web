namespace LeaveManagement.Domain.Models
{
    public class LeaveTypes : BaseEntity
    {
        public string Name { get; set; }
        public int DefaultDays { get; set; }
    }
}