using LeaveManagement.Domain.Common;
using LeaveManagement.Domain.Enums;

namespace LeaveManagement.Domain.Entities
{

    public class LeaveRequest: AuditableEntity
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public LeaveType LeaveType { get; set; }
        public int LeaveDay { get; set; }
        public LeaveStatus LeaveStatus { get; set; }
        public string Description { get; set; }
    }

}