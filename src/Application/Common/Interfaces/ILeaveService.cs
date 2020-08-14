using System.Threading;
using System.Threading.Tasks;
using LeaveManagement.Application.Leaves.Commands.CreateLeave;

namespace LeaveManagement.Application.Common.Interfaces
{
    public interface ILeaveService
    {
         Task<LeaveRequestResult> CheckLeave(CreateLeaveCommand command);
         Task<bool> ApproveLeave(int id, int leaveCount , CancellationToken cancellationToken);
         void AddDefaultLeave(int employeeId);
    }
}