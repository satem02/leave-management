using LeaveManagement.Application.Common.Interfaces;
using LeaveManagement.Domain.Entities;
using LeaveManagement.Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using LeaveManagement.Domain.Enums;
using System;
using System.Linq;

namespace LeaveManagement.Application.Leaves.Commands.CreateLeave
{
    public class CreateLeaveCommand : IRequest<Result>
    {
        public int EmployeeId { get; set; }
        public int RequesterId { get; set; }
        public LeaveType LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class CommandHandler : IRequestHandler<CreateLeaveCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILeaveService _leaveService;
        private readonly IEmployeeSevice _employeeService;
        private readonly INotificationService _notificationService;


        public CommandHandler(IApplicationDbContext context, ILeaveService leaveService, IEmployeeSevice employeeService, INotificationService notificationService = null)
        {
            _context = context;
            _leaveService = leaveService;
            _employeeService = employeeService;
            _notificationService = notificationService;
        }

        public async Task<Result> Handle(CreateLeaveCommand request, CancellationToken cancellationToken)
        {
            var leaveRequestResult = await _leaveService.CheckLeave(request);

            if (!leaveRequestResult.CanUseLeave)
            {
                return Result.Failure("İzin bulunamadı.");
            }

            if (leaveRequestResult.LeaveCount == 0)
            {
                return Result.Failure("Bu tarihte izin kullanılmasına gerek yok.");
            }

            var entity = new LeaveRequest
            {
                LeaveDay = leaveRequestResult.LeaveCount,
                EmployeeId = request.EmployeeId,
                LeaveType = request.LeaveType,
                LeaveStatus = CalculateLeaveStatus(request, leaveRequestResult)
            };

            _context.LeaveRequests.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            if (entity.LeaveStatus == LeaveStatus.Approved)
            {
                await _leaveService.ApproveLeave(leaveRequestResult.Id, leaveRequestResult.LeaveCount, cancellationToken);
                _notificationService.SendNotification(request.EmployeeId, "İzin onaylandı.");
            }
            else
            {
                _notificationService.SendNotification(request.EmployeeId, "İzin Yönetici onayında.");
            }

            return Result.Success(entity.Id);
        }

        private LeaveStatus CalculateLeaveStatus(CreateLeaveCommand request, LeaveRequestResult leaveRequestResult)
        {
            var result = LeaveStatus.Approved;

            if (leaveRequestResult.NeedManagerApproval)
            {
                var employee = _employeeService.GetEmployee(request.EmployeeId);

                if (request.RequesterId == employee.EmployeeManagerId)
                {
                    result = LeaveStatus.Approved;
                }
                else
                {
                    result = LeaveStatus.Created;
                }
            }
            else{

            }

            return result;
        }

    }
}

