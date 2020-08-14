using LeaveManagement.Domain.Common;
using LeaveManagement.Domain.Enums;

namespace LeaveManagement.Domain.Entities
{
    public class EmployeeDeservedLeave: AuditableEntity
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public LeaveType LeaveType { get; set; }
        public int TotalDay { get; set; }
        public bool NeedManagerApproval { get; set; }
    }
}