using System.Linq;
using LeaveManagement.Application.Common.Interfaces;
using LeaveManagement.Domain.Entities;

namespace LeaveManagement.Infrastructure.Services
{
    public class EmployeeSevice : IEmployeeSevice
    {
        private readonly IApplicationDbContext _context;
        public EmployeeSevice(IApplicationDbContext context)
        {
            _context = context;
        }
        public Employee GetEmployee(int employeeId)
        {
            return _context.Employees.FirstOrDefault(x => x.Id == employeeId);
        }
    }
}