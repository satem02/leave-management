using LeaveManagement.Domain.Common;
using LeaveManagement.Domain.Enums;
using System;

namespace LeaveManagement.Domain.Entities
{
    public class Employee : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> EmployeeManagerId { get; set; }
        public Employee EmployeeManager { get; set; }
    }
}
