using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LeaveManagement.Application.Common.Exceptions;
using LeaveManagement.Application.Common.Interfaces;
using LeaveManagement.Application.Leaves.Commands.CreateLeave;
using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Enums;

namespace LeaveManagement.Infrastructure.Services
{
    public class LeaveService : ILeaveService
    {
        private readonly IApplicationDbContext _context;
        private readonly IWorkDayService _workDayService;
        public LeaveService(IApplicationDbContext context, IWorkDayService workDayService)
        {
            _context = context;
            _workDayService = workDayService;
        }

        public void AddDefaultLeave(int employeeId)
        {
            var annualLeave = new EmployeeDeservedLeave
            {
                EmployeeId = employeeId,
                LeaveType = LeaveType.Annual,
                NeedManagerApproval = false,
                TotalDay = 5
            };

            _context.EmployeeDeservedLeaves.Add(annualLeave);

            var sickLeave = new EmployeeDeservedLeave
            {
                EmployeeId = employeeId,
                LeaveType = LeaveType.Sick,
                NeedManagerApproval =true,
                TotalDay = 3
            };

            _context.EmployeeDeservedLeaves.Add(sickLeave);

            _context.SaveChangesAsync(default);
        }

        public async Task<bool> ApproveLeave(int id, int leaveCount, CancellationToken cancellationToken)
        {
            var entity = _context.EmployeeDeservedLeaves.FirstOrDefault(x=>x.Id == id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(LeaveService), id);
            }

            entity.TotalDay -= leaveCount;

            await _context.SaveChangesAsync(cancellationToken);

            return await Task.Run(() =>
              {
                  return true;
              });
        }

        public async Task<LeaveRequestResult> CheckLeave(CreateLeaveCommand command)
        {
            var result = new LeaveRequestResult();
            var availableLeave = _context.EmployeeDeservedLeaves.Where(x => x.EmployeeId == command.EmployeeId && x.LeaveType == command.LeaveType).FirstOrDefault();

            if (availableLeave != null && availableLeave.Id > 0)
            {
                var businessDay = _workDayService.CalculateBusinessDays(command.StartDate, command.EndDate);

                if (availableLeave.TotalDay >= businessDay)
                {
                    result.CanUseLeave = true;
                    result.LeaveCount = businessDay;
                    result.Id = availableLeave.Id;
                    result.NeedManagerApproval = availableLeave.NeedManagerApproval;
                }
            }

            return await Task.Run(() =>
            {
                return result;
            });

        }
    }
}