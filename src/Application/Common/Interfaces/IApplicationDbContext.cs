using LeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Employee> Employees { get; set; }

        DbSet<EmployeeDeservedLeave> EmployeeDeservedLeaves { get; set; }

        DbSet<LeaveRequest> LeaveRequests { get; set; }

        DbSet<PublicHoliday> PublicHolidays { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
