using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using LeaveManagement.Application.Common.Interfaces;

namespace LeaveManagement.Application.Leaves.Commands.UpdateLeave
{
    public class UpdateLeaveCommandValidator : AbstractValidator<UpdateLeaveCommand>
    {
        private readonly IWorkDayService _workDayService;
        public UpdateLeaveCommandValidator(IWorkDayService workDayService)
        {
            _workDayService = workDayService;
        }
        public UpdateLeaveCommandValidator()
        {
            RuleFor(v => v.LeaveId)
                .NotEmpty();
        }

    }
}
