namespace LeaveManagement.Domain.Dto
{
    public class LeaveTypesVM
    {
        public string Id { get; set; }
        public DateTime DateCreated{ get; set; }
        public DateTime DateModified{ get; set; }
        public string Name { get; set; }
        public int DefaultDays { get; set; }
    }
}