using LeaveManagement.Application.Common.Interfaces;
using LeaveManagement.Domain.Entities;
using LeaveManagement.Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using LeaveManagement.Domain.Enums;
using System;
using System.Linq;

namespace LeaveManagement.Application.Leaves.Commands.UpdateLeave
{
    public class UpdateLeaveCommand : IRequest<Result>
    {
        public int LeaveId { get; set; }
        public bool Approve { get; set; }
        public string Description { get; set; }
    }

    public class CommandHandler : IRequestHandler<UpdateLeaveCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly INotificationService _notificationService;
        private readonly ILeaveService _leaveService;


        public CommandHandler(IApplicationDbContext context, INotificationService notificationService, ILeaveService leaveService)
        {
            _context = context;
            _notificationService = notificationService;
            _leaveService = leaveService;
        }

        public async Task<Result> Handle(UpdateLeaveCommand request, CancellationToken cancellationToken)
        {

            var entity = _context.LeaveRequests.FirstOrDefault(x => x.Id == request.LeaveId);

            if (entity == null)
            {
                return Result.Success("İzin bulunamadı.");
            }

            if (request.Approve && entity.LeaveStatus == LeaveStatus.Approved)
            {
                return Result.Success("Onaylı İzin onaylanamaz.");
            }


            if (request.Approve)
            {
                entity.LeaveStatus = LeaveStatus.Approved;
                entity.Description = request.Description;
            }
            else
            {
                entity.LeaveStatus = LeaveStatus.Rejected;
                entity.Description = request.Description;
            }

            _context.LeaveRequests.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            if (entity.LeaveStatus == LeaveStatus.Approved)
            {                
                await _leaveService.ApproveLeave(entity.Id, entity.LeaveDay, cancellationToken);
                _notificationService.SendNotification(entity.EmployeeId, "İzin onaylandı.");
            }
            else
            {
                var rejectedMessage = string.Format("Yönetici izininizi reddetti. Nedeni ise : {0}", request.Description);
                _notificationService.SendNotification(entity.EmployeeId, rejectedMessage);
            }

            return Result.Success(entity.Id);
        }

    }
}

