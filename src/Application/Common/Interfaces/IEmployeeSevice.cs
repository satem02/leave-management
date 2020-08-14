using LeaveManagement.Domain.Entities;

namespace LeaveManagement.Application.Common.Interfaces
{
    public interface IEmployeeSevice
    {
         Employee GetEmployee(int employeeId);
    }
}